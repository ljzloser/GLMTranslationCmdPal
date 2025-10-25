// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GLMTranslation.Commands;
using GLMTranslation.Helpers;
using GLMTranslation.Models;
using GLMTranslation.Properties;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace GLMTranslation;

internal sealed partial class GLMTranslationPage : DynamicListPage, IDisposable, IFallbackHandler
{
    /// <summary>
    /// 用于显示的命令列表
    /// </summary>
    private readonly List<IListItem> items = [];

    private readonly HttpClient httpClient = new();

    /// <summary>
    /// 线程锁主要是保护取消令牌
    /// </summary>
    private readonly Lock @lock = new();

    /// <summary>
    /// http请求的取消令牌
    /// </summary>
    private CancellationTokenSource _cts = new();

    public GLMTranslationPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "GLMTranslation";
        Name = "Open";
        //httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public void Dispose()
    {
        httpClient?.Dispose();
        _cts?.Dispose();
    }

    public override IListItem[] GetItems()
    {
        return [.. items];
    }

    public void UpdateQuery(string query)
    {
        // 如果正在处理，或者查询为空，直接返回
        if (IsLoading || string.IsNullOrEmpty(query)) return;
        IsLoading = true;
        items.Clear();
        try
        {
            // 先检查APIKey，如果有就请求API
            if (ValidateApiKey()) RequestQuery(query);
        }
        catch (Exception ex)
        {
            items.Add(new ListItem(new CommandItem($"{ex.GetType().Name}: {ex.Message}")));
        }
        IsLoading = false;
        // 通知命令面板更新
        RaiseItemsChanged(items.Count);
    }

    /// <summary>
    /// 检查APIKey，如果没有就添加一个能打开链接的命令
    /// </summary>
    /// <returns></returns>
    private bool ValidateApiKey()
    {
        // 如果APIKey不为空，直接返回
        if (!string.IsNullOrEmpty(Instance.Settings.ApiKey))
        {
            return true;
        }
        // 没有APIKey，添加一个能打开链接的命令
        var comand = new OpenUrlCommand("https://bigmodel.cn/usercenter/proj-mgmt/apikeys");
        items.Add(new ListItem(comand)
        {
            Title = Resources.GetApiKeyUrlInfo,
            Subtitle = "https://bigmodel.cn/usercenter/proj-mgmt/apikeys"
        });
        return false;
    }

    /// <summary>
    /// 请求API并根据返回结果添加命令
    /// </summary>
    /// <param name="query"></param>
    private void RequestQuery(string query)
    {
        // 创建取消令牌，用于取消上一个请求
        CancellationTokenSource cts;
        lock (@lock)
        {
            _cts.Cancel();
            _cts = new();
            cts = _cts;
        }
        var requestModel = BuildRequest(query);
        string requestContent = JsonSerializer.Serialize(requestModel, ModelContext.Default.RequestModel);
        var request = new HttpRequestMessage(HttpMethod.Post, Instance.Settings.Url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Instance.Settings.ApiKey);
        request.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");
        var response = httpClient.SendAsync(request, cts.Token).Result;
        // 判断状态码，如果不是200，添加一个错误命令，正常情况下添加翻译命令
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            var responseContent = response.Content.ReadAsStringAsync(cts.Token).Result;
            var comand = new CommandItem($"Error: {response.StatusCode} message {responseContent}");
            items.Add(new ListItem(comand));
        }
        else
        {
            var responseContent = response.Content.ReadAsStringAsync(cts.Token).Result;
            AddResponseItems(responseContent);
        }
    }

    /// <summary>
    /// 添加翻译结果的命令
    /// </summary>
    /// <param name="responseContent">响应结果</param>
    private void AddResponseItems(string responseContent)
    {
        // 这里就是一些解析JSON的逻辑
        JsonDocument jsonDocument = JsonDocument.Parse(responseContent);
        var choices = jsonDocument.RootElement.GetProperty("choices").EnumerateArray();
        var choice = choices.FirstOrDefault();
        var content = choice.GetProperty("message").GetProperty("content").GetString() ?? "";
        var json = JsonDocument.Parse(content);
        // 判断json是否为空
        if (json.RootElement.ValueKind == JsonValueKind.Null)
        {
            items.Add(new ListItem(new CommandItem(content)));
            return;
        }
        var root = json.RootElement;
        ShowDetails = Instance.Settings.Explain;
        foreach (var obj in root.EnumerateObject())
        {
            var key = obj.Name;
            var value = obj.Value;
            var text = value.GetProperty("text").GetString() ?? "";

            var item = new ListItem(new CopyCommand(text))
            {
                Title = text.Length > 50 ? text[..50] + "..." : text,
                Subtitle = key,
                MoreCommands = Instance.LoadCommands(text),
            };
            if (ShowDetails)
            {
                var other = value.GetProperty("other").GetString() ?? "";
                item.Details = new Details
                {
                    Title = Resources.Explain,
                    Body = other,
                    Metadata = [
                        new DetailsElement()
                        {
                            Key = Resources.Complete_Translation,
                            Data = new DetailsLink(){
                                Text = text,
                            }
                        }
                        ]
                };
            }
            items.Add(item);
        }
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        if (Instance.Settings.SpaceFinish && newSearch.Length > 0 && newSearch[^1] != ' ')
            return;
        var query = newSearch.Trim();
        if (string.IsNullOrEmpty(query)) return;
        UpdateQuery(query);
    }

    /// <summary>
    /// 构建请求
    /// </summary>
    /// <param name="model">模型名称</param>
    /// <param name="text">待翻译文本</param>
    /// <returns></returns>
    private static RequestModel BuildRequest(string text)
    {
        // 提示词模板
        List<string> modes = [.. (Instance.Settings.TargetModes).Split(',')];
        Dictionary<string, Dictionary<string, string>> dic = [];
        var output_struct = new Dictionary<string, string>
        {
            ["text"] = ""
        };
        if (Instance.Settings.Explain)
        {
            output_struct["other"] = "";
        }
        // 将翻译的模式转换为字典
        foreach (var mode in modes)
        {
            dic[mode] = output_struct;
        }
        // 将待翻译的语言和模式转换为列表
        var args = new[]
        {
             Instance.Settings.SourceLanguage,
             Instance.Settings.TargetLanguage,
             JsonSerializer.Serialize(dic,ModelContext.Default.DictionaryStringDictionaryStringString)
        };
        var system_text = string.Format(System.Globalization.CultureInfo.InvariantCulture, Instance.SystemText ?? "", args);
        return new RequestModel
        {
            Model = Instance.Settings.Model,
            Messages = [
                    new MessageModel { Role = "system", Content = system_text },
                        new MessageModel { Role = "user", Content = text }
                ]
        };
    }
}
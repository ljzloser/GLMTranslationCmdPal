# GLMTranslation 项目说明

## 项目概述

GLMTranslation 是一个 Windows 平台的 Command Palette 扩展，使用 .NET 9 构建。该项目旨在将智谱清言 (Zhipu AI) 的 GLM 大语言模型能力集成到 Windows Command Palette 中，为用户提供便捷的 AI 驱动翻译服务。

主要特性：
- 实时翻译：用户可以输入文本，实时获得 AI 模型的翻译结果
- 多语言支持：支持源语言和目标语言之间的双向翻译
- 翻译模式：支持多种翻译类型（如专业、简洁等）
- 详细解释：可选项，提供翻译内容的详细解释
- API 集成：通过智谱清言 API 提供翻译服务
- 快捷操作：支持复制翻译结果和语音朗读功能

项目基于 Windows App SDK 构建，采用 MSIX 打包格式，可在 Windows 10/11 上运行。

## 技术架构

### 主要技术栈
- **语言**: C# (.NET 9)
- **框架**: Windows App SDK, Microsoft Command Palette Extensions
- **打包**: MSIX
- **API**: 智谱清言 API (基于 OpenAI 兼容格式)
- **UI**: Windows Command Palette 集成界面

### 核心组件
1. **GLMTranslation.cs**: 主扩展类，实现 IExtension 接口
2. **GLMTranslationCommandsProvider.cs**: 命令提供者，注册扩展到 Command Palette
3. **GLMTranslationPage.cs**: 主要页面类，处理翻译逻辑和 UI 更新
4. **SettingsManager.cs**: 管理应用设置和用户偏好
5. **RequestModel.cs**: 定义 API 请求的数据模型
6. **Commands/**: 包含 CopyCommand 和 SpeechCommand 实现

## 构建和运行

### 系统要求
- Windows 10 (版本 19041.0 或更高)
- .NET 9 SDK
- Visual Studio 2022 或更高版本 (推荐)

### 构建步骤
1. 确保已安装 .NET 9 SDK 和 Visual Studio 2022
2. 打开 GLMTranslation.sln 解决方案文件
3. 恢复 NuGet 包 (构建时会自动进行)
4. 选择目标平台 (x64, ARM64) 和配置 (Debug/Release)
5. 构建解决方案

### 运行说明
1. 首次运行前需要在设置中输入智谱清言的 API Key
2. 应用作为 Command Palette 扩展运行，集成到 Windows 命令面板中
3. 使用 `-RegisterProcessAsComServer` 参数启动扩展

### 依赖项
- Microsoft.CommandPalette.Extensions
- Microsoft.Windows.CsWinRT
- Shmuelie.WinRTServer
- System.Speech
- Microsoft.Windows.SDK.BuildTools.MSIX

## 开发约定

### 代码结构
- **Pages/**: 包含页面和界面逻辑
- **Commands/**: 定义用户可执行的命令
- **Helpers/**: 包含通用工具和设置管理
- **Models/**: 数据模型定义
- **Properties/**: 应用程序属性、资源和设置

### 本地化
- 支持中文 (zh-CN) 和英文 (en-US) 本地化
- 使用 .resx 文件管理资源
- 资源文件位于 Properties/ 目录下

### 设置管理
- 使用 JSON 格式存储用户设置
- 设置文件保存在 `%AppData%\Microsoft.CmdPal\GLMTranslation.json`
- 支持以下设置：
  - 模型名称 (默认: glm-4.5-flash)
  - 源语言 (默认: 中文)
  - 目标语言 (默认: 英文)
  - 翻译模式 (默认: 专业的,简洁的)
  - API Key (必须由用户设置)
  - API URL (默认: 智谱清言 API 地址)
  - 空格完成开关 (默认: true)
  - 解释功能开关 (默认: true)

## 功能说明

### 翻译功能
1. 用户在 Command Palette 中输入待翻译文本
2. 应用根据设置构建 AI 请求，包含系统提示词和用户输入
3. 发送请求到智谱清言 API
4. 解析返回的 JSON 结果
5. 在 Command Palette 中显示翻译结果

### 系统提示词
应用使用预定义的系统提示词，包含：
- 角色定义 (专业翻译助手)
- 翻译要求 (双向翻译、JSON 输出格式、保持风格等)
- 输入输出格式规范

### 用户交互
- 支持空格结束输入模式，减少不必要的 API 请求
- 翻译结果可复制到剪贴板
- 支持语音朗读功能
- 详细解释模式提供翻译说明

### API 集成
- 使用 HTTPS 与智谱清言 API 通信
- 通过 Bearer Token 进行身份验证
- 支持流式和非流式响应
- 包含请求超时和错误处理机制
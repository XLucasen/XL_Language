# XL_Language - WPF多语言支持类库

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> **作者**: 李枭龙 (Lucas__sen)  
> **官网**: [枭龙网络](https://0554h.com)  
>> **源码**: [GitHub](https://github.com/XLucasen/XL_Language)  **源码**: [GitHub](https://github.com/XLucasen/XL_Language)  
> **邮箱**: 332555220@qq.com

一个功能强大的C# WPF多语言支持类库，支持动态切换语言而无需重启应用，提供简洁的API和完善的MVVM支持。

## 📋 目录

- [特性](#-特性)
- [安装](#-安装)
- [快速开始](#-快速开始)
- [基础使用](#-基础使用)
- [XAML绑定](#-xaml绑定)
- [MVVM支持](#-mvvm支持)
- [语言文件管理](#-语言文件管理)
- [高级功能](#-高级功能)
- [API参考](#-api参考)
- [最佳实践](#-最佳实践)
- [故障排除](#-故障排除)
- [更新日志](#-更新日志)
- [许可证](#-许可证)

## ✨ 特性

- 🚀 **动态语言切换** - 无需重启应用即可切换语言
- 📁 **JSON格式** - 使用JSON格式存储语言文件，易于编辑和维护
- 🎯 **简洁API** - 提供 `Lang.Set()` 和 `Lang.Get()` 等简洁方法
- 🔗 **XAML绑定** - 支持在XAML中直接绑定多语言文本
- 🔄 **自动更新** - 语言切换时自动更新所有UI元素
- 🏗️ **MVVM支持** - 完整的MVVM架构支持
- ⚡ **异步支持** - 支持异步语言切换
- 🔍 **调试友好** - 找不到键时显示 `[键名]` 格式，便于调试
- ✅ **文件验证** - 内置语言文件验证和比较功能
- 🎨 **扩展方法** - 支持 `.L()` 扩展方法语法

## 📦 安装

### 通过NuGet安装

```bash
Install-Package XL_Language
```

### 手动安装

1. 下载 `XL_Language.dll` 文件
2. 在项目中添加引用
3. 确保项目已安装 `Newtonsoft.Json` 依赖

## 🚀 快速开始

### 1. 初始化多语言系统

在应用程序启动时（通常是 `App.xaml.cs` 或 `MainWindow.xaml.cs`）初始化：

```csharp
using XL_Language;

// 在应用程序启动时初始化
LanguageHelper.Initialize("Language", "Chinese");
```

### 2. 创建语言文件

在项目根目录创建 `Language` 文件夹，并添加语言文件：

**Language/Chinese.json**:
```json
{
  "App": {
    "Name": "我的应用程序",
    "Title": "主窗口标题",
    "Version": "版本 1.0.0"
  },
  "Common": {
    "OK": "确定",
    "Cancel": "取消",
    "Yes": "是",
    "No": "否",
    "Save": "保存",
    "Load": "加载"
  },
  "Message": {
    "Welcome": "欢迎使用本软件",
    "Goodbye": "感谢使用，再见！"
  },
  "Error": {
    "FileNotFound": "文件未找到",
    "NetworkError": "网络连接错误"
  }
}
```

**Language/English.json**:
```json
{
  "App": {
    "Name": "My Application",
    "Title": "Main Window Title",
    "Version": "Version 1.0.0"
  },
  "Common": {
    "OK": "OK",
    "Cancel": "Cancel",
    "Yes": "Yes",
    "No": "No",
    "Save": "Save",
    "Load": "Load"
  },
  "Message": {
    "Welcome": "Welcome to our software",
    "Goodbye": "Thank you for using, goodbye!"
  },
  "Error": {
    "FileNotFound": "File not found",
    "NetworkError": "Network connection error"
  }
}
```

### 3. 基本使用

```csharp
// 设置语言
Lang.Set("Chinese");
Lang.Set("English");

// 获取文本
string appName = Lang.Get("App.Name");
string buttonText = Lang.Get("Common.OK");

// 使用扩展方法（更简洁）
string appName = "App.Name".L();
string buttonText = "Common.OK".L();
```

## 📖 基础使用

### 语言切换

```csharp
// 同步切换语言
Lang.Set("Chinese");
Lang.Set("English");

// 异步切换语言
await Lang.SetAsync("Chinese");
await Lang.SetAsync("English");
```

### 文本获取

```csharp
// 基本用法
string text = Lang.Get("App.Title");

// 带默认值
string text = Lang.Get("App.Title", "默认标题");

// 使用扩展方法
string text = "App.Title".L();
string text = "App.Title".L("默认标题");
```

### 嵌套键访问

支持使用点分隔符访问嵌套的JSON结构：

```csharp
// 访问嵌套键
string appName = Lang.Get("App.Name");        // 我的应用程序
string version = Lang.Get("App.Version");     // 版本 1.0.0
string okText = Lang.Get("Common.OK");        // 确定
```

### 错误处理

当找不到对应的键时，会显示 `[键名]` 格式：

```csharp
string text = Lang.Get("NonExistent.Key");    // 返回: [NonExistent.Key]
string text = Lang.Get("NonExistent.Key", "默认值");  // 返回: 默认值
```

## 🎨 XAML绑定

### 使用Lang标记扩展

在XAML中直接使用多语言绑定：

```xml
<Window xmlns:lang="clr-namespace:XL_Language;assembly=XL_Language"
        Title="{lang:Lang App.Title}"
        Height="450" Width="800">
    <Grid>
        <StackPanel>
            <!-- 基本绑定 -->
            <TextBlock Text="{lang:Lang App.Name}"/>
            
            <!-- 带默认值的绑定 -->
            <TextBlock Text="{lang:Lang Message.Welcome, DefaultValue=欢迎}"/>
            
            <!-- 按钮文本绑定 -->
            <Button Content="{lang:Lang Common.OK}"/>
            <Button Content="{lang:Lang Common.Cancel}"/>
            
            <!-- 菜单项绑定 -->
            <Menu>
                <MenuItem Header="{lang:Lang Common.Save}"/>
                <MenuItem Header="{lang:Lang Common.Load}"/>
            </Menu>
        </StackPanel>
    </Grid>
</Window>
```

### 使用LocalizedBinding转换器

```xml
<Window xmlns:lang="clr-namespace:XL_Language;assembly=XL_Language"
        Title="{Binding 'App.Title', Converter={x:Static lang:LocalizedBindingExtensions.Converter}}">
    <Grid>
        <TextBlock Text="{Binding 'Message.Welcome', Converter={x:Static lang:LocalizedBindingExtensions.Converter}}"/>
        <Button Content="{Binding 'Common.OK', Converter={x:Static lang:LocalizedBindingExtensions.Converter}}"/>
    </Grid>
</Window>
```

### 动态语言切换按钮

```xml
<StackPanel Orientation="Horizontal">
    <Button Content="中文" Click="SwitchToChinese"/>
    <Button Content="English" Click="SwitchToEnglish"/>
</StackPanel>
```

```csharp
private void SwitchToChinese(object sender, RoutedEventArgs e)
{
    Lang.Set("Chinese");
}

private void SwitchToEnglish(object sender, RoutedEventArgs e)
{
    Lang.Set("English");
}
```

## 🏗️ MVVM支持

### 使用LocalizedString类

```csharp
public class MainViewModel : INotifyPropertyChanged
{
    private LocalizedString _welcomeMessage;
    private LocalizedString _appTitle;
    
    public MainViewModel()
    {
        // 创建本地化字符串
        _welcomeMessage = new LocalizedString("Message.Welcome");
        _appTitle = new LocalizedString("App.Title");
    }
    
    // 属性绑定
    public LocalizedString WelcomeMessage => _welcomeMessage;
    public LocalizedString AppTitle => _appTitle;
    
    // 清理资源
    public void Dispose()
    {
        _welcomeMessage?.Dispose();
        _appTitle?.Dispose();
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
}
```

### XAML中的MVVM绑定

```xml
<Window DataContext="{Binding MainViewModel}">
    <Grid>
        <TextBlock Text="{Binding WelcomeMessage.Value}"/>
        <TextBlock Text="{Binding AppTitle.Value}"/>
    </Grid>
</Window>
```

### 使用LocalizedStringFactory

```csharp
public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly LocalizedString _saveButtonText;
    private readonly LocalizedString _cancelButtonText;
    
    public SettingsViewModel()
    {
        _saveButtonText = LocalizedStringFactory.Create("Common.Save");
        _cancelButtonText = LocalizedStringFactory.Create("Common.Cancel");
    }
    
    public LocalizedString SaveButtonText => _saveButtonText;
    public LocalizedString CancelButtonText => _cancelButtonText;
}
```

## 📁 语言文件管理

### 文件结构

推荐的语言文件目录结构：

```
项目根目录/
├── Language/
│   ├── Chinese.json
│   ├── English.json
│   ├── Japanese.json
│   └── Korean.json
└── ...
```

### 自定义语言目录

```csharp
// 设置自定义语言目录
Lang.SetLanguageDirectory("MyLanguages");
LanguageHelper.Initialize("MyLanguages", "Chinese");
```

### 语言文件验证

```csharp
// 验证语言文件格式
var result = LanguageHelper.ValidateLanguageFile("Chinese");
if (result.IsValid)
{
    Console.WriteLine("语言文件有效");
}
else
{
    Console.WriteLine($"错误: {result.ErrorMessage}");
}
```

### 语言文件比较

```csharp
// 比较两个语言文件的键值差异
var comparison = LanguageHelper.CompareLanguages("Chinese", "English");
Console.WriteLine($"中文缺少的键: {comparison.MissingInLanguage1.Count}");
Console.WriteLine($"英文缺少的键: {comparison.MissingInLanguage2.Count}");
Console.WriteLine($"共同键数量: {comparison.CommonKeys.Count}");
```

### 获取可用语言

```csharp
// 获取所有可用的语言
var languages = LanguageHelper.GetAvailableLanguages();
foreach (var lang in languages)
{
    Console.WriteLine($"可用语言: {lang}");
}
```

## 🔧 高级功能

### 异步语言切换

```csharp
public async Task SwitchLanguageAsync(string language)
{
    try
    {
        await Lang.SetAsync(language);
        Console.WriteLine($"语言已切换到: {language}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"语言切换失败: {ex.Message}");
    }
}
```

### 语言切换事件监听

```csharp
public class LanguageAwareViewModel : INotifyPropertyChanged
{
    public LanguageAwareViewModel()
    {
        // 订阅语言切换事件
        Lang.Instance.LanguageChanged += OnLanguageChanged;
    }
    
    private void OnLanguageChanged(object sender, LanguageChangedEventArgs e)
    {
        Console.WriteLine($"语言已切换到: {e.Language}");
        // 在这里可以执行语言切换后的自定义逻辑
        OnPropertyChanged(nameof(SomeProperty));
    }
    
    public void Dispose()
    {
        Lang.Instance.LanguageChanged -= OnLanguageChanged;
    }
}
```

### 动态键生成

```csharp
public class DynamicLocalization
{
    public string GetLocalizedText(string category, string key)
    {
        return Lang.Get($"{category}.{key}");
    }
    
    public string GetLocalizedText(string category, string subcategory, string key)
    {
        return Lang.Get($"{category}.{subcategory}.{key}");
    }
}

// 使用示例
var localization = new DynamicLocalization();
string text = localization.GetLocalizedText("Error", "NetworkError");  // Error.NetworkError
```

### 条件本地化

```csharp
public class ConditionalLocalization
{
    public string GetMessage(bool isError, string key)
    {
        string category = isError ? "Error" : "Message";
        return Lang.Get($"{category}.{key}");
    }
}

// 使用示例
var conditional = new ConditionalLocalization();
string errorMsg = conditional.GetMessage(true, "FileNotFound");   // Error.FileNotFound
string infoMsg = conditional.GetMessage(false, "Welcome");        // Message.Welcome
```

## 📚 API参考

### Lang类

主要的语言管理类，提供静态方法进行语言切换和文本获取。

#### 静态方法

| 方法 | 描述 | 参数 | 返回值 |
|------|------|------|--------|
| `Set(string language)` | 设置当前语言 | `language`: 语言名称 | `void` |
| `Get(string key, string defaultValue = null)` | 获取本地化文本 | `key`: 键名, `defaultValue`: 默认值 | `string` |
| `SetAsync(string language)` | 异步设置语言 | `language`: 语言名称 | `Task` |
| `SetLanguageDirectory(string directory)` | 设置语言文件目录 | `directory`: 目录路径 | `void` |
| `GetAvailableLanguages()` | 获取所有可用的语言 | 无 | `List<string>` |

#### 实例属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `CurrentLanguage` | `string` | 当前语言 |
| `LanguageDirectory` | `string` | 语言文件目录 |

#### 事件

| 事件 | 类型 | 描述 |
|------|------|------|
| `LanguageChanged` | `EventHandler<LanguageChangedEventArgs>` | 语言切换事件 |
| `PropertyChanged` | `PropertyChangedEventHandler` | 属性更改事件 |

### StringExtensions类

提供字符串扩展方法，支持 `.L()` 语法。

#### 方法

| 方法 | 描述 | 参数 | 返回值 |
|------|------|------|--------|
| `L(this string key)` | 获取本地化文本 | `key`: 本地化键名 | `string` |
| `L(this string key, string defaultValue)` | 获取本地化文本（带默认值） | `key`: 本地化键名, `defaultValue`: 默认值 | `string` |

### LocalizedString类

支持MVVM绑定的本地化字符串类。

#### 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `Key` | `string` | 本地化键名 |
| `Value` | `string` | 本地化文本值 |

#### 方法

| 方法 | 描述 | 参数 | 返回值 |
|------|------|------|--------|
| `LocalizedString(string key)` | 构造函数 | `key`: 本地化键名 | - |
| `Dispose()` | 释放资源 | 无 | `void` |

### LanguageHelper类

提供便捷的语言管理功能。

#### 静态方法

| 方法 | 描述 | 参数 | 返回值 |
|------|------|------|--------|
| `Initialize(string languageDirectory, string defaultLanguage)` | 初始化多语言系统 | `languageDirectory`: 语言目录, `defaultLanguage`: 默认语言 | `void` |
| `InitializeAsync(string languageDirectory, string defaultLanguage)` | 异步初始化 | `languageDirectory`: 语言目录, `defaultLanguage`: 默认语言 | `Task` |
| `IsLanguageAvailable(string language)` | 检查语言文件是否存在 | `language`: 语言名称 | `bool` |
| `GetAvailableLanguages()` | 获取所有可用的语言 | 无 | `List<string>` |
| `ValidateLanguageFile(string language)` | 验证语言文件格式 | `language`: 语言名称 | `LanguageValidationResult` |
| `CompareLanguages(string language1, string language2)` | 比较两个语言文件的键值差异 | `language1`: 语言1, `language2`: 语言2 | `LanguageComparisonResult` |

### LocalizedBinding类

XAML绑定转换器，用于在XAML中直接绑定多语言文本。

#### 方法

| 方法 | 描述 | 参数 | 返回值 |
|------|------|------|--------|
| `Convert(object value, Type targetType, object parameter, CultureInfo culture)` | 将本地化键名转换为本地化文本 | `value`: 键名, `targetType`: 目标类型, `parameter`: 参数, `culture`: 文化信息 | `object` |
| `ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)` | 反向转换（不支持） | - | `NotImplementedException` |

## 💡 最佳实践

### 1. 语言文件组织

```json
{
  "App": {
    "Name": "应用程序名称",
    "Title": "窗口标题",
    "Version": "版本信息"
  },
  "Common": {
    "OK": "确定",
    "Cancel": "取消",
    "Save": "保存",
    "Load": "加载"
  },
  "Menu": {
    "File": {
      "New": "新建",
      "Open": "打开",
      "Save": "保存",
      "Exit": "退出"
    },
    "Edit": {
      "Cut": "剪切",
      "Copy": "复制",
      "Paste": "粘贴"
    }
  },
  "Message": {
    "Success": "操作成功",
    "Error": "操作失败",
    "Warning": "警告",
    "Info": "提示"
  },
  "Error": {
    "FileNotFound": "文件未找到",
    "NetworkError": "网络连接错误",
    "PermissionDenied": "权限不足"
  }
}
```

### 2. 键名命名规范

- 使用有意义的键名
- 使用点分隔符组织层次结构
- 保持键名的一致性
- 使用PascalCase命名法

```csharp
// 好的命名
"App.Title"
"Common.OK"
"Menu.File.New"
"Error.FileNotFound"

// 避免的命名
"title"
"ok"
"new"
"error1"
```

### 3. 错误处理

```csharp
public class SafeLocalization
{
    public static string GetSafeText(string key, string fallback = null)
    {
        try
        {
            string result = Lang.Get(key);
            // 检查是否返回了未找到的键格式
            if (result.StartsWith("[") && result.EndsWith("]"))
            {
                return fallback ?? key;
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"获取本地化文本失败: {ex.Message}");
            return fallback ?? key;
        }
    }
}
```

### 4. 性能优化

```csharp
public class CachedLocalization
{
    private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
    
    public string GetCachedText(string key)
    {
        if (_cache.TryGetValue(key, out string cachedValue))
        {
            return cachedValue;
        }
        
        string value = Lang.Get(key);
        _cache[key] = value;
        return value;
    }
    
    public void ClearCache()
    {
        _cache.Clear();
    }
}
```

### 5. 资源管理

```csharp
public class LocalizedViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly List<LocalizedString> _localizedStrings = new List<LocalizedString>();
    
    public LocalizedViewModel()
    {
        // 创建本地化字符串
        _localizedStrings.Add(new LocalizedString("App.Title"));
        _localizedStrings.Add(new LocalizedString("Message.Welcome"));
        
        // 订阅语言切换事件
        Lang.Instance.LanguageChanged += OnLanguageChanged;
    }
    
    private void OnLanguageChanged(object sender, LanguageChangedEventArgs e)
    {
        // 语言切换时的处理逻辑
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(WelcomeMessage));
    }
    
    public void Dispose()
    {
        // 清理资源
        foreach (var localizedString in _localizedStrings)
        {
            localizedString.Dispose();
        }
        _localizedStrings.Clear();
        
        Lang.Instance.LanguageChanged -= OnLanguageChanged;
    }
}
```

## 🔍 故障排除

### 常见问题

#### 1. 语言文件未找到

**问题**: 出现 "语言文件不存在" 错误

**解决方案**:
```csharp
// 检查语言文件路径
string filePath = Path.Combine(Lang.Instance.LanguageDirectory, "Chinese.json");
Console.WriteLine($"语言文件路径: {filePath}");
Console.WriteLine($"文件是否存在: {File.Exists(filePath)}");

// 设置正确的语言目录
Lang.SetLanguageDirectory("正确的语言目录路径");
```

#### 2. XAML绑定不更新

**问题**: 语言切换后XAML中的文本没有更新

**解决方案**:
```xml
<!-- 确保使用正确的绑定方式 -->
<TextBlock Text="{lang:Lang App.Title}"/>

<!-- 或者使用转换器 -->
<TextBlock Text="{Binding 'App.Title', Converter={x:Static lang:LocalizedBindingExtensions.Converter}}"/>
```

#### 3. JSON格式错误

**问题**: 语言文件加载失败

**解决方案**:
```csharp
// 验证JSON格式
var result = LanguageHelper.ValidateLanguageFile("Chinese");
if (!result.IsValid)
{
    Console.WriteLine($"JSON格式错误: {result.ErrorMessage}");
}
```

#### 4. 键名不存在

**问题**: 显示 `[键名]` 格式

**解决方案**:
```csharp
// 检查键名是否正确
string text = Lang.Get("App.Title");
if (text.StartsWith("[") && text.EndsWith("]"))
{
    Console.WriteLine($"键名不存在: {text}");
}

// 使用默认值
string text = Lang.Get("App.Title", "默认标题");
```

### 调试技巧

#### 1. 启用调试输出

```csharp
// 在代码中添加调试信息
System.Diagnostics.Debug.WriteLine($"当前语言: {Lang.Instance.CurrentLanguage}");
System.Diagnostics.Debug.WriteLine($"语言目录: {Lang.Instance.LanguageDirectory}");
System.Diagnostics.Debug.WriteLine($"可用语言: {string.Join(", ", Lang.GetAvailableLanguages())}");
```

#### 2. 检查语言文件内容

```csharp
public static void DebugLanguageFile(string language)
{
    string filePath = Path.Combine(Lang.Instance.LanguageDirectory, $"{language}.json");
    if (File.Exists(filePath))
    {
        string content = File.ReadAllText(filePath);
        Console.WriteLine($"语言文件内容 ({language}):");
        Console.WriteLine(content);
    }
}
```

#### 3. 验证键值映射

```csharp
public static void DebugKeyMapping(string key)
{
    string value = Lang.Get(key);
    Console.WriteLine($"键: {key} -> 值: {value}");
}
```

## 📝 更新日志

### v1.0.0 (2024-01-01)

#### 新增功能
- ✨ 初始版本发布
- ✨ 支持基本的语言切换功能
- ✨ 支持MVVM绑定
- ✨ 支持XAML绑定
- ✨ 提供完整的API和示例
- ✨ 支持异步语言切换
- ✨ 内置语言文件验证和比较功能
- ✨ 支持扩展方法语法 `.L()`
- ✨ 提供LocalizedString类用于MVVM
- ✨ 支持动态语言切换事件监听

#### 技术特性
- 🔧 使用JSON格式存储语言文件
- 🔧 支持嵌套键访问（点分隔符）
- 🔧 自动UI更新机制
- 🔧 错误处理和调试友好
- 🔧 线程安全的单例模式
- 🔧 完整的资源管理支持

## 📄 许可证

本项目采用 MIT 许可证。

**软件协议**: 本软件遵循 MIT 开源协议，您可以在遵守协议的情况下，自由使用、修改和分发本软件。

## 🤝 贡献

欢迎提交Issue和Pull Request来改进这个项目。

### 贡献指南

1. Fork 本项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开拉取请求

### 报告问题

如果您发现了错误或有功能建议，请通过以下方式报告：

1. 在GitHub上创建问题
2. 提供详细的错误描述和重现步骤
3. 包含相关的代码示例
4. 提供环境信息（.NET版本、操作系统等）

## 📞 支持与联系

如果您需要帮助或有任何问题，请通过以下方式联系：

### 🔗 枭龙网络
- 🌐 官方网站: https://0554h.com

### 👨‍💻 联系作者
- 💬 微信: Lucas__sen (李枭龙)
- 📧 邮箱: 332555220@qq.com

### 💻 软件源码
- 📂 GitHub: https://github.com/XLucasen/XL_Language
- 💬 GitHub Issues: [创建Issue](https://github.com/XLucasen/XL_Language/issues)

---


**XL_Language** - 让WPF多语言支持变得简单而强大！ 🚀

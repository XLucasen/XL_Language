using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Data;
using Newtonsoft.Json;

namespace XL_Language
{
    /// <summary>
    /// 多语言管理类，支持动态切换语言和MVVM绑定，同时支持XAML标记扩展
    /// </summary>
    public class Lang : MarkupExtension, INotifyPropertyChanged
    {
        private static Lang _instance;
        private static readonly object _lock = new object();
        
        private Dictionary<string, object> _currentLanguageData;
        private string _currentLanguage = "Chinese";
        private string _languageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Language");
        
        // MarkupExtension 属性
        /// <summary>
        /// 本地化键名（用于XAML标记扩展）
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// 默认值（用于XAML标记扩展）
        /// </summary>
        public string DefaultValue { get; set; }
        
        /// <summary>
        /// 单例实例
        /// </summary>
        public static Lang Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new Lang();
                    }
                }
                return _instance;
            }
        }
        
        /// <summary>
        /// 当前语言
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguage;
            private set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// 语言文件目录
        /// </summary>
        public string LanguageDirectory
        {
            get => _languageDirectory;
            set => _languageDirectory = value;
        }
        
        /// <summary>
        /// 语言切换事件
        /// </summary>
        public event EventHandler<LanguageChangedEventArgs> LanguageChanged;
        
        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        private Lang()
        {
            _currentLanguageData = new Dictionary<string, object>();
            LoadLanguage(_currentLanguage);
        }
        
        /// <summary>
        /// 构造函数（用于XAML标记扩展）
        /// </summary>
        public Lang(string key) : this()
        {
            Key = key;
        }
        
        /// <summary>
        /// MarkupExtension 的 ProvideValue 方法
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>本地化文本</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return DefaultValue ?? string.Empty;
            }

            // 创建一个绑定，当语言切换时能够通知UI更新
            var binding = new Binding("CurrentLanguage")
            {
                Source = Instance,
                Converter = new LanguageConverter(),
                ConverterParameter = new LanguageBindingInfo { Key = Key, DefaultValue = DefaultValue },
                Mode = BindingMode.OneWay
            };

            return binding.ProvideValue(serviceProvider);
        }
        
        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="language">语言名称（如：Chinese、English）</param>
        public static void Set(string language)
        {
            Instance.SetLanguage(language);
        }
        
        /// <summary>
        /// 获取本地化文本
        /// </summary>
        /// <param name="key">键名，支持点分隔符（如：App.Name）</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>本地化文本</returns>
        public static string Get(string key, string defaultValue = null)
        {
            return Instance.GetText(key, defaultValue);
        }
        
        /// <summary>
        /// 异步设置语言
        /// </summary>
        /// <param name="language">语言名称</param>
        /// <returns>任务</returns>
        public static async Task SetAsync(string language)
        {
            await Instance.SetLanguageAsync(language);
        }
        
        /// <summary>
        /// 设置语言目录
        /// </summary>
        /// <param name="directory">语言文件目录</param>
        public static void SetLanguageDirectory(string directory)
        {
            Instance.LanguageDirectory = directory;
        }
        
        /// <summary>
        /// 获取所有可用的语言
        /// </summary>
        /// <returns>可用语言列表</returns>
        public static List<string> GetAvailableLanguages()
        {
            return Instance.GetLanguages();
        }
        
        private void SetLanguage(string language)
        {
            if (LoadLanguage(language))
            {
                CurrentLanguage = language;
                OnLanguageChanged(new LanguageChangedEventArgs(language));
            }
        }
        
        private async Task SetLanguageAsync(string language)
        {
            if (await LoadLanguageAsync(language))
            {
                CurrentLanguage = language;
                OnLanguageChanged(new LanguageChangedEventArgs(language));
            }
        }
        
        private bool LoadLanguage(string language)
        {
            try
            {
                string filePath = Path.Combine(_languageDirectory, $"{language}.json");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"语言文件不存在: {filePath}");
                }
                
                string jsonContent = File.ReadAllText(filePath);
                _currentLanguageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                
                // 调试信息
                System.Diagnostics.Debug.WriteLine($"加载语言文件成功: {filePath}");
                System.Diagnostics.Debug.WriteLine($"加载的键数量: {_currentLanguageData?.Count ?? 0}");
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载语言文件失败: {ex.Message}");
                Console.WriteLine($"❌ 加载语言文件失败: {ex.Message}");
                return false;
            }
        }
        
        private async Task<bool> LoadLanguageAsync(string language)
        {
            try
            {
                string filePath = Path.Combine(_languageDirectory, $"{language}.json");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"语言文件不存在: {filePath}");
                }
                
                string jsonContent = await File.ReadAllTextAsync(filePath);
                _currentLanguageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"异步加载语言文件失败: {ex.Message}");
                return false;
            }
        }
        
        private string GetText(string key, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue ?? key;
            
            try
            {
                string[] keyParts = key.Split('.');
                object current = _currentLanguageData;
                
                // 调试信息（生产环境可注释掉）
                // Console.WriteLine($"查找键: {key}");
                // if (_currentLanguageData != null)
                // {
                //     Console.WriteLine($"当前语言数据键: {string.Join(", ", _currentLanguageData.Keys)}");
                // }
                
                foreach (string part in keyParts)
                {
                    if (current is Dictionary<string, object> dict)
                    {
                        if (dict.TryGetValue(part, out current))
                        {
                            // Console.WriteLine($"找到部分键: {part}, 值类型: {current?.GetType()?.Name}");
                            continue;
                        }
                    }
                    else if (current is Newtonsoft.Json.Linq.JObject jObj)
                    {
                        if (jObj.TryGetValue(part, out var jToken))
                        {
                            current = jToken;
                            // Console.WriteLine($"找到部分键: {part}, 值类型: {current?.GetType()?.Name}");
                            continue;
                        }
                    }
                    // Console.WriteLine($"未找到键部分: {part}");
                    return defaultValue ?? $"[{key}]";
                }
                
                string result = current?.ToString() ?? defaultValue ?? $"[{key}]";
                // Console.WriteLine($"最终结果: {result}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取本地化文本失败: {ex.Message}");
                return defaultValue ?? $"[{key}]";
            }
        }
        
        private List<string> GetLanguages()
        {
            List<string> languages = new List<string>();
            
            try
            {
                if (Directory.Exists(_languageDirectory))
                {
                    string[] files = Directory.GetFiles(_languageDirectory, "*.json");
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        languages.Add(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取可用语言失败: {ex.Message}");
            }
            
            return languages;
        }
        
        protected virtual void OnLanguageChanged(LanguageChangedEventArgs e)
        {
            LanguageChanged?.Invoke(this, e);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            System.Diagnostics.Debug.WriteLine($"PropertyChanged 触发: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>
    /// 语言切换事件参数
    /// </summary>
    public class LanguageChangedEventArgs : EventArgs
    {
        public string Language { get; }
        
        public LanguageChangedEventArgs(string language)
        {
            Language = language;
        }
    }

    /// <summary>
    /// 语言绑定信息
    /// </summary>
    public class LanguageBindingInfo
    {
        public string Key { get; set; }
        public string DefaultValue { get; set; }
    }

    /// <summary>
    /// 语言转换器，用于XAML绑定
    /// </summary>
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"LanguageConverter.Convert 被调用, value: {value}, parameter: {parameter}");
            
            if (parameter is LanguageBindingInfo bindingInfo)
            {
                string result = Lang.Get(bindingInfo.Key, bindingInfo.DefaultValue);
                System.Diagnostics.Debug.WriteLine($"转换结果: {result}");
                return result;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

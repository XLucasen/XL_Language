using System;
using System.Windows.Markup;
using System.Windows.Data;

namespace XL_Language
{
    /// <summary>
    /// XAML标记扩展，用于在XAML中直接使用多语言功能
    /// </summary>
    public class LangExtension : MarkupExtension
    {
        /// <summary>
        /// 本地化键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LangExtension()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">本地化键名</param>
        public LangExtension(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 提供值
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
                Source = Lang.Instance,
                Converter = new LanguageConverter(),
                ConverterParameter = new LanguageBindingInfo { Key = Key, DefaultValue = DefaultValue },
                Mode = BindingMode.OneWay
            };

            return binding.ProvideValue(serviceProvider);
        }
    }

}

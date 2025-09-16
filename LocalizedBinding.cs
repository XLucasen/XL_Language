using System;
using System.Globalization;
using System.Windows.Data;

namespace XL_Language
{
    /// <summary>
    /// 本地化绑定转换器，用于XAML中的多语言绑定
    /// </summary>
    public class LocalizedBinding : IValueConverter
    {
        /// <summary>
        /// 将本地化键名转换为本地化文本
        /// </summary>
        /// <param name="value">本地化键名</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化信息</param>
        /// <returns>本地化文本</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key)
            {
                return Lang.Get(key);
            }
            return value?.ToString() ?? string.Empty;
        }
        
        /// <summary>
        /// 反向转换（不支持）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("LocalizedBinding不支持反向转换");
        }
    }
    
    /// <summary>
    /// 本地化绑定扩展，提供静态资源访问
    /// </summary>
    public static class LocalizedBindingExtensions
    {
        private static readonly LocalizedBinding _converter = new LocalizedBinding();
        
        /// <summary>
        /// 获取本地化绑定转换器实例
        /// </summary>
        public static LocalizedBinding Converter => _converter;
    }
}

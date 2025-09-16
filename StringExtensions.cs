using System;

namespace XL_Language
{
    /// <summary>
    /// 字符串扩展方法，提供简洁的多语言文本获取语法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 获取本地化文本的扩展方法
        /// </summary>
        /// <param name="key">本地化键名</param>
        /// <returns>本地化文本</returns>
        public static string L(this string key)
        {
            return Lang.Get(key);
        }
        
        /// <summary>
        /// 获取本地化文本的扩展方法（带默认值）
        /// </summary>
        /// <param name="key">本地化键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>本地化文本</returns>
        public static string L(this string key, string defaultValue)
        {
            return Lang.Get(key, defaultValue);
        }
    }
}

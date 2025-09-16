using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XL_Language
{
    /// <summary>
    /// 语言切换通知器，用于自动更新UI元素
    /// </summary>
    public class LanguageNotifier : INotifyPropertyChanged
    {
        private static LanguageNotifier _instance;
        private static readonly object _lock = new object();
        
        /// <summary>
        /// 单例实例
        /// </summary>
        public static LanguageNotifier Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LanguageNotifier();
                        }
                    }
                }
                return _instance;
            }
        }
        
        /// <summary>
        /// 当前语言
        /// </summary>
        public string CurrentLanguage => Lang.Instance.CurrentLanguage;
        
        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        private LanguageNotifier()
        {
            // 订阅语言切换事件
            Lang.Instance.LanguageChanged += OnLanguageChanged;
        }
        
        private void OnLanguageChanged(object sender, LanguageChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLanguage));
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// 获取本地化文本
        /// </summary>
        /// <param name="key">本地化键名</param>
        /// <returns>本地化文本</returns>
        public string GetText(string key)
        {
            return Lang.Get(key);
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Lang.Instance.LanguageChanged -= OnLanguageChanged;
        }
    }
}

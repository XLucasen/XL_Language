using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XL_Language
{
    /// <summary>
    /// 本地化字符串类，支持MVVM绑定和动态语言切换
    /// </summary>
    public class LocalizedString : INotifyPropertyChanged
    {
        private string _key;
        private string _value;
        
        /// <summary>
        /// 本地化键名
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    UpdateValue();
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// 本地化文本值
        /// </summary>
        public string Value
        {
            get => _value;
            private set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">本地化键名</param>
        public LocalizedString(string key)
        {
            _key = key;
            UpdateValue();
            
            // 订阅语言切换事件
            Lang.Instance.LanguageChanged += OnLanguageChanged;
        }
        
        /// <summary>
        /// 隐式转换到字符串
        /// </summary>
        /// <param name="localizedString">本地化字符串</param>
        /// <returns>字符串值</returns>
        public static implicit operator string(LocalizedString localizedString)
        {
            return localizedString?.Value ?? string.Empty;
        }
        
        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串值</returns>
        public override string ToString()
        {
            return Value ?? string.Empty;
        }
        
        private void OnLanguageChanged(object sender, LanguageChangedEventArgs e)
        {
            UpdateValue();
        }
        
        private void UpdateValue()
        {
            Value = Lang.Get(_key);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Lang.Instance.LanguageChanged -= OnLanguageChanged;
        }
    }
    
    /// <summary>
    /// 本地化字符串工厂类
    /// </summary>
    public static class LocalizedStringFactory
    {
        /// <summary>
        /// 创建本地化字符串
        /// </summary>
        /// <param name="key">本地化键名</param>
        /// <returns>本地化字符串实例</returns>
        public static LocalizedString Create(string key)
        {
            return new LocalizedString(key);
        }
    }
}

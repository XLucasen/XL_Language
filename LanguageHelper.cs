using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XL_Language
{
    /// <summary>
    /// 语言辅助类，提供便捷的语言管理功能
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// 初始化多语言系统
        /// </summary>
        /// <param name="languageDirectory">语言文件目录</param>
        /// <param name="defaultLanguage">默认语言</param>
        public static void Initialize(string languageDirectory = "Language", string defaultLanguage = "Chinese")
        {
            Lang.SetLanguageDirectory(languageDirectory);
            Lang.Set(defaultLanguage);
        }
        
        /// <summary>
        /// 异步初始化多语言系统
        /// </summary>
        /// <param name="languageDirectory">语言文件目录</param>
        /// <param name="defaultLanguage">默认语言</param>
        /// <returns>任务</returns>
        public static async System.Threading.Tasks.Task InitializeAsync(string languageDirectory = "Language", string defaultLanguage = "Chinese")
        {
            Lang.SetLanguageDirectory(languageDirectory);
            await Lang.SetAsync(defaultLanguage);
        }
        
        /// <summary>
        /// 检查语言文件是否存在
        /// </summary>
        /// <param name="language">语言名称</param>
        /// <returns>是否存在</returns>
        public static bool IsLanguageAvailable(string language)
        {
            string filePath = Path.Combine(Lang.Instance.LanguageDirectory, $"{language}.json");
            return File.Exists(filePath);
        }
        
        /// <summary>
        /// 获取所有可用的语言
        /// </summary>
        /// <returns>可用语言列表</returns>
        public static List<string> GetAvailableLanguages()
        {
            return Lang.GetAvailableLanguages();
        }
        
        /// <summary>
        /// 验证语言文件格式
        /// </summary>
        /// <param name="language">语言名称</param>
        /// <returns>验证结果</returns>
        public static LanguageValidationResult ValidateLanguageFile(string language)
        {
            try
            {
                string filePath = Path.Combine(Lang.Instance.LanguageDirectory, $"{language}.json");
                if (!File.Exists(filePath))
                {
                    return new LanguageValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = $"语言文件不存在: {filePath}"
                    };
                }
                
                string jsonContent = File.ReadAllText(filePath);
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                
                if (data == null || data.Count == 0)
                {
                    return new LanguageValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "语言文件为空或格式无效"
                    };
                }
                
                return new LanguageValidationResult
                {
                    IsValid = true,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new LanguageValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"验证语言文件时发生错误: {ex.Message}"
                };
            }
        }
        
        /// <summary>
        /// 比较两个语言文件的键值差异
        /// </summary>
        /// <param name="language1">语言1</param>
        /// <param name="language2">语言2</param>
        /// <returns>差异报告</returns>
        public static LanguageComparisonResult CompareLanguages(string language1, string language2)
        {
            var result = new LanguageComparisonResult();
            
            try
            {
                var keys1 = GetAllKeys(language1);
                var keys2 = GetAllKeys(language2);
                
                result.MissingInLanguage1 = keys2.Except(keys1).ToList();
                result.MissingInLanguage2 = keys1.Except(keys2).ToList();
                result.CommonKeys = keys1.Intersect(keys2).ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"比较语言文件时发生错误: {ex.Message}";
                return result;
            }
        }
        
        private static List<string> GetAllKeys(string language)
        {
            var keys = new List<string>();
            string filePath = Path.Combine(Lang.Instance.LanguageDirectory, $"{language}.json");
            
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                ExtractKeys(data, "", keys);
            }
            
            return keys;
        }
        
        private static void ExtractKeys(Dictionary<string, object> data, string prefix, List<string> keys)
        {
            foreach (var kvp in data)
            {
                string key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
                
                if (kvp.Value is Dictionary<string, object> nestedDict)
                {
                    ExtractKeys(nestedDict, key, keys);
                }
                else
                {
                    keys.Add(key);
                }
            }
        }
    }
    
    /// <summary>
    /// 语言文件验证结果
    /// </summary>
    public class LanguageValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// 语言比较结果
    /// </summary>
    public class LanguageComparisonResult
    {
        public List<string> MissingInLanguage1 { get; set; } = new List<string>();
        public List<string> MissingInLanguage2 { get; set; } = new List<string>();
        public List<string> CommonKeys { get; set; } = new List<string>();
        public string ErrorMessage { get; set; }
    }
}

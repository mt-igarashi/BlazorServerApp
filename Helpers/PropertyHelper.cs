using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// プロパティヘルパー
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// プロパティの値を取得します
        /// </summary>
        /// <returns>プロパティ情報</returns>
        public static PropertyInfo GetPropertyInfo(object obj, string name)
        {
            return obj.GetType().GetProperty(name);
        }

        /// <summary>
        /// プロパティの値を取得します
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <returns>値</returns>
        public static T GetPropertyValue<T>(PropertyInfo info, object obj)
        {
            return (T)info.GetValue(obj);
        }

        /// <summary>
        /// DisplayNameを取得します
        /// </summary>
        /// <returns>DisplayName</returns>
        public static string GetDisplayName(PropertyInfo info)
        {
            var display = info.GetCustomAttribute<DisplayAttribute>();
            if (display == null)
            {
                return string.Empty;
            }

            return display.Name ?? string.Empty;
        }
    }
}
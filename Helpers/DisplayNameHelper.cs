using System;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// HtmlHelperクラス
    /// </summary>
    public static class DisplayNameHelper
    {
        /// <summary>
        /// Display属性から名称を取得します。
        /// </summary>
        /// <param name="model">モデル</param>
        /// <param name="expression">エクスプレッション</param>
        /// <typeparam name="TModel">モデル型</typeparam>
        /// <typeparam name="TProperty">プロパティ型</typeparam>
        /// <returns>名称</returns>
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression) 
        {
            var type = ExpressionHelper.GetParentProperty(expression);

            var memberExpression = (MemberExpression)expression.Body;
            string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);
            
            var attr = (DisplayAttribute)type.GetProperty(propertyName)?.GetCustomAttributes(typeof(DisplayAttribute), true)?.SingleOrDefault();

            if (attr == null)
            {
                var metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)?.FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true)?.SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : string.Empty;
        }
    }
}
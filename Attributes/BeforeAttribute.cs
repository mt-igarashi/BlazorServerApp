using System;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Helpers;

namespace BlazorApp.Attributes
{
    /// <summary>
    /// 日付検証属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class BeforeAttribute : ValidationAttribute
    {
        /// <summary>
        /// プロパティ名
        /// </summary>
        public string PropertyName { get; set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">プロパティ名</param>
        public BeforeAttribute(string name)
        {
            PropertyName = name;
            ErrorMessage = "{0}は{1}以前の日付を指定してください";
        }

        /// <summary>
        /// 検証を行います。
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="validationContext">バリデーションコンテキスト</param>
        /// <returns>バリデーションリザルト</returns>
        protected override ValidationResult IsValid(object value, 
            ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(PropertyName))
            {
                return ValidationResult.Success;
            }

            var from = value as DateTime?;
            if (from == null)
            {
                return ValidationResult.Success;
            }

            var info = PropertyHelper.GetPropertyInfo(validationContext.ObjectInstance, PropertyName);
            var to = PropertyHelper.GetPropertyValue<DateTime?>(info, validationContext.ObjectInstance);

            if (to is null)
            {
                return ValidationResult.Success;
            }

            if (from <= to)
            {
                return ValidationResult.Success;
            }

            var diplayName = PropertyHelper.GetDisplayName(info);
            var message = string.Format(ErrorMessage, validationContext.DisplayName, diplayName);
            return new ValidationResult(message, new[] { validationContext.MemberName });
        }
    }
}
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// EditContextヘルパー
    /// </summary>
    public static class EditContextHelper
    {
        /// <summary>
        /// 検証実行後のスタイルを返却します。
        /// </summary>
        /// <param name="hasError">エラーフラグ</param>
        /// <returns>スタイル</returns>
        public static string GetFieldCss(bool hasError)
        {
            if (hasError)
            {
                return "invalid";
            }
            else
            {
                return "modified";
            }
        }

        /// <summary>
        /// 対象フィールドのバリデーションメッセージを取得します。
        /// </summary>
        /// <param name="context">EditContext</param>
        /// <param name="name">プロパティ名</param>
        /// <returns></returns>
        public static string GetValidationMessage(this EditContext context, string name)
        {
            var field = context.Field(name);
            var message = context.GetValidationMessages(field).FirstOrDefault();
            return  message ?? string.Empty;
        }

        /// <summary>
        /// 指定したプロパティの変更イベントを通知します。
        /// </summary>
        /// <param name="context">EditContext</param>
        /// <param name="name">プロパティ名</param>
        /// <return>エラーあり/なし</return>
        public static bool NotifyFieldChanged(this EditContext context, string name)
        {
            var field = context.Field(name);
            context.NotifyFieldChanged(field);
            return context.GetValidationMessages(field).Count() > 0;
        }

        /// <summary>
        /// 指定したフィールドに検証エラーがあるかを検証します。
        /// </summary>
        /// <param name="context">EditContext</param>
        /// <param name="name">プロパティ名</param>
        /// <return>エラーあり/なし</return>
        public static bool HasValidationError(this EditContext context, string name) 
        {
            var field = context.Field(name);
            var info = PropertyHelper.GetPropertyInfo(context.Model, name);
            var displayName = PropertyHelper.GetDisplayName(info);

            return context.GetValidationMessages().Any(x => x.Contains(displayName));
        }
    }
}
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// EditContextヘルパー
    /// </summary>
    public static class EditContextHelper
    {
        /// <summary>
        /// 指定したプロパティの変更イベントを通知します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        public static void NotifyFieldChanged(EditContext context, string name)
        {
            var field = context.Field(name);
            context.NotifyFieldChanged(field);
        }
    }
}
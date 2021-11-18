using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// ツールチップヘルパー
    /// </summary>
    public static class TooltipHelper
    {
        /// <summary>
        /// ツールチップを表示します。
        /// </summary>
        /// <param name="service">ツールチップサービス</param>
        /// <param name="context">EditContext</param>
        /// <param name="element">HTMLタグ参照</param>
        /// <param name="message">メッセージ</param>
        public static void ShowErrorTooltip(this TooltipService service, EditContext context, ElementReference element, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var opts = new TooltipOptions(){ 
                    Position = TooltipPosition.Right,
                    Style="background-color: #fff0f5; color:#ff1493",
                    Duration = null };
                    
                service.Open(element, message, opts);
            }
        } 
    }
}
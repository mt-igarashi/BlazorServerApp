using System;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Attributes;
using BlazorApp.Messages;

namespace BlazorApp.Forms
{
    /// <summary>
    /// 映画一覧フォーム
    /// </summary>
    public class MovieIndexForm
    {
        /// <summary>
        /// 映画ジャンル
        /// </summary>
        public string MovieGenre { get; set; }

        /// <summary>
        /// 検索文字列
        /// </summary>
        [Display(Name = "検索文字列")]
        [StringLength(20, ErrorMessage = "{0}は{1}文字以内で入力してください")]
        public string SearchString { get; set; }

        /// <summary>
        /// 開始日
        /// </summary>
        [Display(Name = "開始日")]
        [Before(nameof(To), ErrorMessage = "{0}は{1}以前の日付を指定してください")]
        public DateTime? From { get; set; }

        /// <summary>
        /// 終了日
        /// </summary>
        [Display(Name = "終了日")]
        public DateTime? To { get; set; }

        /// <summary>
        /// メッセージリスト
        /// </summary>
        public MessageList MessageList { get; set; } = new();
    }
}
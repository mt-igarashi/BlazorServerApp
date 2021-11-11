using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Models;

namespace BlazorApp.Forms
{
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
    }
}
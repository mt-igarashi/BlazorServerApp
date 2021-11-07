using System.Collections.Generic;
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
        public string SearchString { get; set; }

        /// <summary>
        /// 映画ジャンルリスト
        /// </summary>
        public List<string> GenreList { get; set; } = new List<string>();

        /// <summary>
        /// 映画リスト
        /// </summary>
        public List<Movie> MovieList { get; set; } = new List<Movie>();
    }
}
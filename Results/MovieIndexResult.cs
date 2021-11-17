using System.Collections.Generic;
using BlazorApp.Models;

namespace BlazorApp.Results
{
    public class MovieIndexResult
    {
                /// <summary>
        /// 映画ジャンルリスト
        /// </summary>
        public List<string> GenreList { get; set; } = new ();

        /// <summary>
        /// 映画リスト
        /// </summary>
        public List<Movie> MovieList { get; set; } = new ();
    }
}
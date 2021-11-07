using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using BlazorApp.Forms;
using BlazorApp.Models;

namespace BlazorApp.Pages {
    public partial class Movie : ComponentBase
    {
        // DBコンテキスト 
        [Inject]
        protected BlazorAppContext Context { get; set; }

        /// <summary>
        /// 画面フォーム
        /// </summary>
        public MovieIndexForm MovieIndexForm { get; set; } = new MovieIndexForm();

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
    　　{
            IQueryable<string> genreQuery = from m in Context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            MovieIndexForm.GenreList = await genreQuery.Distinct().ToListAsync();
            
            await Search();
        }

        /// <summary>
        /// 検索実行イベント
        /// </summary>
        private async Task SearchSubmit()
        {
            await Search();
        }

        private async Task Search() {

            var movies = from m in Context.Movie
                         select m;
            
            if (!String.IsNullOrEmpty(MovieIndexForm.SearchString)) {
                movies = movies.Where(s => s.Title.Contains(MovieIndexForm.SearchString));
            }

            if (!String.IsNullOrEmpty(MovieIndexForm.MovieGenre)) {
                movies = movies.Where(x => x.Genre == MovieIndexForm.MovieGenre);
            }

            MovieIndexForm.MovieList = await movies.ToListAsync();
        }
    }
}
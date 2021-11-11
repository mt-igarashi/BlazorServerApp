
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Forms;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class MovieIndexService : IMovieIndexService
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public MovieIndexService(BlazorAppContext context)
        {
            Context = context;
        }
        
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        protected BlazorAppContext Context { get; set; }

        /// <summary>
        /// ジャンル一覧を取得します。
        /// </summary>
        /// <returns>ジャンルリスト</returns>
        public Task<List<string>> GetGenreList()
        {
            var genreQuery = from m in Context.Movie
                                       orderby m.Genre
                                       select m.Genre;
            return genreQuery.Distinct().ToListAsync();
        } 

        /// <summary>
        /// 映画一覧を取得します。
        /// </summary>
        /// <param name="form">MovieIndexForm</param>
        /// <returns>映画一覧</returns>
        public Task<List<Movie>> GetMovieList(MovieIndexForm form)
        {
            var movies = from m in Context.Movie
                         select m;
            
            if (!String.IsNullOrEmpty(form.SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(form.SearchString));
            }

            if (!String.IsNullOrEmpty(form.MovieGenre))
            {
                movies = movies.Where(x => x.Genre == form.MovieGenre);
            }

            return movies.ToListAsync();
        }
    }
}
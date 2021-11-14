
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Forms;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    /// <summary>
    /// 映画一覧サービス
    /// </summary>
    public class MovieIndexService : IMovieIndexService
    {
        /// <summary>
        /// DBコンテキストファクトリー
        /// </summary>
        public IDbContextFactory<BlazorAppContext> DbFactory { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="factory">DBコンテキストファクトリー</param>
        public MovieIndexService(IDbContextFactory<BlazorAppContext> factory)
        {
            DbFactory = factory;
        }

        /// <summary>
        /// ジャンル一覧を取得します。
        /// </summary>
        /// <returns>ジャンルリスト</returns>
        public async Task<List<string>> GetGenreList()
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var genres = await GetGenreList(context);
                    await tran.CommitAsync();
                    return genres;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return new List<string>();
            }
        }

        /// <summary>
        /// ジャンル一覧を取得します。
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>ジャンルリスト</returns>
        public Task<List<string>> GetGenreList(BlazorAppContext context)
        {
            var genreQuery = from m in context.Movie
                                    orderby m.Genre
                                    select m.Genre;
            return genreQuery.Distinct().ToListAsync();
        }

        /// <summary>
        /// 映画一覧を取得します。
        /// </summary>
        /// <param name="form">MovieIndexForm</param>
        /// <returns>映画一覧</returns>
        public async Task<List<Movie>> GetMovieList(MovieIndexForm form)
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var movies = await GetMovieList(form, context);
                    await tran.CommitAsync();
                    return movies;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return new List<Movie>();
            }
        }

        /// <summary>
        /// 映画一覧を取得します。
        /// </summary>
        /// <param name="form">MovieIndexForm</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>映画一覧</returns>
        public Task<List<Movie>> GetMovieList(MovieIndexForm form, BlazorAppContext context)
        {
            var movies = from m in context.Movie
                         select m;
            
            if (!String.IsNullOrEmpty(form.MovieGenre))
            {
                movies = movies.Where(x => x.Genre == form.MovieGenre);
            }

            if (!String.IsNullOrEmpty(form.SearchString))
            {
                movies = movies.Where(x => x.Title.Contains(form.SearchString));
            }

            if (form.From is not null)
            {
                movies = movies.Where(x => x.ReleaseDate >= form.From.Value);
            }

            if (form.To is not null)
            {
                movies = movies.Where(x => x.ReleaseDate <= form.To.Value);
            }

            return movies.ToListAsync();
        }
    }
}
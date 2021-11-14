using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Forms;
using BlazorApp.Models;

namespace BlazorApp.Services {
    /// <summary>
    /// 映画一覧サービス
    /// </summary>
    public interface IMovieIndexService
    {
        /// <summary>
        /// ジャンル一覧を取得します
        /// </summary>
        Task<List<string>> GetGenreList(); 

        /// <summary>
        /// ジャンル一覧を取得します
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        Task<List<string>> GetGenreList(BlazorAppContext context); 

        /// <summary>
        /// 映画一覧を取得します。
        /// </summary>
        /// <param name="form">映画一覧フォーム</param>
        Task<List<Movie>> GetMovieList(MovieIndexForm form);

        /// <summary>
        /// 映画一覧を取得します。
        /// </summary>
        /// <param name="form">映画一覧フォーム</param>
        /// <param name="context">DBコンテキスト</param>
        Task<List<Movie>> GetMovieList(MovieIndexForm form, BlazorAppContext context);
    }
}
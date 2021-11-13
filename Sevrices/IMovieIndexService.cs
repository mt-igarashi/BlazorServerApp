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
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        Movie FindById(int id);

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        Task<Movie> FindByIdAsync(int id);

        Task<List<string>> GetGenreList(); 
        
        Task<List<Movie>> GetMovieList(MovieIndexForm form);
    }
}
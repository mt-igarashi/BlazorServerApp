using System.Threading.Tasks;
using BlazorApp.Messages;
using BlazorApp.Models;

namespace BlazorApp.Services {
    /// <summary>
    /// 映画登録サービス
    /// </summary>
    public interface IMovieCreateService
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

        /// <summary>
        /// 映画を登録します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージリスト</returns>
        Task<MessageList> Register(Movie movie);
 
        /// <summary>
        /// 映画を更新します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージリスト</returns>
        Task<MessageList> Update(Movie movie);

        /// <summary>
        /// 映画を削除します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>メッセージリスト</returns>
        Task<MessageList> Delete(int id);

        /// <summary>
        /// 検証を実行します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージ一覧</returns>
        Task<MessageList> Validate(Movie movie);
    }
}
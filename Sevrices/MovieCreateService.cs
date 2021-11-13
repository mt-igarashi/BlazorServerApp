using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Messages;
using BlazorApp.Models;

namespace BlazorApp.Services {
    /// <summary>
    /// 映画登録サービス
    /// </summary>
    public class MovieCreateService : IMovieCreateService
    {
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        protected BlazorAppContext Context { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public MovieCreateService(BlazorAppContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        public Movie FindById(int id)
        {
            return Context.Movie.Find(id);
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        public async Task<Movie> FindByIdAsync(int id)
        {
            return await Context.Movie.FindAsync(id);
        }

        /// <summary>
        /// 映画を登録します。
        /// </summary>
        public async Task<MessageList> Register(Movie movie)
        {
            var messageList = new MessageList();
            try
            {
                Context.Add(movie);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // nop
            }

            return messageList;
        } 

        /// <summary>
        /// 映画を更新します。
        /// </summary>
        public async Task<MessageList> Update(Movie movie)
        {
            var messageList = new MessageList();
            try
            {
                Context.Update(movie);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var isFound = await Context.Movie.FindAsync(movie.ID) != null;
                if (isFound)
                {
                    messageList.AddErrorMessage("既に更新されている為、再度処理を行ってください");   
                }
                else
                {
                    messageList.HasDeletionMessage = true;
                    messageList.AddErrorMessage("対象の映画は既に削除されています");   
                }

            }

            return messageList;
        }

        /// <summary>
        /// 映画を削除します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Delete(int id)
        {
            var messageList = new MessageList();
            try
            {
                var movie = await Context.Movie.FindAsync(id);
                if (movie == null)
                {
                    messageList.AddErrorMessage("既に削除されていいます");
                    return messageList;
                }

                Context.Movie.Remove(movie);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                messageList.AddErrorMessage("既に削除されていいます");
            }

            return messageList;
        }

        /// <summary>
        /// 検証を実行します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージリスト</returns>
        public Task<MessageList> Validate(Movie movie)
        {
            var messageList = new MessageList();
            if (movie.Rating == "N" && movie.Price != 10)
            {
                messageList.AddValidationMessage(() => movie.Price, "評価がNの場合は価格を10に設定してください");
            }
            
            return Task.FromResult(messageList);
        }
    }
}
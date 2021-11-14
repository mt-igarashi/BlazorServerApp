
using System;
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
        /// DBコンテキストファクトリー
        /// </summary>
        public IDbContextFactory<BlazorAppContext> DbFactory { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="factory">DBコンテキストファクトリー</param>
        public MovieCreateService(IDbContextFactory<BlazorAppContext> factory)
        {
            DbFactory = factory;
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        public Movie FindById(int id)
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = context.Database.BeginTransaction())
            {
                try
                {
                    var movie =  FindById(id, context);
                    tran.Commit();
                    return movie;
                }
                catch (Exception)
                {
                    tran.Rollback();
                }

                return null;
            }
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>映画エンティティ</returns>
        public Movie FindById(int id, BlazorAppContext context)
        {
            return context.Movie.Find(id);
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画エンティティ</returns>
        public async Task<Movie> FindByIdAsync(int id)
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var movie = await FindByIdAsync(id, context);
                    await tran.CommitAsync();
                    return movie;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return null;
            }
        }

        /// <summary>
        /// 指定したIDに紐付く映画を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>映画エンティティ</returns>
        public async Task<Movie> FindByIdAsync(int id, BlazorAppContext context)
        {
            return await context.Movie.FindAsync(id);
        }

        /// <summary>
        /// 映画を登録します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Register(Movie movie)
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var messages = await Register(movie, context);
                    await tran.CommitAsync();
                    return messages;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return new MessageList();
            }
        }

        /// <summary>
        /// 映画を登録します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Register(Movie movie, BlazorAppContext context)
        {
            var messageList = new MessageList();
            try
            {
                context.Add(movie);
                await context.SaveChangesAsync();
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
        /// <param name="movie">映画エンティティ</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Update(Movie movie)
        {
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var messages = await Update(movie, context);
                    await tran.CommitAsync();
                    return messages;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return new MessageList();
            }
        }

        /// <summary>
        /// 映画を更新します。
        /// </summary>
        /// <param name="movie">映画エンティティ</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Update(Movie movie, BlazorAppContext context)
        {
            var messageList = new MessageList();
            try
            {
                context.Update(movie);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var isFound = await context.Movie.FindAsync(movie.ID) != null;
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
            using (var context = DbFactory.CreateDbContext())
            using (var tran = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var messages = await Delete(id, context);
                    await tran.CommitAsync();
                    return messages;
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

                return new MessageList();
            }
        }

        /// <summary>
        /// 映画を削除します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="context">DBコンテキスト</param>
        /// <returns>メッセージリスト</returns>
        public async Task<MessageList> Delete(int id, BlazorAppContext context)
        {
            var messageList = new MessageList();
            Movie movie = null;
            try
            {
                movie = await context.Movie.FindAsync(id);
                if (movie == null)
                {
                    messageList.AddErrorMessage("既に削除されていいます");
                    return messageList;
                }

                context.Movie.Remove(movie);
                await context.SaveChangesAsync();
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
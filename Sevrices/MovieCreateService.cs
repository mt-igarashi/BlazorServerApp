using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using BlazorApp.Messages;
using BlazorApp.Models;

namespace BlazorApp.Services {
    public class MovieCreateService : IMovieCreateService
    {
        public MessageList MessageList { get; set; }
        public ValidationMessageStore MessageStore { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public MovieCreateService(BlazorAppContext context)
        {
            Context = context;
        }
        
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        protected BlazorAppContext Context { get; set; }

        /// <summary>
        /// 映画を登録します。
        /// </summary>
        public async Task Register(Movie movie)
        {
            try
            {
                Context.Update(movie);
                await Context.SaveChangesAsync();
                MessageList.AddSuccessMessage("登録が完了しました");
            }
            catch (DbUpdateConcurrencyException)
            {
                // nop
            }
        } 

        /// <summary>
        /// 映画を更新します。
        /// </summary>
        public async Task Update(Movie movie)
        {
            try
            {
                Context.Update(movie);
                await Context.SaveChangesAsync();
                MessageList.AddSuccessMessage("更新が完了しました");
            }
            catch (DbUpdateConcurrencyException)
            {
                MessageList.AddErrorMessage("既に更新されている為、再度処理を行ってください");
            }
        }

        /// <summary>
        /// 更新リクエストを検証します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        public void HandleUpdateValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            var context = (EditContext)sender;
            var form = context.Model as Movie;

            MessageList.ClearMessages();
            MessageStore.Clear();

            if (form.Rating == "N" && form.Price != 10)
            {
                MessageStore.Add(() => form.Price, "評価がNの場合は価格を10に設定してください");
            }
        }

        /// <summary>
        /// メッセージを設定します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="store">validationメッセージ</param>
        public void SetMessages(MessageList message, ValidationMessageStore store)
        {
            MessageList = message;
            MessageStore = store;
        }
    }
}
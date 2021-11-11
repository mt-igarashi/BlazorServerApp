using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BlazorApp.Components;
using BlazorApp.Services;

namespace BlazorApp.Pages
{
    /// <summary>
    /// 映画登録画面
    /// </summary>
    public partial class MovieCreate : BlazorAppComponent, IDisposable
    {
        /// <summary>
        /// 映画作成サービス
        /// </summary>
        [Inject]
        protected IMovieCreateService MovieCreateService { get; set; }

        /// <summary>
        /// ナビゲーションマネジャー
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// 画面フォーム
        /// </summary>
        protected Models.Movie MovieCreateForm { get; set; } = new();

        /// <summary>
        /// 編集コンテキスト
        /// </summary>
        protected EditContext FormEditContext { get; set; }

        /// <summary>
        /// 検証メッセージストア
        /// </summary>
        protected ValidationMessageStore MessageStore { get; set; }

        /// <summary>
        /// フォームデータを復元します。
        /// </summary>
        protected async override Task<string> RestoreFormDataAsync()
        {
            return await Task.FromResult(nameof(Movie));
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Task</returns>
        protected override void OnInitialized()
    　　{
            FormEditContext = new EditContext(MovieCreateForm);
            FormEditContext.OnValidationRequested += MovieCreateService.HandleUpdateValidationRequested;
            MessageStore = new ValidationMessageStore(FormEditContext);
            MovieCreateService.SetMessages(MessageList, MessageStore);
        }

        /// <summary>
        /// 登録実行イベント
        /// </summary>
        private async Task HandleUpdateValidSubmit()
        {
            await MovieCreateService.Register(MovieCreateForm);
            NavigationManager.NavigateTo("/movie");
        }

        /// <summary>
        /// 廃棄処理
        /// </summary>
        public void Dispose()
        {
            if (FormEditContext is not null)
            {
                FormEditContext.OnValidationRequested -= MovieCreateService.HandleUpdateValidationRequested;
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BlazorApp.Components;
using BlazorApp.Forms;
using BlazorApp.Messages;
using BlazorApp.Services;

namespace BlazorApp.Pages
{
    /// <summary>
    /// 映画登録画面
    /// </summary>
    public partial class MovieCreate : BlazorAppComponent, IDisposable
    {
        /// <summary>
        /// 映画一覧サービス
        /// </summary>
        [Inject]
        protected IMovieIndexService MovieIndexService { get; set; }

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
        /// ID
        /// </summary>
        [Parameter]
        public int? Id { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Parameter]
        public string Token { get; set; }

        /// <summary>
        /// 存在フラグ
        /// </summary>
        public bool NotFound { get; set; }

        /// <summary>
        /// 読み取りモード
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// フォームデータを復元します。
        /// </summary>
        protected override Task<string> RestoreFormDataAsync()
        {
            return Task.FromResult(nameof(Movie));
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void OnInitialized()
    　　{
            ReadOnly = !string.IsNullOrEmpty(Token);
            if (Id.HasValue)
            {
                var form = MovieIndexService.FindById(Id.Value);
                if (form != null)
                {
                    MovieCreateForm = form;
                }
                else
                {
                    NotFound = true;
                }
            }

            FormEditContext = new EditContext(MovieCreateForm);
            FormEditContext.OnValidationRequested += HandleUpdateValidationRequested;
            MessageStore = new ValidationMessageStore(FormEditContext);
        }

        /// <summary>
        /// 画面描画後処理
        /// </summary>
        /// <param name="firstRender">初回描画を表すフラグ</param>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (NotFound)
                {
                    var messageList = new MessageList();
                    messageList.AddErrorMessage("既に削除されています");
                    
                    var index = await GetFormDataAsync<MovieIndexForm>(nameof(Movie));
                    index.MessageList = messageList;

                    await SetFormDataAsync(nameof(Movie), index);
                    NavigationManager.NavigateTo("/movie");
                }
            }
        }

        /// <summary>
        /// 更新リクエストを検証します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        private async void HandleUpdateValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            MessageList.ClearMessages();
            MessageStore.Clear();

            var messageList = await MovieCreateService.Validate(MovieCreateForm);
            MergeMessages(messageList, MessageStore);
        }

        /// <summary>
        /// 登録実行イベント
        /// </summary>
        private async Task HandleUpdateValidSubmit()
        {
            MessageList messageList;
            if (Id.HasValue)
            {
                messageList = await MovieCreateService.Update(MovieCreateForm);
                if (messageList.ErrorMessageList.Count == 0)
                {
                    messageList.AddSuccessMessage("更新が完了しました");
                }
                
                var form = await GetFormDataAsync<MovieIndexForm>(nameof(Movie));
                form.MessageList = messageList;

                await SetFormDataAsync(nameof(Movie), form);
                NavigationManager.NavigateTo("/movie");
            }
            else
            {
                messageList = await MovieCreateService.Register(MovieCreateForm);
                if (messageList.ErrorMessageList.Count == 0)
                {
                    messageList.AddSuccessMessage("登録が完了しました");
                }
                
                var form = new MovieIndexForm(){
                    MessageList = messageList
                };

                await SetFormDataAsync(nameof(Movie), form);
                NavigationManager.NavigateTo("/movie");
            }
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            if (FormEditContext is not null)
            {
                FormEditContext.OnValidationRequested -= HandleUpdateValidationRequested;
            }
        }
    }
}
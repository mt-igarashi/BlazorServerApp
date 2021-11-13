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
        /// 詳細モード
        /// </summary>
        public bool DetailMode { get; set; }

        /// <summary>
        /// 編集モード
        /// </summary>
        public bool EditMode { get; set; }

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
            DetailMode = !string.IsNullOrEmpty(Token);
            EditMode = Id.HasValue;

            if (EditMode)
            {
                // 編集時はDBから映画を取得
                // EntityFrameworkがモデルをトラッキングをしているので
                // 必ずEntityFrameworkから取得する
                var form = MovieCreateService.FindById(Id.Value);
                if (form != null)
                {
                    MovieCreateForm = form;
                }
                else
                {
                    // ブラウザのセッションにアクセスする為
                    // 処理はOnAfterRenderAsyncで行う
                    // Javascriptを呼び出すメソッドは
                    // OnAfterRender、OnAfterRrenderAsyncで実行する
                    // (上記関数以外は例外が発生する)
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
                // 映画が削除されていた場合は
                // 以前の検索条件で映画一覧に表示する(エラーメッセージあり)
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
            if (EditMode)
            {
                // 更新時
                messageList = await MovieCreateService.Update(MovieCreateForm);
                
                // 正常終了
                if (messageList.ErrorMessageList.Count == 0)
                {
                    messageList.AddSuccessMessage("更新が完了しました");
                    MergeMessages(messageList);
                    return;
                }

                // 楽観ロックエラー
                if (!messageList.HasDeletionMessage)
                {
                    MergeMessages(messageList);
                    return;
                }
                
                // 対象映画が削除されていた場合
                var form = await GetFormDataAsync<MovieIndexForm>(nameof(Movie));
                form.MessageList = messageList;

                await SetFormDataAsync(nameof(Movie), form);
                NavigationManager.NavigateTo("/movie");
            }
            else
            {
                // 登録処理
                messageList = await MovieCreateService.Register(MovieCreateForm);
                
                // 正常終了
                if (messageList.ErrorMessageList.Count == 0)
                {
                    messageList.AddSuccessMessage("登録が完了しました");
                }
                
                // メッセージを検索条件なしで映画一覧に表示する
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
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BlazorApp.Components;
using BlazorApp.Forms;
using BlazorApp.Results;
using BlazorApp.Services;

namespace BlazorApp.Pages
{
    /// <summary>
    /// 映画一覧クラス
    /// </summary>
    public partial class Movie : BlazorAppComponent, IDisposable
    {
        /// <summary>
        /// 映画作成サービス
        /// </summary>
        [Inject]
        public IMovieCreateService MovieCreateService { get; set; }

        /// <summary>
        /// 映画一覧サービス
        /// </summary>
        [Inject]
        public IMovieIndexService MovieIndexService { get; set; }

        /// <summary>
        /// ナビゲーションマネジャー
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// 画面フォーム
        /// </summary>
        public MovieIndexForm MovieIndexForm { get; set; } = new();

        /// <summary>
        /// サービス実行結果
        /// </summary>
        public MovieIndexResult MovieIndexResult { get; set; } = new(); 

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
        /// フォームデータの復元
        /// </summary>
        /// <returns></returns>
        protected override async Task<string> RestoreFormDataAsync()
        {
            var form = await GetFormDataAsync<MovieIndexForm>(nameof(Movie));
            if (form is not null)
            {
                MovieIndexForm.SearchString = form.SearchString;
                MovieIndexForm.MovieGenre = form.MovieGenre;
                MovieIndexForm.MessageList = form.MessageList;
                StateHasChanged();
            }
            return await Task.FromResult(nameof(Movie));
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Task</returns>
        protected override void OnInitialized()
    　　{
            FormEditContext = new EditContext(MovieIndexForm);
            FormEditContext.OnValidationRequested += HandleUpdateValidationRequested;
            FormEditContext.OnFieldChanged += HandleFieldChanged;
            MessageStore = new ValidationMessageStore(FormEditContext);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
    　　{
            await base.OnInitializedAsync();
            MovieIndexResult.GenreList = await MovieIndexService.GetGenreList();
        }

        /// <summary>
        /// パラメータ設定処理
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await DeleteMovieIfExist();
        }

        /// <summary>
        /// 画面描画後処理
        /// </summary>
        /// <param name="firstRender">初回描画を表すフラグ</param>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);
                MergeMessages(MovieIndexForm.MessageList, true);
                StateHasChanged();
            }
            await SetFormDataAsync(nameof(Movie), MovieIndexForm);
        }

        /// <summary>
        /// 更新リクエスト検証イベント。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        private void HandleUpdateValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            MessageList.ClearMessages();
            MessageStore.Clear();
        }

        /// <summary>
        /// フィールド変更イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        private void HandleFieldChanged(object sender, FieldChangedEventArgs args)
        {
            MessageList.ClearMessages();
        }

        /// <summary>
        /// 検索実行イベント
        /// </summary>
        private async Task HandleSearchValidSubmit()
        {
            MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);
        }

        /// <summary>
        /// Idパラメータが存在する場合に削除処理を行います。
        /// </summary>
        private async Task DeleteMovieIfExist()
        {
            if (!Id.HasValue)
            {
                return;
            }

            var messageList = await MovieCreateService.Delete(Id.Value);
            Id = null;
            
            if (messageList.ErrorMessageList.Count == 0)
            {
                messageList.AddSuccessMessage("削除に成功しました");
                MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);
            }
            MergeMessages(messageList);

            NavigationManager.NavigateTo("/movie");
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            if (FormEditContext is not null)
            {
                FormEditContext.OnValidationRequested -= HandleUpdateValidationRequested;
                FormEditContext.OnFieldChanged -= HandleFieldChanged;
            }
        }
    }
}
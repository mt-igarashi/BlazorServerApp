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
        public EditContext FormEditContext { get; set; }

        /// <summary>
        /// 検証メッセージストア
        /// </summary>
        public ValidationMessageStore MessageStore { get; set; }

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
                // EditContextを使う場合はMovieIndexFormをそのまま置き換えると
                // 検証属性のバリデーションが動かなくなる(MovieindexFormの属性を参照)
                // MovieIndexFormを置き換える場合はEditContextを再構成する
                // MovieIndexForm置き換えない場合はプロパティをコピーする
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
            // OnInitialized、OnInitializedAsyncはインスタンス生成時に１回だけ呼び出される
            // タイミングはURLが変わったタイミング(URLバーからの変更はインスタンスが再生成される)
            // EditContextの設定はOnInitializedで行う
            // OnInitializedAsyncAsyncメソッドで行うと例外が発生する
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
            // OnInitializedAsyncではDBアクセスなど重い処理を行う
            // awaitしている間に画面が描画される
            await base.OnInitializedAsync();
            MovieIndexResult.GenreList = await MovieIndexService.GetGenreList();
        }

        /// <summary>
        /// パラメータ設定処理
        /// </summary>
        protected async override Task OnParametersSetAsync()
        {
            // フォームのパラメータが変更されたときはこのイベントは呼び出されない
            // Initializedの後かParameter属性の値が変更された場合に呼び出される(IDプロパティを参照)
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
                // firstRenderで判定することにより初回表示に１回だけ呼び出されるのが保証される
                MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);
                MergeMessages(MovieIndexForm.MessageList, true);

                // 初回描画時は画面に反映されないので変更したことを通知する
                StateHasChanged();
            }

            // 画面描画イベントは複数回発生するのでその対処が必要
            await SetFormDataAsync(nameof(Movie), MovieIndexForm);
        }

        /// <summary>
        /// 更新リクエスト検証イベント。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        private void HandleUpdateValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            // Submit時に呼び出されるメソッド
            // 検証属性による入力項目の検証エラー時は呼び出されない
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
            // 入力項目を変更してフォーカスアウトしたときに発生するイベント
            MessageList.ClearMessages();
        }

        /// <summary>
        /// 検索実行イベント
        /// </summary>
        private async Task HandleSearchValidSubmit()
        {
            // 検証成功時(属性検証、HandleUpdateValidationRequested含む)に呼び出される
            // OnSubmitは検証関係なく呼び出される
            // OnInValidSubmitは検証失敗時に呼び出される
            // Movie.razorのEditFormを参照
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorApp.Components;
using BlazorApp.Forms;
using BlazorApp.Results;
using BlazorApp.Services;

namespace BlazorApp.Pages
{
    public partial class Movie : BlazorAppComponent
    {
        /// <summary>
        /// 映画インデックスサービス
        /// </summary>
        [Inject]
        protected IMovieIndexService MovieIndexService { get; set; }

        /// <summary>
        /// 画面フォーム
        /// </summary>
        public MovieIndexForm MovieIndexForm { get; set; } = new();

        /// <summary>
        /// サービス実行結果
        /// </summary>
        public MovieIndexResult MovieIndexResult { get; set; } = new(); 

        /// <summary>
        /// フォームデータの復元
        /// </summary>
        /// <returns></returns>
        protected override async Task<string> RestoreFormDataAsync()
        {
            var form = await GetFormDataAsync<MovieIndexForm>(nameof(Movie));
            if (form is not null)
            {
                MovieIndexForm = form;
                StateHasChanged();
            }
            return await Task.FromResult(nameof(Movie));
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
    　　{
            MovieIndexResult.GenreList = await MovieIndexService.GetGenreList();
        }

        /// <summary>
        /// 画面描画後処理
        /// </summary>
        /// <param name="firstRender">初回表示を表すフラグ</param>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);
                StateHasChanged();
            }
            await SetFormDataAsync(nameof(Movie), MovieIndexForm);
        }

        /// <summary>
        /// 検索実行イベント
        /// </summary>
        private async Task HandleSearchValidSubmit()
        {
            MovieIndexResult.MovieList = await MovieIndexService.GetMovieList(MovieIndexForm);;
        }
    }
}
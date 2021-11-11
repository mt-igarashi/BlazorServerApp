using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using BlazorApp.Messages;
using BlazorApp.Storage;

namespace BlazorApp.Components 
{
    /// <summary>
    /// コンポーネント基底クラス
    /// </summary>
    public partial class BlazorAppComponent : ComponentBase
    {
        /// <summary>
        /// セッションキー
        /// </summary>
        private static readonly string SESSION_STORAGE_KEY = "SESSION_STORAGE_KEY";

        /// <summary>
        /// セッションストレージ
        /// </summary>
        [Inject]
        protected ProtectedSessionStorage sessionStorage { get; set; }

        /// <summary>
        /// メッセージリスト
        /// </summary>
        public MessageList MessageList { get; set; } = new();

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        /// <summary>
        /// 初期化処理(非同期)
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// パラメータ設定処理
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        /// <summary>
        /// パラメータ設定処理(非同期)
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// 画面描画後処理
        /// </summary>
        /// <param name="firstRender">初回レンダリングを表すフラグ</param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        /// <summary>
        /// 画面描画後処理(非同期)
        /// </summary>
        /// <param name="firstRender">初回レンダリングを表すフラグ</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var exclude = await RestoreFormDataAsync();
                await RemoveAllSessionStorageAsync(exclude);
            }
        }

        /// <summary>
        /// フォームを復元します。
        /// </summary>
        protected virtual string RestoreFormData()
        {
            return string.Empty;
        }

        /// <summary>
        /// フォームを復元します。
        /// </summary>
        protected virtual async Task<string> RestoreFormDataAsync()
        {
            return await Task.FromResult(string.Empty);
        }

        /// <summary>
        /// セッションストレージからデータを取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>ディクショナリ</returns>
        protected Dictionary<string, SessionData> GetSessinStorage()
        {
            return GetSessionStorageAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// セッションストレージからデータを取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>ディクショナリ</returns>
        protected async Task<Dictionary<string, SessionData>> GetSessionStorageAsync()
        {
            var storage = await sessionStorage.GetAsync<string>(SESSION_STORAGE_KEY);
            Dictionary<string, SessionData> dic = null;
            if (storage.Success && !string.IsNullOrEmpty(storage.Value))
            {
                dic = JsonConvert.DeserializeObject<Dictionary<string, SessionData>>(storage.Value);
            }
            else
            {
                dic = new Dictionary<string, SessionData>();
            }

            return dic;
        }

        /// <summary>
        /// セッションにデータを格納します。
        /// </summary>
        /// <param name="key">キー</param>
        protected T GetFormData<T>(string key) where T : class, new()
        {
            return GetFormDataAsync<T>(key).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// セッションにデータを格納します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="delete">削除フラグ</param>
        protected async Task<T> GetFormDataAsync<T>(string key) where T : class, new()
        {
            var dic = await GetSessionStorageAsync();
            var data = dic.GetValueOrDefault(key) ?? new SessionData();
            return data.GetFormData<T>();
        }

        /// <summary>
        /// セッションにデータを格納します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="data">データ</param>
        protected void SetFormData(string key, object data)
        {
            var task = SetFormDataAsync(key, data);
        }

        /// <summary>
        /// セッションにデータを格納します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="data">データ</param>
        protected async Task SetFormDataAsync(string key, object data)
        {
            var dic = await GetSessionStorageAsync();
            var sessionData = dic.GetValueOrDefault(key) ?? new();
            
            sessionData.FormData = JsonConvert.SerializeObject(data);
            dic[key] = sessionData;
            
            await sessionStorage.SetAsync(SESSION_STORAGE_KEY, JsonConvert.SerializeObject(dic));
        }

        /// <summary>
        /// セッションストレージからデータを削除します。
        /// </summary>
        /// <param name="key">キー</param>
        protected void RemoveSessionStorage(string key)
        {
            var task = RemoveSessionStorageAsync(key);
        }

        /// <summary>
        /// セッションストレージからデータを削除します。
        /// </summary>
        /// <param name="key">キー</param>
        protected async Task RemoveSessionStorageAsync(string key)
        {
            var dic = await GetSessionStorageAsync();
            dic.Remove(key);
            await sessionStorage.SetAsync(SESSION_STORAGE_KEY, JsonConvert.SerializeObject(dic));
        }

        /// <summary>
        /// セッションストレージからフォームデータを削除します。
        /// </summary>
        /// <param name="key">キー</param>
        protected void RemoveFormData(string key)
        {
            var task = RemoveFormDataAsync(key);
        }

        /// <summary>
        /// セッションストレージからフォームデータを削除します。
        /// </summary>
        /// <param name="key">キー</param>
        protected async Task RemoveFormDataAsync(string key)
        {
            var dic = await GetSessionStorageAsync();
            var data = dic.GetValueOrDefault(key);

            if (data is not null)
            {
                data.FormData = null;
            }
            await sessionStorage.SetAsync(SESSION_STORAGE_KEY, JsonConvert.SerializeObject(dic));
        }

        /// <summary>
        /// セッションストレージからデータを削除します。
        /// </summary>
        /// <param name="exclude">除外対象</param>
        protected void RemoveAllSessionStorage(string exclude = "")
        {
            var task = RemoveAllSessionStorageAsync(exclude);
        }

        /// <summary>
        /// セッションストレージからデータを削除します。
        /// </summary>
        /// <param name="exclude">除外対象</param>
        protected async Task RemoveAllSessionStorageAsync(string exclude = "")
        {
            var dic = await GetSessionStorageAsync();
            foreach(var key in dic.Keys)
            {
                var excludeList = exclude.Split(",");
                if (!exclude.Contains(key))
                {
                    dic.Remove(key);
                }
            }
            await sessionStorage.SetAsync(SESSION_STORAGE_KEY, JsonConvert.SerializeObject(dic));
        }
    }
}
using Newtonsoft.Json;

namespace BlazorApp.Storage
{
    /// <summary>
    /// セッションデータ
    /// </summary>
    public class SessionData
    {
        /// <summary>
        /// フォームデータ
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// フォームデータを取得します。
        /// </summary>
        /// <typeparam name="T">格納した型</typeparam>
        public T GetFormData<T>() where T : class, new()
        {
            if (!string.IsNullOrWhiteSpace(FormData)) 
            {
                return JsonConvert.DeserializeObject<T>(FormData);
            };

            return new T();
        }
    }
}
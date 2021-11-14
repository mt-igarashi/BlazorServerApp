using BlazorApp.Messages;

namespace BlazorApp.Results
{
    /// <summary>
    /// 実行結果保持クラス
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    public class BlazorAppResult<T> 
    {
        /// <summary>
        /// メッセージリスト
        /// </summary>
        public MessageList MessageList { get; set; }

        /// <summary>
        /// 実行結果
        /// </summary>
        /// <value></value>
        public T Result { get; set; }
    }
}
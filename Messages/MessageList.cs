using System.Collections.Generic;

namespace BlazorApp.Messages
{
    /// <summary>
    /// メッセージ保持クラス
    /// </summary>
    public class MessageList
    {
        /// <summary>
        /// 成功メッセージリスト
        /// </summary>
        public List<string> SuccessMessageList { get; private set; } = new();

        /// <summary>
        /// エラーメッセージリスト
        /// </summary>
        public List<string> ErrorMessageList { get; private set; } = new();

        /// <summary>
        /// 成功メッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddSuccessMessage(string message)
        {
            SuccessMessageList.Add(message);
        }

        /// <summary>
        /// エラーメッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddErrorMessage(string message)
        {
            ErrorMessageList.Add(message);
        }

        /// <summary>
        /// メッセージをクリアします。
        /// </summary>
        public void ClearMessages()
        {
            SuccessMessageList.Clear();
            ErrorMessageList.Clear();
        }
    }
}
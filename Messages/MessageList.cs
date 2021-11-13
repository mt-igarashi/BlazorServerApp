using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        /// 検証メッセージ
        /// </summary>
        public List<MessageStore> ValidationMessageList { get; private set; } = new();

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
        /// 検証メッセージを追加します。
        /// </summary>
        /// <param name="accessor">アクセッサ</param>
        /// <param name="message">メッセージ</param>
        public void AddValidationMessage(Expression<Func<object>> accessor, string message)
        {
            ValidationMessageList.Add(new MessageStore {
                Accessor = accessor,
                Message = message
            });
        }

        /// <summary>
        /// メッセージをクリアします。
        /// </summary>
        public void ClearMessages()
        {
            SuccessMessageList.Clear();
            ErrorMessageList.Clear();
            ValidationMessageList.Clear();
        }
    }
}
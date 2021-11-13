using System;
using System.Linq.Expressions;

namespace BlazorApp.Messages
{
    /// <summary>
    /// 検証メッセージ保持クラス
    /// </summary>
    public class MessageStore
    {
        /// <summary>
        /// アクセッサ
        /// </summary>
        public Expression<Func<object>> Accessor { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; } 
    }
}
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Extensions
{
    /// <summary>
    /// DbContext拡張クラス
    /// </summary>
    public static class DbContextExtension
    {
        /// <summary>
        /// DBコンテキストをリフレッシュします。
        /// </summary>
        /// <param name="context">DbContexr</param>
        public static void RefreshAll(this DbContext context)
        {
            foreach (var entity in context.ChangeTracker.Entries().ToArray())
            {
                entity.Reload();
            }
        }
    }
}
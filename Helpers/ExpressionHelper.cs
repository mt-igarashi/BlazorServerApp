using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorApp.Helpers
{
    /// <summary>
    /// 式木Helperクラス
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// MemberExpressionを取得します。
        /// </summary>
        /// <param name="expression">式木</param>
        /// <returns>MemberExpression</returns>
        public static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }
            else if (expression is LambdaExpression)
            {
                var lambdaExpression = expression as LambdaExpression;
                if (lambdaExpression.Body is MemberExpression)
                {
                    return (MemberExpression)lambdaExpression.Body;
                }
                else if (lambdaExpression.Body is UnaryExpression)
                {
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
                }
            }
            return null;
        }

        /// <summary>
        /// 親クラスのTypeを返却します。
        /// 親クラスが存在しない場合は自クラスのTypeを返却します。
        /// </summary>
        /// <param name="expression">式木</param>
        /// <returns></returns>
        public static Type GetParentPropertyType(Expression expression)
        {
            var list = new List<MemberExpression>();
            var memberExpression = GetMemberExpression(expression);
            do
            {
                list.Add(memberExpression);
                memberExpression = GetMemberExpression(memberExpression.Expression);
            }
            while (memberExpression != null);

            PropertyInfo prop = null;
            if (list.Count == 1)
            {
                prop = list[0].Member as PropertyInfo;
            }
            else
            {
                prop = list[list.Count - 2].Member as PropertyInfo;
            }

            return prop.DeclaringType;
        }
    }
}
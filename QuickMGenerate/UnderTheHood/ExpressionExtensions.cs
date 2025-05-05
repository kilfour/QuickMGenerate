using System.Linq.Expressions;
using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public static class ExpressionExtensions
	{
		public static MemberExpression AsMemberExpression<TTarget, TExpression>(this Expression<Func<TTarget, TExpression>> expression)
		{
			if (expression.Body is UnaryExpression)
				return ((UnaryExpression)expression.Body).Operand as MemberExpression;
			return expression.Body as MemberExpression;
		}

		public static PropertyInfo AsPropertyInfo<TTarget, TExpression>(this Expression<Func<TTarget, TExpression>> expression) =>
			expression.AsMemberExpression().Member as PropertyInfo;
	}
}

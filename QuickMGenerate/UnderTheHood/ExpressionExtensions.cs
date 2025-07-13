using System.Linq.Expressions;
using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public static class ExpressionExtensions
	{
		public static PropertyInfo AsPropertyInfo<TTarget, TProperty>(this Expression<Func<TTarget, TProperty>> expression)
		{
			if (expression.Body is MemberExpression memberExpr)
			{
				if (memberExpr.Member is PropertyInfo property)
					return property;
			}

			if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
			{
				if (unaryMember.Member is PropertyInfo property)
					return property;
			}

			throw new ArgumentException($"Expression '{expression}' does not refer to a property.");
		}
	}
}

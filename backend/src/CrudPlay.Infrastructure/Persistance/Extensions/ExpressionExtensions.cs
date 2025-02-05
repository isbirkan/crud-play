using System.Linq.Expressions;

namespace CrudPlay.Infrastructure.Persistance.Extensions;

public static class ExpressionExtensions
{
    public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is MemberExpression member)
            return member.Member.Name;

        throw new ArgumentException("Invalid property expression");
    }
}

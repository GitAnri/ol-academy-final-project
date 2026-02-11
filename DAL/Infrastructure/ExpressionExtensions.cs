using System.Linq.Expressions;
namespace DAL.Infrastructure
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T));
            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    Expression.Invoke(expr1, param),
                    Expression.Invoke(expr2, param)
                ),
                param
            );
            return combined;
        }
    }
}

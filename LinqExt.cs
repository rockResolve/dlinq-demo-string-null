using System;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApp31ConcatTypes
{
    public static class LinqExt
    {
        public static IQueryable Concat(this IQueryable source1, IQueryable source2)
        {
            if (source1 == null)
                throw new ArgumentNullException("source1");
            if (source2 == null)
                throw new ArgumentNullException("source2");
            return source1.Provider.CreateQuery(
                Expression.Call(
                typeof(Queryable), "Concat",
                new Type[] { source1.ElementType },
                source1.Expression, source2.Expression
                    ));
        }
    }
}

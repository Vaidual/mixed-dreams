using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MixedDreams.Application.Extensions.QueryableExtensions;

namespace MixedDreams.Application.Extensions
{
    public static class QueryableExtensions
    {
        //public static IQueryable<T> ApplySort<T>(IQueryable<T> entities, string orderByQueryString)
        //{
        //    if (!entities.Any())
        //        return entities;
        //    if (string.IsNullOrWhiteSpace(orderByQueryString))
        //    {
        //        return entities;
        //    }
        //    var orderParams = orderByQueryString.Trim().Split(',');
        //    var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    var orderQueryBuilder = new StringBuilder();
        //    foreach (var param in orderParams)
        //    {
        //        if (string.IsNullOrWhiteSpace(param))
        //            continue;
        //        var propertyFromQueryName = param.Split(" ")[0];
        //        var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
        //        if (objectProperty == null)
        //            continue;
        //        var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
        //        orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
        //    }
        //    var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        //    var parameter = Expression.Parameter(typeof(T), "x");
        //    var orderByExpression = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(T), orderQuery);
        //    var orderedEntities = Expression.Call(
        //        typeof(Queryable),
        //        sortingOrder == "descending" ? "OrderByDescending" : "OrderBy",
        //        new[] { typeof(T), orderByExpression.ReturnType },
        //        entities.Expression,
        //        Expression.Quote(orderByExpression)
        //    );

        //    return entities.Provider.CreateQuery<T>(orderedEntities);
        //}

        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string property,
            ListSortDirection sortOrder = ListSortDirection.Ascending)
        {

            return ApplyOrder<T>(source, property, sortOrder == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(
            IQueryable<T> source,
            string property,
            string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.ObjectModel;

namespace Market.Infrastructure.Data
{
    public static class BuilderHasIndexExtensions
    {

        static readonly MethodInfo HasIndexMethod = typeof(BuilderHasIndexExtensions)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && t.Name == nameof(SetIndex));

        public static void HasIndexOnAllEntities<TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, object>> filterExpression)
        {
            foreach (var type in builder.Model.GetEntityTypes()
                .Where(t => t.BaseType == null)
                .Select(t => t.ClrType)
                .Where(t => typeof(TEntityInterface).IsAssignableFrom(t)))
            {
                builder.SetEntityIndex(
                    type,
                    filterExpression);
            }
        }

        static void SetEntityIndex<TEntityInterface>(
            this ModelBuilder builder,
            Type entityType,
            Expression<Func<TEntityInterface, object>> filterExpression)
        {
            HasIndexMethod
                .MakeGenericMethod(entityType, typeof(TEntityInterface))
               .Invoke(null, new object[] { builder, filterExpression });
        }

        static void SetIndex<TEntity, TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, object>> filterExpression)
                where TEntityInterface : class
                where TEntity : class, TEntityInterface
        {
            var concreteExpression = filterExpression
                .Convert<TEntityInterface, TEntity>();
            builder.Entity<TEntity>()
                .HasIndex(concreteExpression);
        }

    }

    public static class HasIndexExpressionExtensions
    {
        // This magic is courtesy of this StackOverflow post.
        // https://stackoverflow.com/questions/38316519/replace-parameter-type-in-lambda-expression
        // I made some tweaks to adapt it to our needs - @haacked
        public static Expression<Func<TTarget, object>> Convert<TSource, TTarget>(
            this Expression<Func<TSource, object>> root)
        {
            var visitor = new ParameterTypeVisitor<TSource, TTarget>();
            return (Expression<Func<TTarget, object>>)visitor.Visit(root);
        }
        class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
        {
            private ReadOnlyCollection<ParameterExpression> _parameters;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameters?.FirstOrDefault(p => p.Name == node.Name)
                       ?? (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                _parameters = VisitAndConvert(node.Parameters, "VisitLambda");
                return Expression.Lambda(Visit(node.Body), _parameters);
            }
        }
    }

}

using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            return new People(expression);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return (IQueryable<TResult>)new People(expression);
        }

        public object Execute(Expression expression)
        {
            return new PersonService().Search(GetSqlQuery(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            return new SqlExpressionVisitor().Translate(expression);
        }
    }
}


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
            return (IQueryable<TResult>)CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            return new PersonService().Search(GetSqlQuery(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);

            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            return new SqlExpressionVisitor().Translate(expression);

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }
    }
}

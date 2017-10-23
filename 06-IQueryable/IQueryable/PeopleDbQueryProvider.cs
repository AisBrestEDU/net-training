
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return (IQueryable<TResult>) new People(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            PersonService service = new PersonService();

            String query = this.GetSqlQuery(expression);

            return (TResult) service.Search(query);

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

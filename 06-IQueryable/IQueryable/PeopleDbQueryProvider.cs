
using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();

            Type elementType = expression.Type;
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(People).MakeGenericType(elementType), new object[] { expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            // TODO: Implement CreateQuery
            //throw new NotImplementedException();

            return (IQueryable<TResult>) new People(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            //throw new NotImplementedException();
            return new PersonService().Search(GetSqlQuery(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            //throw new NotImplementedException();

            return (TResult) Execute(expression);
            // HINT: Use GetSqlQuery to build query and pass the query to PersonService
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            var queryBuilder = new VstrPeople();

            var queryString = queryBuilder.GetQueryString((expression as MethodCallExpression).Arguments[1]); ;

            if ("select * from person" == queryString) throw new InvalidOperationException();

            return queryString;

        }
    }
}

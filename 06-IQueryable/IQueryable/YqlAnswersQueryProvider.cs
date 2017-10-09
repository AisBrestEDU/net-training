using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class YqlAnswersQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery
            throw new NotImplementedException();
        }

        public IQueryable<Question> CreateQuery<Question>(Expression expression)
        {
            // TODO: Implement CreateQuery
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();

            // HINT: Use GetYqlQuery to build query
            // HINT: Use YqlAnswersService to fetch data
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetYqlQuery(Expression expression)
        {
            // TODO: Implement GetYqlQuery
            throw new NotImplementedException();

            // HINT: Create a class derived from ExpressionVisitor
        }
    }
}

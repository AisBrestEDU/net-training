using System.Text;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
			Type elementType = expression.Type;
			return (IQueryable)Activator.CreateInstance(typeof(People).MakeGenericType(elementType), new object[] { this, expression });
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
			return (IQueryable<TResult>)new People(expression);
        }

        public object Execute(Expression expression)
        {
			return this.Execute<object>(expression);
	    }

        public TResult Execute<TResult>(Expression expression)
        {
			var sqlQuery =  GetSqlQuery(expression);
			var dbService = new PersonService();
			return (TResult)dbService.Search(sqlQuery);
		}

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
			var translator = new SqlTranslator();	
			return translator.Translate(expression);

			// HINT: This method is not part of IQueryProvider interface and is used here only for tests.
			// HINT: To transform expression to sql query create a class derived from ExpressionVisitor
			// HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
		}
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
		//Создает объект IQueryable, который позволяет вычислить запрос, представленный заданным деревом выражения.
		public IQueryable CreateQuery(Expression expression)
        {
			// TODO: Implement CreateQuery
	        //throw new NotImplementedException();

			return new People(expression);
		}

		//Создает объект IQueryable<T>, который позволяет вычислить запрос, представленный заданным деревом выражения.
		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
			// TODO: Implement CreateQuery
			//throw new NotImplementedException();

	        return (IQueryable<TResult>)new People(expression);
		}

		//Выполняет запрос, представленный заданным деревом выражения.
		public object Execute(Expression expression)
        {
			// TODO: Implement Execute
			//throw new NotImplementedException();

			var service = new PersonService();
	        return service.Search(GetSqlQuery(expression));
		}

		////Выполняет сторого типизированный запрос, представленный заданным деревом выражения.
		public TResult Execute<TResult>(Expression expression)
        {
			// TODO: Implement Execute
			//throw new NotImplementedException();

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
            // TODO: Implement GetYqlQuery
            //throw new NotImplementedException();

			var visitor = new SqlExpressionVisitor();

	        return visitor.Translate(expression);

			// HINT: This method is not part of IQueryProvider interface and is used here only for tests.
			// HINT: To transform expression to sql query create a class derived from ExpressionVisitor
			// HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
		}
    }
}

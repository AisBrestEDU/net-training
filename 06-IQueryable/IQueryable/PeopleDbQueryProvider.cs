using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    public class PeopleDbQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            // TODO: Implement CreateQuery  //-2
            return new People(expression);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            // TODO: Implement CreateQuery// -1
            return (IQueryable<TResult>) new People(expression);
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute

            return new PersonService().Search(GetSqlQuery(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
            //  var queryString = GetSqlQuery(expression);

            return (TResult) Execute(expression);
        }

        /// <summary>
        ///     Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
         var visitor = new SqlExpressionVisitor();

            return visitor.Translate(expression);


            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }
    }
}
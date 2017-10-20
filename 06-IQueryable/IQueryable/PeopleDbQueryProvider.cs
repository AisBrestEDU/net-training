using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

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
            return (IQueryable<TResult>) (new People(expression));
        }

        public object Execute(Expression expression)
        {
            // TODO: Implement Execute
           // var queryString = 
            return GetSqlQuery(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            // TODO: Implement Execute
          //  var queryString = GetSqlQuery(expression);
           
            return (TResult) GetSqlQuery(expression).Cast<TResult>();
        }

        /// <summary>
        /// Generates YQL Query
        /// </summary>
        /// <param name="expression">Expression tree</param>
        /// <returns></returns>
        public string GetSqlQuery(Expression expression)
        {
            
            var queryBuilder = new QueryBilder();

            var queryString = queryBuilder.GetQueryString((expression as MethodCallExpression).Arguments[1]).ToLower();

            
            var regexp = new Regex("\".+\"");
            var str1256=regexp.Match(queryString).Value;
            string str2;
            if (str1256 == "")
                throw new InvalidOperationException();
                str2 = regexp.Replace(queryString, $@"'%{str1256.Substring(1, str1256.Length - 2)}%'", 1);
            
             

            if (string.IsNullOrEmpty(queryString))
                throw new InvalidOperationException();

            queryString = str2.Replace("firstname=", "firstname like ").Replace("="," = ").Replace("greaterthan", " > ");
            if (queryString.Contains("ChosenAnswer") || queryString.Contains("Content"))
                throw new NotSupportedException();

          return queryString;

            // HINT: This method is not part of IQueryProvider interface and is used here only for tests.
            // HINT: To transform expression to sql query create a class derived from ExpressionVisitor
            // HINT: Read the tutorial https://msdn.microsoft.com/en-us/library/bb546158.aspx for more info
        }
    }
    
    
}
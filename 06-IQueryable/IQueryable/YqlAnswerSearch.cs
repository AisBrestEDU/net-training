using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    /// <summary>
    /// Implements Linq to YQL for the answers.search table
    /// http://developer.yahoo.com/yql/
    /// 
    /// YQL for the answers.search table sample
    /// http://developer.yahoo.com/yql/console/?q=select%20*%20from%20answers.search%20where%20query%3D%22cars%22%20and%20type%3D%22undecided%22%20#h=select%20*%20from%20answers.search%20where%20query%3D%22cars%22%20and%20category_id%3D2115500137%20and%20type%3D%22resolved%22
    /// 
    /// simple YQL query looks like SQL query:
    ///     select * from answers.search where query="Belarus" and type="resolved"
    /// </summary>
    public class YqlAnswerSearch : IQueryable<Question>
    {
        public YqlAnswerSearch()
        {
            Expression = Expression.Constant(this);
        }

        public YqlAnswerSearch(Expression expression)
        {
            Expression = expression;
        }

        public IEnumerator<Question> GetEnumerator()
        {
            // TODO: Implement GetEnumerator
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // TODO: Implement GetEnumerator
            throw new NotImplementedException();
        }

        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get
            {
                // TODO: Implement ElementType get
                throw new NotImplementedException();
            }
        }

        public IQueryProvider Provider { get { return new YqlAnswersQueryProvider(); } }

		/// <summary>
		/// Builds YQL query by an expression
		/// </summary>
		/// <returns>YQL query</returns>
        public override string ToString()
        {
            return ((YqlAnswersQueryProvider) Provider).GetYqlQuery(Expression);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IQueryableTask
{
    /// <summary>
    /// Implements Linq to Sql for the people database. 
    /// Database is represented by sqllite (https://www.sqlite.org/) database and you can find it in the file people.db in the IQueryableTests.Test project.
    /// 
    /// To check the database structure you can use http://sqlitebrowser.org/ utility.
    /// To see the examples of queries see the project with tests
    /// 
    /// The database contains only one table Person with following structure
    /// 
    /// CREATE TABLE "person" (
    ///  `Id` INTEGER PRIMARY KEY AUTOINCREMENT,
    ///  `FirstName` TEXT, 
    ///  `LastName` TEXT,
    ///  `Sex` INTEGER,
    ///  `Age` INTEGER 
    /// ) 
    /// </summary>
    public class People : IQueryable<Person>
    {
        public People()
        {
            Expression = Expression.Constant(this);
        }
        
        public People(Expression expression)
        {
            Expression = expression;
        }

        public IEnumerator<Person> GetEnumerator()
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
                // TODO: Implement GetEnumerator
                throw new NotImplementedException();
            }
        }

        public IQueryProvider Provider => new PeopleDbQueryProvider();

        /// <summary>
		/// Builds SQL query by an expression. Needed for tests
		/// </summary>
		/// <returns>SQL query</returns>
        public override string ToString()
        {
            return ((PeopleDbQueryProvider) Provider).GetSqlQuery(Expression);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using IQueryableTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQueryable.Tests
{
    [TestClass]
    public class QueryableTests
    {
        #region Test Helpers

        private static void QueryTest(IQueryable<Person> query, string expected, string assertMessage)
        {
            QueryTest(query, new[] {expected}, assertMessage);
        }

        private static void QueryTest(IQueryable<Person> query, IEnumerable<string> expected, string assertMessage)
        {
            var actual = query.ToString();
            Assert.IsTrue(expected.Contains(actual, StringComparer.InvariantCultureIgnoreCase), assertMessage);
        }

        #endregion

        [TestMethod]
        public void Where_Should_Filter_Results()
        {
            var people = from p in new People()
            			 where p.FirstName.Contains("Alex")
            			 select p;

            var males = from p in new People()
            			   where p.FirstName.Contains("Alex") && p.Sex == Sex.Male
            			   select p;

            var peopleOldenThan24 = from a in new People()
            					  where a.FirstName.Contains("Alex") && a.Age > 24
            					  select a;

            QueryTest(people, "select * from person where firstname like \'%Alex%\'", "Where should filter by FirstName");

            QueryTest(males, new[] { "select * from person where firstname like \'%Alex%\' and sex = 0",
                                     "select * from person where sex = 0 and firstname like \'%Alex%\'" },
            		  "Where should filter by firstname and sex");

            QueryTest(peopleOldenThan24, new[] { "select * from person where firstname like \'%Alex%\' and age > 24",
                                                 "select * from person where age > 24 and firstname like \'%Alex%\'" },
                      "Where should filter by firstname and age");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Simple_Select_Should_Raise_Exception_When_Filters_Not_Specified()
        {
            var people = from p in new People()
                         select p;

            people.GetEnumerator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Where_Should_Raise_Exception_If_Querying_By_FullName()
        {
            var people = from p in new People()
                          where p.FullName == "Poopy Butthole"
                          select p;

            people.GetEnumerator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Where_Should_Raise_Exception_If_Querying_By_LastName()
        {
            var people = from p in new People()
                where p.LastName == "Sanchez"
                select p;

            people.GetEnumerator();
        }

        [TestMethod]
        public void Where_Should_Fetch_Proper_Data_From_Database()
        {
            var people = new People().Where(a => a.FirstName.Contains("Alex") && a.Age > 24).ToList();

            foreach (var person in people)
            {
                Assert.IsTrue(person.FirstName.IndexOf("Alex", StringComparison.InvariantCultureIgnoreCase) >= 0, 
                            "Where should fetch proper data from database");

                Assert.IsTrue(person.Age > 24,
                    "Where should fetch proper data from database");
            }
        }
    }
}

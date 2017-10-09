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

        private static void QueryTest(IQueryable<Question> query, string expected, string assertMessage)
        {
            QueryTest(query, new[] {expected}, assertMessage);
        }

        private static void QueryTest(IQueryable<Question> query, IEnumerable<string> expected, string assertMessage)
        {
            var actual = query.ToString();
            Assert.IsTrue(expected.Contains(actual, StringComparer.InvariantCultureIgnoreCase), assertMessage);
        }

        #endregion

        [TestMethod]
        public void Where_Should_Filter_Results()
        {
			var answers = from a in new YqlAnswerSearch()
						  where a.Subject.Contains("Belarus")
						  select a;

			var answered = from a in new YqlAnswerSearch()
						   where a.Subject.Contains("Belarus") && a.Type == QuestionType.Resolved
						   select a;

			var answersInEurope = from a in new YqlAnswerSearch()
								  where a.Subject.Contains("Belarus") && a.Category == "Other - Europe"
								  select a;

			QueryTest(answers, "select * from answers.search where query=\"Belarus\"", "Where should query by Belarus");

			QueryTest(answered, new[] { "select * from answers.search where query=\"Belarus\" and type=\"resolved\"",
			                            "select * from answers.search where type=\"resolved\" and query=\"Belarus\"" },
					  "Where should query by Belarus and by type = resolved");

			
			QueryTest(answersInEurope, new[] { "select * from answers.search where query=\"Belarus\" and category_name=\"Other - Europe\"",
			                                   "select * from answers.search where category_name=\"Other - Europe\" and query=\"Belarus\"" },
					  "Where should query by Belarus and by category = Other - Europe");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Simple_Select_Should_Raise_Exception_As_SubjectContains_Not_Specified()
        {
            var answers = from a in new YqlAnswerSearch()
                          select a;

            answers.ToString();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Where_Should_Raise_Exception_If_Category_Specified_But_SubjectContains_Not_Specified()
        {
            var answers = from a in new YqlAnswerSearch()
                          where a.Category == "Other - Europe"
                          select a;

            answers.ToString();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Where_Should_Raise_Exception_If_Querying_By_ChosenAnswer()
        {
            var answers = from a in new YqlAnswerSearch()
                          where a.ChosenAnswer == "42"
                          select a;

            answers.ToString();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Where_Should_Raise_Exception_If_Querying_By_Content()
        {
            var answers = from a in new YqlAnswerSearch()
                          where a.Content == "something"
                          select a;

            answers.ToString();
        }

        [TestMethod]
        public void Where_Should_Fetch_Proper_Data_From_Yql_Service()
        {
            var answers = from a in new YqlAnswerSearch()
                          where a.Subject.Contains("Belarus")
                          select a;

            foreach (var answer in answers)
            {
                Assert.IsTrue(answer.Subject.IndexOf("Belarus", StringComparison.InvariantCultureIgnoreCase) >= 0, 
                            "Where should fetch proper data from Yql service");
            }
        }
    }
}

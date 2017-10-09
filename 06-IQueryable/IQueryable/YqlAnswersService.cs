using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace IQueryableTask
{
    /// <summary>
    /// YQL Data Access Service
    /// </summary>
    public class YqlAnswersService
    {
        private const string YqlUrl = "http://query.yahooapis.com/v1/public/yql?q={0}";

        public IEnumerable<Question> Search(string yqlQuery)
        {
            string url = string.Format(YqlUrl, HttpUtility.UrlEncode(yqlQuery));
            XDocument doc = XDocument.Load(url);

            XNamespace ns = "urn:yahoo:answers";
            return from q in doc.Descendants(ns + "Question")
                   select new Question
                              {
                                  Subject = (string) q.Element(ns + "Subject"),
                                  Content = (string) q.Element(ns + "Content"),
                                  Category = (string) q.Element(ns + "Category"),
                                  Type = ConvertQuestionType((string) q.Attribute("type")),
                                  ChosenAnswer = (string) q.Element(ns + "ChosenAnswer")
                              };
        }

        private static QuestionType ConvertQuestionType(string source)
        {
            switch (source)
            {
                case "Answered":
                    return QuestionType.Resolved;
                case "Open":
                    return QuestionType.Open;
                case "Voting":
                    return QuestionType.Undecided;
                default:
                    throw new ArgumentOutOfRangeException("source");
                    
            }
        }
    }
}

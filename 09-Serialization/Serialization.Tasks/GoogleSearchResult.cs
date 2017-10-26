
using System;
using System.Web.Script.Serialization;

namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



 
    public class GoogleSearchResult
    {
        public string Title { get; set; }
        public string TotalResults { get; set; }
        public string SearchTerms { get; set; }
        public string Count { get; set; }
        public string StartIndex { get; set; }
        public string InputEncoding { get; set; }
        public string OutputEncoding { get; set; }
        public string Cx { get; set; }
        public string HtmlTitle { get; set; }
        public string DisplayLink { get; set; }
        public string Snippet { get; set; }
        public string HtmlSnippet { get; set; }
}

//    public class Programm
//    {
//        // create a new instance of JavaScriptSerializer
//        JavaScriptSerializer s1 = new JavaScriptSerializer();
//
//        // deserialise the received response 
//        GoogleSearchResult results = s1.Deserialize<GoogleSearchResult>(json);
//
//        Console.WriteLine(s1.Serialize(results));
//    }


}

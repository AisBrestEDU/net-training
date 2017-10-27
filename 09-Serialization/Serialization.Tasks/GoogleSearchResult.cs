
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt


    [DataContract]
    public class GoogleSearchResult
    {
        //    [DataContract]
        //    public class Url
        //    {
        //        [DataMember]
        //        public string type { get; set; }
        //        [DataMember]
        //        public string template { get; set; }
        //    }

        //    [DataContract]
        //    public class NextPage
        //    {
        //        public string title { get; set; }
        //        public int totalResults { get; set; }
        //        public string searchTerms { get; set; }
        //        public int count { get; set; }
        //        public int startIndex { get; set; }
        //        public string inputEncoding { get; set; }
        //        public string outputEncoding { get; set; }
        //        public string cx { get; set; }
        //    }

        //    [DataContract]
        //    public class Request
        //    {
        //        public string title { get; set; }
        //        public int totalResults { get; set; }
        //        public string searchTerms { get; set; }
        //        public int count { get; set; }
        //        public int startIndex { get; set; }
        //        public string inputEncoding { get; set; }
        //        public string outputEncoding { get; set; }
        //        public string cx { get; set; }
        //    }

        //    [DataContract]
        //    public class Queries
        //    {
        //        public List<NextPage> nextPage { get; set; }
        //        public List<Request> request { get; set; }
        //    }

        //    [DataContract]
        //    public class Context
        //    {
        //        public string title { get; set; }
        //    }

        //    [DataContract]
        //    public class Context
        //    {
        //        public string title { get; set; }
        //    }

        //    [DataContract]
        //    public class RTO
        //    {
        //        public string format { get; set; }
        //        public string group_impression_tag { get; set; }
        //        public string __invalid_name__Opt::max_rank_top { get; set; }
        //        public string __invalid_name__Opt::threshold_override { get; set; }
        //        public string __invalid_name__Opt::disallow_same_domain { get; set; }
        //        public string __invalid_name__Output::title { get; set; }
        //        public string __invalid_name__Output::want_title_on_right { get; set; }
        //        public string __invalid_name__Output::num_lines1 { get; set; }
        //        public string __invalid_name__Output::text1 { get; set; }
        //        public string __invalid_name__Output::gray1b { get; set; }
        //        public string __invalid_name__Output::no_clip1b { get; set; }
        //        public string __invalid_name__UrlOutput::url2 { get; set; }
        //        public string __invalid_name__Output::link2 { get; set; }
        //        public string __invalid_name__Output::text2b { get; set; }
        //        public string __invalid_name__UrlOutput::url2c { get; set; }
        //        public string __invalid_name__Output::link2c { get; set; }
        //        public string result_group_header { get; set; }
        //        public string __invalid_name__Output::image_url { get; set; }
        //        public string image_size { get; set; }
        //        public string __invalid_name__Output::inline_image_width { get; set; }
        //        public string __invalid_name__Output::inline_image_height { get; set; }
        //        public string __invalid_name__Output::image_border { get; set; }
        //    }

        //    [DataContract]
        //    public class Pagemap
        //    {
        //        public List<RTO> RTO { get; set; }
        //    }

        //    [DataContract]
        //    public class Item
        //    {
        //        public string kind { get; set; }
        //        public string title { get; set; }
        //        public string htmlTitle { get; set; }
        //        public string link { get; set; }
        //        public string displayLink { get; set; }
        //        public string snippet { get; set; }
        //        public string htmlSnippet { get; set; }
        //        public Pagemap pagemap { get; set; }
        //    }
        //}

        //public string kind { get; set; }
        //public Url url { get; set; }
        //public Queries queries { get; set; }
        //public Context context { get; set; }
        //public List<Item> items { get; set; }
    }
}

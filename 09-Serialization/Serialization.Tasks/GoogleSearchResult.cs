
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



    /*    [DataContract]
        public class GoogleSearchResult
        {
            [DataMember]
            public string Title { get; set; }
            [DataMember]
            public string TotalResults { get; set; }
            [DataMember]
            public string SearchTerms { get; set; }
            [DataMember]
            public string Count { get; set; }
            [DataMember]
            public string StartIndex { get; set; }
            [DataMember]
            public string InputEncoding { get; set; }
            [DataMember]
            public string OutputEncoding { get; set; }
            [DataMember]
            public string Cx { get; set; }
            [DataMember]
            public string HtmlTitle { get; set; }
            [DataMember]
            public string DisplayLink { get; set; }
            [DataMember]
            public string Snippet { get; set; }
            [DataMember]
            public string HtmlSnippet { get; set; }
    }*/
    
        [DataContract]
        public class Url
        {

            [DataMember(Name = "type")]
            public string Type { get; set; }

            [DataMember(Name = "template")]
            public string Template { get; set; }
        }

        [DataContract]
        public class NextPage
        {

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "totalResults")]
            public Int64 TotalResults { get; set; }

            [DataMember(Name = "searchTerms")]
            public string SearchTerms { get; set; }

            [DataMember(Name = "count")]
            public int Count { get; set; }

            [DataMember(Name = "startIndex")]
            public int StartIndex { get; set; }

            [DataMember(Name = "inputEncoding")]
            public string InputEncoding { get; set; }

            [DataMember(Name = "outputEncoding")]
            public string OutputEncoding { get; set; }

            [DataMember(Name = "cx")]
            public string Cx { get; set; }
        }

        [DataContract]
        public class Request
        {

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "totalResults")]
            public Int64 TotalResults { get; set; }

            [DataMember(Name = "searchTerms")]
            public string SearchTerms { get; set; }

            [DataMember(Name = "count")]
            public int Count { get; set; }

            [DataMember(Name = "startIndex")]
            public int StartIndex { get; set; }

            [DataMember(Name = "inputEncoding")]
            public string InputEncoding { get; set; }

            [DataMember(Name = "outputEncoding")]
            public string OutputEncoding { get; set; }

            [DataMember(Name = "cx")]
            public string Cx { get; set; }
        }

        [DataContract]
        public class Queries
        {

            [DataMember(Name = "nextPage")]
            public IList<NextPage> NextPage { get; set; }

            [DataMember(Name = "request")]
            public IList<Request> Request { get; set; }

            [DataMember(Name = "PreviousPage")]
            public IList<int?> PreviousPage { get; set; }
    }

        [DataContract]
        public class Context
        {

            [DataMember(Name = "title")]
            public string Title { get; set; }
        }

        [DataContract]
        public class RTO
        {

            [DataMember(Name = "format")]
            public string Format { get; set; }

            [DataMember(Name = "group_impression_tag")]
            public string GroupImpressionTag { get; set; }

            [DataMember(Name = "Opt::max_rank_top")]
            public string OptMaxRankTop { get; set; }

            [DataMember(Name = "Opt::threshold_override")]
            public string OptThresholdOverride { get; set; }

            [DataMember(Name = "Opt::disallow_same_domain")]
            public string OptDisallowSameDomain { get; set; }

            [DataMember(Name = "Output::title")]
            public string OutputTitle { get; set; }

            [DataMember(Name = "Output::want_title_on_right")]
            public string OutputWantTitleOnRight { get; set; }

            [DataMember(Name = "Output::num_lines1")]
            public string OutputNumLines1 { get; set; }

            [DataMember(Name = "Output::text1")]
            public string OutputText1 { get; set; }

            [DataMember(Name = "Output::gray1b")]
            public string OutputGray1b { get; set; }

            [DataMember(Name = "Output::no_clip1b")]
            public string OutputNoClip1b { get; set; }

            [DataMember(Name = "UrlOutput::url2")]
            public string UrlOutputUrl2 { get; set; }

            [DataMember(Name = "Output::link2")]
            public string OutputLink2 { get; set; }

            [DataMember(Name = "Output::text2b")]
            public string OutputText2b { get; set; }

            [DataMember(Name = "UrlOutput::url2c")]
            public string UrlOutputUrl2c { get; set; }

            [DataMember(Name = "Output::link2c")]
            public string OutputLink2c { get; set; }

            [DataMember(Name = "result_group_header")]
            public string ResultGroupHeader { get; set; }

            [DataMember(Name = "Output::image_url")]
            public string OutputImageUrl { get; set; }

            [DataMember(Name = "image_size")]
            public string ImageSize { get; set; }

            [DataMember(Name = "Output::inline_image_width")]
            public string OutputInlineImageWidth { get; set; }

            [DataMember(Name = "Output::inline_image_height")]
            public string OutputInlineImageHeight { get; set; }

            [DataMember(Name = "Output::image_border")]
            public string OutputImageBorder { get; set; }
        }

        [DataContract]
        public class Pagemap
        {

            [DataMember(Name = "RTO")]
            public IList<RTO> RTO { get; set; }
        }

        [DataContract]
        public class Item
        {

            [DataMember(Name = "kind")]
            public string Kind { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "htmlTitle")]
            public string HtmlTitle { get; set; }

            [DataMember(Name = "link")]
            public string Link { get; set; }

            [DataMember(Name = "displayLink")]
            public string DisplayLink { get; set; }

            [DataMember(Name = "snippet")]
            public string Snippet { get; set; }

            [DataMember(Name = "htmlSnippet")]
            public string HtmlSnippet { get; set; }

            [DataMember(Name = "pagemap")]
            public Pagemap Pagemap { get; set; }
        }

        [DataContract]
        public class GoogleSearchResult
        {
            
            [DataMember(Name = "kind")]
            public string Kind { get; set; }

            [DataMember(Name = "url")]
            public Url Url { get; set; }

            [DataMember(Name = "queries")]
            public Queries Queries { get; set; }

            [DataMember(Name = "context")]
            public Context Context { get; set; }
           

        [DataMember(Name = "items")]
            public IList<Item> Items { get; set; }
        }


    }






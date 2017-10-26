using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncIO
{
    public static class Tasks
    {


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris)
        {

            using (var webClient = new MyWebClient())
            {
                foreach (var link in uris)
                {
                    yield return webClient.DownloadString(link);
                }
                
            }  

        }


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            var tasks = new List<Task<string>>();

            foreach (var url in uris)
            {
                tasks.Add(ProcessUrlAsync(url.ToString()));
                if (tasks.Count - tasks.Count(x => x.IsCompleted) >= maxConcurrentStreams)
                    Task.WaitAny(tasks.Where(x => !x.IsCompleted).ToArray());
            }


            Task.WaitAll(tasks.ToArray());
            foreach (var page in tasks)
                yield return page.Result;

            async Task<string> ProcessUrlAsync(string url)
            {
                using (var webClient = new MyWebClient())
                {
                    return await webClient.DownloadStringTaskAsync(new Uri(url));
                }
            }
        }

        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static async Task<string> GetMD5Async(this Uri resource)
        {

            var webClient = new WebClient();


            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = await webClient.DownloadDataTaskAsync(resource);
                return BitConverter.ToString(md5.ComputeHash(hash)).Replace("-",string.Empty);
            }
            
        }
    }

    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }

}

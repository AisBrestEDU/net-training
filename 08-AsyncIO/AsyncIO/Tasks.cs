using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
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
            var fetchedData = new List<string>();

            using (var client = new AutomaticDecompressionWebClient())
            {
                foreach (var uri in uris)
                {
                    fetchedData.Add(client.DownloadString(uri));
                }
            }
            return fetchedData;
        }


        //additional class for decompression
        public class AutomaticDecompressionWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;
                if (request == null) throw new InvalidOperationException();
                request.AutomaticDecompression = DecompressionMethods.GZip;

                return request;
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
            /*var data = uris.ToDictionary(item => item.Host, item => string.Empty);

            Parallel.ForEach(uris, new ParallelOptions() {MaxDegreeOfParallelism = maxConcurrentStreams}, (uri) =>
            {
                var client = new AutomaticDecompressionWebClient();
                var str = client.DownloadStringTaskAsync(uri).Result;
                data[uri.Host] = str;
            });

            return data.Select(item => item.Value);*/

            List<Task<string>> tasks = new List<Task<string>>();

            foreach (var uri in uris)
            {
                if (tasks.Count(item => !item.IsCompleted) >= maxConcurrentStreams)
                {
                    Task.WaitAny(tasks.Where(item => !item.IsCompleted).ToArray());
                }

                using (var client = new AutomaticDecompressionWebClient())
                {
                    tasks.Add(client.DownloadStringTaskAsync(uri));
                }

            }
            Task.WaitAll(tasks.ToArray());
            return tasks.Select(item => item.Result);
            
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
            MD5 hashMd5 = MD5.Create();
            byte[] fetchedData = await new WebClient().DownloadDataTaskAsync(resource);

            return BitConverter.ToString(hashMd5.ComputeHash(fetchedData)).Replace("-", string.Empty);
        }

    }



}

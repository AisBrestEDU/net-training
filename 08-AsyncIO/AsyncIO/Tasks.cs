using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Collections.Concurrent;

namespace AsyncIO
{
    public static class Tasks
    {

		class ExWebClient1 : WebClient
		{

			protected override WebRequest GetWebRequest(Uri address)
			{
				HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
				request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.None;

				return request;
			}
		}
		/// <summary>
		/// Returns the content of required uris.
		/// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
		/// </summary>
		/// <param name="uris">Sequence of required uri</param>
		/// <returns>The sequence of downloaded url content</returns>
		public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris)
		{
			using (var client = new ExWebClient1())
			{
				foreach (var item in uris)
				{
					yield return client.DownloadString(item);
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
			List<Task<string>> list = new List<Task<string>>();
			foreach (var item in uris)
			{
				if (list.Count(t => !t.IsCompleted) >= maxConcurrentStreams)
				{
					Task.WaitAny(list.Where(t => !t.IsCompleted).ToArray());
				}

				var client = new ExWebClient1();
				var task = client.DownloadStringTaskAsync(item);
				list.Add(task);

			}

			Task.WaitAll(list.ToArray());
			return list.Select(t => t.Result);
		}


		/// <summary>
		/// Calculates MD5 hash of required resource.
		/// 
		/// Method has to run asynchronous. 
		/// Resource can be any of type: http page, ftp file or local file.
		/// </summary>
		/// <param name="resource">Uri of resource</param>
		/// <returns>MD5 hash</returns>
		public static Task<string> GetMD5Async(this Uri resource)
        {
			throw new NotImplementedException();
        }

    }



}

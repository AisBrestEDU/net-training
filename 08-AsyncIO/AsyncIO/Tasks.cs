using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
	        //TODO: Implement GetUrlContent
	        //throw new NotImplementedException();

	        using (var client = new DecompressionWebClient())
	        {
		        foreach (var uri in uris)
		        {
			        yield return client.DownloadString(uri);
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
			// TODO : Implement GetUrlContentAsync
			throw new NotImplementedException();

			//List<Task> list = new List<Task>(maxConcurrentStreams);

			//foreach (var uri in uris)
			//{
			//	list.Add(new DecompressionWebClient().DownloadStringTaskAsync(uri));
			//}

			//int count = maxConcurrentStreams;
			//List<Task<string>> list = new List<Task<string>>();
			//var list1 = new List<Task>();

			//foreach (var uri in uris)
			//{
			//	list.Add(new DecompressionWebClient().DownloadStringTaskAsync(uri));
			//}

			//while (list.Count > 0)
			//{
			//	yield return list[0].Result;
			//	list.RemoveAt(0);
			//}
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
			// TODO : Implement GetMD5Async
			//throw new NotImplementedException();

	        using (var client = new WebClient())
	        {
				var result = await client.DownloadDataTaskAsync(resource);
				return BitConverter.ToString(MD5.Create().ComputeHash(result)).Replace("-", String.Empty);
			}
		}
	}

	class DecompressionWebClient : WebClient
	{
		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
			request.AutomaticDecompression = DecompressionMethods.GZip;
			return request;
		}
	}
}

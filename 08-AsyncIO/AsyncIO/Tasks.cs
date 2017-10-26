using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
		public static  IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
		{
			// TODO : Implement GetUrlContentAsync
			//throw new NotImplementedException();


			var list = new List<Task<string>>();

			foreach (var uri in uris)
			{
				if (list.Count(n => !n.IsCompleted) >= maxConcurrentStreams)
				{
					Task.WaitAny(list.Where(n => !n.IsCompleted).ToArray());
				}

				list.Add(new DecompressionWebClient().DownloadStringTaskAsync(uri));
			}

			Task.WaitAll(list.ToArray());

			foreach (var task in list)
			{
				yield return task.Result;
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
			// TODO : Implement GetMD5Async
			//throw new NotImplementedException();

	        using (var alhoritm = MD5.Create())
	        {
		        return BitConverter.ToString(alhoritm.ComputeHash(await new WebClient()
			        .DownloadDataTaskAsync(resource))).Replace("-", string.Empty);
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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace IOStreams
{

	public static class TestTasks
	{
		/// <summary>
		/// Parses Resourses\Planets.xlsx file and returns the planet data: 
		///   Jupiter     69911.00
		///   Saturn      58232.00
		///   Uranus      25362.00
		///    ...
		/// See Resourses\Planets.xlsx for details
		/// </summary>
		/// <param name="xlsxFileName">source file name</param>
		/// <returns>sequence of PlanetInfo</returns>
		public static IEnumerable<PlanetInfo> ReadPlanetInfoFromXlsx(string xlsxFileName)
		{
			// TODO : Implement ReadPlanetInfoFromXlsx method using System.IO.Packaging + Linq-2-Xml

			// HINT : Please be as simple & clear as possible.
			//        No complex and common use cases, just this specified file.
			//        Required data are stored in Planets.xlsx archive in 2 files:
			//         /xl/sharedStrings.xml      - dictionary of all string values
			//         /xl/worksheets/sheet1.xml  - main worksheet

			var uri1 = new Uri("/xl/worksheets/sheet1.xml", UriKind.Relative);
			var uri2 = new Uri("/xl/sharedStrings.xml", UriKind.Relative);

			using (var pac = Package.Open(xlsxFileName))
			{
				var part1 = pac.GetPart(uri1);
				var part2 = pac.GetPart(uri2);
				var planetNamesDoc = new XDocument();
				var distansionsDoc = new XDocument();
				
				using (var stream = part2.GetStream())
				{
					planetNamesDoc = XDocument.Load(stream);
				}

				var ns = planetNamesDoc.Root.GetDefaultNamespace();
				var planetsName = planetNamesDoc.Root.Descendants(ns + "t").Select(n => n.Value);

				using (var stream = part1.GetStream())
				{
					distansionsDoc = XDocument.Load(stream);
				}

				ns = distansionsDoc.Root.GetDefaultNamespace();

				return distansionsDoc.Root.Descendants(ns + "v")
					.Select(n => n.Value)
					.Skip(3)
					.Where((n, index) => index % 2 == 0)
					.Zip(planetsName, (distance, name) => new PlanetInfo {Name = name, MeanRadius = double.Parse(distance)});
			}
		}


		/// <summary>
		/// Calculates hash of stream using specifued algorithm
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
			// TODO : Implement CalculateHash method
			//throw new NotImplementedException();

			var algorithm = HashAlgorithm.Create(hashAlgorithmName);

			if (algorithm == null)
			{
				throw new ArgumentException();
			}

			var hash = algorithm.ComputeHash(stream);

			return BitConverter.ToString(hash).Replace("-", String.Empty);
		}


		/// <summary>
		/// Returns decompressed strem from file. 
		/// </summary>
		/// <param name="fileName">source file</param>
		/// <param name="method">method used for compression (none, deflate, gzip)</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
			// TODO : Implement DecompressStream method

			var fs = new FileStream(fileName, FileMode.Open);

			if (method == DecompressionMethods.GZip)
			{
				return new GZipStream(fs,CompressionMode.Decompress);
			}

			else if(method == DecompressionMethods.Deflate)
			{
				return new DeflateStream(fs, CompressionMode.Decompress);
			}

			else
			{
				return fs;
			}
		}


		/// <summary>
		/// Reads file content econded with non Unicode encoding
		/// </summary>
		/// <param name="fileName">source file name</param>
		/// <param name="encoding">encoding name</param>
		/// <returns>Unicoded file content</returns>
		public static string ReadEncodedText(string fileName, string encoding)
		{
			// TODO : Implement ReadEncodedText method
			//throw new NotImplementedException();

			return File.ReadAllText(fileName, Encoding.GetEncoding(encoding));
		}
	}


	public class PlanetInfo : IEquatable<PlanetInfo>
	{
		public string Name { get; set; }
		public double MeanRadius { get; set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", Name, MeanRadius);
		}

		public bool Equals(PlanetInfo other)
		{
			return Name.Equals(other.Name)
				&& MeanRadius.Equals(other.MeanRadius);
		}
	}



}

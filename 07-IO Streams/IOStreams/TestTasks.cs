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
			// HINT : Please be as simple & clear as possible.
			//        No complex and common use cases, just this specified file.
			//        Required data are stored in Planets.xlsx archive in 2 files:
			//         /xl/sharedStrings.xml      - dictionary of all string values
			//         /xl/worksheets/sheet1.xml  - main worksheet

			IEnumerable<string> planetNames;
			IEnumerable<double> radiuses;

			using (System.IO.Packaging.Package pac = Package.Open(xlsxFileName))
			{
				var sharedStringsUri = new Uri("/xl/sharedStrings.xml", UriKind.Relative);
				var sheetUri = new Uri("/xl/worksheets/sheet1.xml", UriKind.Relative);
				
				using (var stringStream = pac.GetPart(sharedStringsUri).GetStream() )
				{
					var root = XDocument.Load(stringStream).Root;
					var ns = root.Name.Namespace;

					planetNames = root.Elements(ns + "si").Select(item => item.Element(ns + "t").Value).Take(8);
						
				}
				using (var sheetStream = pac.GetPart(sheetUri).GetStream())
				{
					var root = XDocument.Load(sheetStream).Root;
					var ns = root.Name.Namespace;

					radiuses = (from cell in root.Element(ns + "sheetData").Descendants(ns + "c")
								where cell.Attribute("r").Value.Contains("B")
								select cell.Element(ns + "v").Value)
							   .Skip(1).Select(item => double.Parse(item, System.Globalization.NumberStyles.AllowDecimalPoint));
				}
			}

			return Enumerable.Zip(planetNames, radiuses, (name, radius) => new PlanetInfo { Name = name, MeanRadius = radius });
		}


		/// <summary>
		/// Calculates hash of stream using specifued algorithm
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
			var hash = HashAlgorithm.Create(hashAlgorithmName) ?? throw new ArgumentException();
			return hash.ComputeHash(stream).Aggregate(string.Empty, (seed, item) => seed + item.ToString("X2"));
		}


		/// <summary>
		/// Returns decompressed strem from file. 
		/// </summary>
		/// <param name="fileName">source file</param>
		/// <param name="method">method used for compression (none, deflate, gzip)</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
			FileStream file = new FileStream(fileName, FileMode.Open);
			switch(method)
			{
				case DecompressionMethods.Deflate:
					return new DeflateStream(file, CompressionMode.Decompress);
				case DecompressionMethods.GZip:
					return new GZipStream(file, CompressionMode.Decompress);
				default:
					return file;
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
			return File.ReadAllText(fileName,Encoding.GetEncoding(encoding));
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

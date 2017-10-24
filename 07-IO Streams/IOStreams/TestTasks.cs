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

		    IEnumerable<string> planetNames;
		    IEnumerable<double> meanRadius;
		    using (FileStream fs = new FileStream(xlsxFileName, FileMode.Open, FileAccess.Read))
		    {
		        using (var archive = ZipPackage.Open(fs))
		        {
		            var sharedStrings = archive.GetPart(new Uri(@"/xl/sharedStrings.xml", UriKind.Relative)).GetStream();
		            var sheet1 = archive.GetPart(new Uri(@"/xl/worksheets/sheet1.xml", UriKind.Relative)).GetStream();

		            var sharedStringsRoot = XDocument.Load(sharedStrings);
		            var sheetRoot = XDocument.Load(sheet1);

		            planetNames = (from item in sharedStringsRoot.Root.Descendants()
		                where item.Name.LocalName == "t"
		                select item.Value).Take(8).ToList();

		            meanRadius = (from item in sheetRoot.Root.Descendants()
		                where item.Name.LocalName == "v" && item.Parent.Attribute("t") == null
		                select Convert.ToDouble(item.Value)).ToList();
		        }
		    }

		    return planetNames.Zip(meanRadius, (p, r) => new PlanetInfo{Name = p, MeanRadius = r});
		}


		/// <summary>
		/// Calculates hash of stream using specifued algorithm
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
		/// <returns></returns>
		public static string CalculateHash(this Stream stream, string hashAlgorithmName)
		{
		    HashAlgorithm hash = HashAlgorithm.Create(hashAlgorithmName)??throw new ArgumentException();

		    byte[] computedHash = hash.ComputeHash(stream);
		    return BitConverter.ToString(computedHash).Replace("-", String.Empty);
		}


		/// <summary>
		/// Returns decompressed strem from file. 
		/// </summary>
		/// <param name="fileName">source file</param>
		/// <param name="method">method used for compression (none, deflate, gzip)</param>
		/// <returns>output stream</returns>
		public static Stream DecompressStream(string fileName, DecompressionMethods method)
		{
		    FileStream stream = new FileStream(fileName, FileMode.Open);

		    if (method == DecompressionMethods.Deflate)
		    {
		        return new DeflateStream(stream, CompressionMode.Decompress);
		    }
		    if (method == DecompressionMethods.GZip)
		    {
		        return new GZipStream(stream, CompressionMode.Decompress);
		    }

		    return stream;
		}


		/// <summary>
		/// Reads file content econded with non Unicode encoding
		/// </summary>
		/// <param name="fileName">source file name</param>
		/// <param name="encoding">encoding name</param>
		/// <returns>Unicoded file content</returns>
		public static string ReadEncodedText(string fileName, string encoding)
		{
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

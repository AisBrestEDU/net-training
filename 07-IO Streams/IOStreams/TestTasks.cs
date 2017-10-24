using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace IOStreams
{
    public static class TestTasks
    {
        /// <summary>
        ///     Parses Resourses\Planets.xlsx file and returns the planet data:
        ///     Jupiter     69911.00
        ///     Saturn      58232.00
        ///     Uranus      25362.00
        ///     ...
        ///     See Resourses\Planets.xlsx for details
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
            using (var planetsFile = new FileStream(xlsxFileName, FileMode.Open, FileAccess.Read))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                var zipFile = Package.Open(planetsFile);
                var planetsZip = zipFile.GetPart(new Uri(@"/xl/sharedStrings.xml", UriKind.Relative)).GetStream();
                var radiusZip = zipFile.GetPart(new Uri(@"/xl/worksheets/sheet1.xml", UriKind.Relative)).GetStream();
                var planetsXml = XDocument.Load(planetsZip);
                var radiusXml = XDocument.Load(radiusZip);
                var planets = planetsXml.Root.Descendants().Where(t => t.Name.LocalName == "t").Take(8)
                    .Select(p => p.Value).ToArray();
                var radiusArray = radiusXml.Root.Descendants()
                    .Where(r => r.Name.LocalName == "v")
                    .Select(r => double.Parse(r.Value))
                    .Where(r => r > 100).ToArray();
                return planets.Zip(radiusArray, (p, r) => new PlanetInfo {MeanRadius = r, Name = p});
            }
        }


        /// <summary>
        ///     Calculates hash of stream using specifued algorithm
        /// </summary>
        /// <param name="stream">source stream</param>
        /// <param name="hashAlgorithmName">hash algorithm ("MD5","SHA1","SHA256" and other supported by .NET)</param>
        /// <returns></returns>
        public static string CalculateHash(this Stream stream, string hashAlgorithmName)
        {
            var hasher = HashAlgorithm.Create(hashAlgorithmName) ?? throw new ArgumentException();
            return BitConverter.ToString(hasher.ComputeHash(stream)).Replace("-", "");
        }


        /// <summary>
        ///     Returns decompressed stream from file.
        /// </summary>
        /// <param name="fileName">source file</param>
        /// <param name="method">method used for compression (none, deflate, gzip)</param>
        /// <returns>output stream</returns>
        public static Stream DecompressStream(string fileName, DecompressionMethods method)
        {
            var result = new FileStream(fileName, FileMode.Open);

            if (DecompressionMethods.Deflate == method)
                return new DeflateStream(result, CompressionMode.Decompress);
            if (DecompressionMethods.GZip == method)
                return new GZipStream(result, CompressionMode.Decompress);
            if (DecompressionMethods.None == method)
                return result;
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Reads file content econded with non Unicode encoding
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
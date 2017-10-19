using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LinqToXml
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            var inptXml = XDocument.Parse(xmlRepresentation);
            var newData = new XDocument(new XElement(inptXml.Root.Name,
                inptXml.Root.Elements()
                .Where(x => x.Name.LocalName == "Data")
                .GroupBy(x => x.Element("Category").Value)
                .Select(x => new XElement("Group", new XAttribute("ID", x.Key),
                    x.Select(p => new XElement("Data",
                        p.Element("Quantity"),
                        p.Element("Price"))
                 )))
                 ));

            //var doc = XDocument.Parse(xmlRepresentation);
            //var newData =
            //    new XElement("Root",
            //        from data in doc.Root.Elements("Data")
            //        group data by (string)data.Element("Category") into groupedData
            //        select new XElement("Group",
            //            new XAttribute("ID", groupedData.Key),
            //            from g in groupedData
            //            select new XElement("Data",
            //                g.Element("Quantity"),
            //                g.Element("Price")
            //            )
            //        )
            //    );

            return newData.ToString();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            var inptXml = XDocument.Parse(xmlRepresentation);

            return String.Join(",",
                
                inptXml.Root.Descendants()
                .Where(x => x.Name.LocalName=="State" && x.Parent.LastAttribute.Value== "Shipping" && x.Value == "NY")
                .SelectMany(x => x.Parent.Parent.Attributes()
                    .Where(a=>a.Name.LocalName=="PurchaseOrderNumber")
                    .Select(p => p.Value).ToArray())
                .Distinct().ToArray());
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            // throw new NotImplementedException();

            var str = new XDocument(new XElement("Root",

                from s in customers.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    let c=s.Split(',')
                select
                    new XElement("Customer",
                        new XAttribute("CustomerID",c[0]),
                        new XElement("CompanyName",c[1]),
                        new XElement("ContactName",c[2]),
                        new XElement("ContactTitle",c[3]),
                        new XElement("Phone",c[4]),
                        new XElement("FullAddress",
                            new XElement("Address",c[5]),
                            new XElement("City",c[6]),
                            new XElement("Region",c[7]),
                            new XElement("PostalCode",c[8]),
                            new XElement("Country",c[9])))
                ));

            return str.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            return XDocument.Parse(xmlRepresentation).Root.Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            var doc = XDocument.Parse(xmlRepresentation);

            doc.Root.ReplaceAll(doc.Root.Elements().Select(x => new XElement("contact", x.Elements())));

            return doc.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            var doc = XDocument.Parse(xmlRepresentation);

            return doc.Root.Elements()
                .Where(x => x.Elements()
                    .Count(s => s.Name.LocalName == "subscriber") >= 2 && x.Nodes().Any(s => s.NodeType == System.Xml.XmlNodeType.Comment))
                .Select(x => int.Parse( x.FirstAttribute.Value));
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            //throw new NotImplementedException();

            var doc = XDocument.Parse(xmlRepresentation);

            var str = new XDocument(new XElement("Root",
                doc.Root.Elements()
                .OrderBy(c => c.Element("FullAddress").Element("Country").Value)
                .ThenBy(c => c.Element("FullAddress").Element("City").Value)
                .Select(c => c)
                ));

            return str.ToString();

        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            throw new NotImplementedException();
        }
    }
}

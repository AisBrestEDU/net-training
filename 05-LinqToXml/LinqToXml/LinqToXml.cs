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
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Element("Root");
            xRoot.Element("TaxRate").Remove();
            var categories = xRoot.Elements().Select(x => x.Element("Category").Value);
            categories = categories.Distinct().OrderBy(x => x);
            var groups = categories.Select(x => {
                XElement group = (
                    new XElement("Group",
                        new XAttribute("ID", x),
                        xRoot
                        .Elements()
                        .Where(y => y.Element("Category").Value == x)
                        .Select(
                            y => new XElement("Data",
                            new XElement("Quantity", y.Element("Quantity").Value),
                            new XElement("Price", y.Element("Price").Value) )
                        )
                    )
             );
                return group;
            });
            xRoot.ReplaceAll(groups);
            return xDocument.ToString();
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
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Root;
            var orderNemberOfShiping = xRoot
                .Elements()
                .Where(x => x.Elements().First().Elements().Skip(3).FirstOrDefault().Value == "NY")
                .Select(x => x.FirstAttribute.Value);
            return string.Join(",", orderNemberOfShiping);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            var csvCustomers = customers.Split("\r\n".ToArray()).Where((x, i) => i % 2 == 0).ToArray();
            XElement xCustomers = new XElement("Root",
                from customer in csvCustomers
                let fields = customer.Split(',')
                select new XElement("Customer",
                    new XAttribute("CustomerID", fields[0]),
                    new XElement("CompanyName", fields[1]),
                    new XElement("ContactName", fields[2]),
                    new XElement("ContactTitle", fields[3]),
                    new XElement("Phone", fields[4]),
                    new XElement("FullAddress",
                        new XElement("Address", fields[5]),
                        new XElement("City", fields[6]),
                        new XElement("Region", fields[7]),
                        new XElement("PostalCode", fields[8]),
                        new XElement("Country", fields[9])
                        )
                    )
                );
            return xCustomers.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Root;
            return string
                .Concat(
                    xRoot
                    .Elements()
                    .SelectMany(x => x.Value)
                );
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            return xmlRepresentation.Replace("customer", "contact");
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Root;
            var channels = xRoot.Elements()
                                .Where(x => x.Elements().Count() >= 2 && x.DescendantNodes().OfType<XComment>().Count() > 0)
                                .Select(x => x.Attribute("id").Value);
            return channels.Select(x => int.Parse(x));
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Root;

            var orderCustomers = xRoot.Elements()
                                      .OrderBy(x => x.Element("FullAddress").Element("Country").Value)
                                      .ThenBy(x => x.Element("FullAddress").Element("City").Value);
            xRoot.ReplaceAll(orderCustomers);
            return xRoot.ToString();
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
            return string.IsNullOrEmpty(xmlRepresentation.ToString()) 
                ? null 
                : xmlRepresentation.ToString();
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            XDocument xDocument = XDocument.Parse(xmlRepresentation);
            XElement xRoot = xDocument.Root;

            int totalPurchaseValue = 0;
            var ordersOfProduct = xRoot.Element("Orders").Elements()
                                       .Select(x => x.Element("product").Value);
            var products = xRoot.Element("products").Elements()
                                .Select(x => new { id = x.Attribute("Id").Value, value = x.Attribute("Value").Value });
            foreach (var order in ordersOfProduct)
            {
                totalPurchaseValue += int.Parse(products.FirstOrDefault(x => x.id == order).value);
            }
            return totalPurchaseValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            XElement root = XElement.Parse(xmlRepresentation);

            var groupData = 
                from data in root.Elements("Data")
                    group data by data.Element("Category")?.Value;

            XElement result = new XElement("Root",
                from grouping in groupData
                    select new XElement("Group", new XAttribute("ID", grouping.Key),
                        from groupItem in grouping
                            select new XElement("Data", 
                                groupItem.Element("Quantity"), 
                                groupItem.Element("Price"))));

            return result.ToString();
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
            var root = XElement.Parse(xmlRepresentation);
            var ns = root.GetNamespaceOfPrefix("aw");


            var result = from item in root.Elements()
                where item.Elements().Any(x => x.Attribute(ns + "Type").Value.Equals("Shipping"))
                where item.Elements(ns + "Address").All(y => y.Element(ns + "State").Value.Equals("NY"))
                select item.Attribute(ns + "PurchaseOrderNumber")?.Value;

            return string.Join(",", result);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            string [] customersList = customers.Split(new[]{"\r\n"}, StringSplitOptions.None);

            var customersXmlTree = new XElement("Root",
                from cus in customersList
                let fields = cus.Split(',')
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
                                new XElement("Country", fields[9]))));

            return customersXmlTree.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
/*
            var root = XElement.Parse(xmlRepresentation);
            var result = from item in root.Elements().Elements() select item.Value;
            string str = string.Join("", result);
            return str;
*/
            return XElement.Parse(xmlRepresentation).Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            var root = XElement.Parse(xmlRepresentation);
            root.ReplaceAll(
                from item in root.Elements()
                select new XElement("contact", item.Elements()));
            return root.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            var root = XElement.Parse(xmlRepresentation);

            var channelsIdList = from item in root.Elements("channel")
                where item.Nodes().Any(x => x.GetType() == typeof(XComment)) && item.Elements("subscriber").Count() >= 2
                select int.Parse(item.Attribute("id")?.Value ?? throw new InvalidOperationException());

            return channelsIdList;
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            var root = XElement.Parse(xmlRepresentation);

            var sortedCustomers = from item in root.Elements()
                orderby item.Element("FullAddress")?.Element("Country")?.Value, item.Element("FullAddress")
                    ?.Element("City")
                    ?.Value
                select item;

            return new XElement("Root", sortedCustomers).ToString();

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
            return xmlRepresentation.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            var root = XElement.Parse(xmlRepresentation);

            var totalValueOfOrders = from order in root.Element("Orders")?.Elements()
                join product in root.Element("products")?.Elements()
                    on order.Element("product")?.Value equals product.Attribute("Id")?.Value
                select int.Parse(product.Attribute("Value")?.Value ?? throw new InvalidOperationException());

            return totalValueOfOrders.Sum();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
	        var doc = XElement.Parse(xmlRepresentation);

	        var output =
		        new XElement("Root",
			        from data in doc.Elements("Data")
			        group data by data.Element("Category").Value
			        into grData
			        select new XElement("Group",
				        new XAttribute("ID", grData.Key),
				        from n in grData
				        select new XElement("Data", n.Element("Quantity"), n.Element("Price"))
			        )
		        );

	        return output.ToString();
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
	        var doc = XElement.Parse(xmlRepresentation);
			XNamespace ns = "http://www.adventure-works.com";

			var leList = from data in doc.Elements(ns + "PurchaseOrder")
						 where data.Elements().Any(c => c.Attribute(ns + "Type").Value == "Shipping")
						 where data.Elements(ns + "Address").All(n =>
				n.Element(ns + "State").Value.Equals("NY", StringComparison.OrdinalIgnoreCase))
						 select data.Attribute(ns + "PurchaseOrderNumber").Value;

			return string.Join(",", leList);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
	        var lines = customers.Split(new char[]{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

			var xml = new XElement("Root",
			 from str in lines
			 let line = str.Split(',')
			 select new XElement("Customer",
			  new XAttribute("CustomerID", line[0]),
			  new XElement("CompanyName", line[1]),
			  new XElement("ContactName", line[2]),
			  new XElement("ContactTitle", line[3]),
			  new XElement("Phone", line[4]),
			  new XElement("FullAddress",
			   new XElement("Address", line[5]),
			   new XElement("City", line[6]),
			   new XElement("Region", line[7]),
			   new XElement("PostalCode", line[8]),
			   new XElement("Country", line[9])
			  )
			 )
			);

			return xml.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
			return XElement.Parse(xmlRepresentation).Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
	        var doc = XElement.Parse(xmlRepresentation);

	        var output = new XElement("Document",
		        doc.Elements().Select(n =>
			        new XElement("contact", n.Element("name"), n.Element("lastname")
				        )
			        )
		        );
	        
	        return output.ToString();
        }

		/// <summary>
		/// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
		/// </summary>
		/// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
		/// <returns>Sequence of channels ids</returns>
		public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
		{
			var doc = XDocument.Parse(xmlRepresentation);

			var output = doc.DescendantNodes().OfType<XComment>()
				.Where(n => n.Parent.Elements("subscriber").Count() > 1 && n.Value.Equals("DELETE"))
				.Select(n => int.Parse(n.Parent.Attribute("id").Value));

			return output;
		}

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
	        var doc = XDocument.Parse(xmlRepresentation);

	        var output = new XElement("Root",
		        doc.Root.Descendants("Customers")
			        .OrderBy(n => n.Element("FullAddress").Element("Country").Value)
			        .ThenBy(n => n.Element("FullAddress").Element("City").Value)
	        ).ToString();

	        return output;
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
			var doc = XElement.Parse(xmlRepresentation);

	        var output = doc.Element("Orders").Elements("Order")
		        .Join(doc.Element("products").Elements(),
			        order => order.Element("product").Value,
			        prod => prod.Attribute("Id").Value,
			        (order, prod) => int.Parse(prod.Attribute("Value").Value)).Sum();

	        return output;
        }
	}
}

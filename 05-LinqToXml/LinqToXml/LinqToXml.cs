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
			var input = XDocument.Parse(xmlRepresentation, LoadOptions.None);
			var output = new XDocument(new XElement(input.Root.Name,
													from data in input.Root.Elements()
													where data.Name == "Data"
													group data by data.Element("Category").Value into dataGroup
													select new XElement("Group", new XAttribute("ID", dataGroup.Key),
																		from groupItem in dataGroup
																		select new XElement("Data",	groupItem.Element("Quantity"), groupItem.Element("Price"))
										)));
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
			XNamespace aw = "http://www.adventure-works.com";

			var input = XDocument.Parse(xmlRepresentation, LoadOptions.None);
			var output =	from data in input.Root.Elements( aw + "PurchaseOrder")
							where	( (string)data.Element(aw + "Address").Attribute(aw + "Type") == "Shipping") &&
									(data.Element(aw + "Address").Element(aw + "State").Value == "NY") 
							select data.Attribute(aw + "PurchaseOrderNumber").Value;

			return string.Join(",", output);
		}
		

		/// <summary>
		/// Reads csv representation and creates appropriate xml representation
		/// </summary>
		/// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
		/// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
		public static string ReadCustomersFromCsv(string customers)
        {
			var customersArr = customers.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			var output = new XElement("Root",
									from item in customersArr
									let fields = item.Split(',')
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
				return output.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
			var input = XDocument.Parse(xmlRepresentation);
			// ^-^
			return input.Root.Value;
			
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
			var input = XDocument.Parse(xmlRepresentation);
			input.Root.Elements("customer").ToList().ForEach(item => item.Name = "contact");
			return input.ToString();
		}

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
			var input = XDocument.Parse(xmlRepresentation);
			var output = from item in input.Root.Elements("channel")
						 where	item.Elements("subscriber").Count() > 1 &&
								item.Nodes().Any(x => x is XComment && ((XComment)x).Value == "DELETE")
						select int.Parse(item.Attribute("id").Value);
			return output;
		}

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
			var input = XDocument.Parse(xmlRepresentation);
			var output = new XElement(	input.Root.Name,
										from customer in input.Root.Elements("Customers")
										orderby customer.Element("FullAddress").Element("Country").Value, customer.Element("FullAddress").Element("City").Value
										select customer);
			return output.ToString();
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
			var input = XDocument.Parse(xmlRepresentation);
			
			var joined = from order in input.Root.Element("Orders").Elements("Order")
					join product in input.Root.Element("products").Elements() 
					on order.Element("product").Value equals product.Attribute("Id").Value
					select int.Parse(product.Attribute("Value").Value);
			return joined.Sum();
		}
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

 
	[DataContract(IsReference = true)]
	[KnownType(typeof(Worker))]
    public class Company : IExtensibleDataObject
	{
		[DataMember]
        public string Name { get; set; }

		[DataMember]
        public IList<Employee> Employee { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	[DataContract]
	[KnownType(typeof(Manager))]
    public abstract class Employee {

		[DataMember]
        public string Name { get; set; }

		[DataMember]
        public string LastName { get; set; }

		[DataMember]
        public string Title { get; set; }

        public Manager Manager { get; set; }
    }

	[DataContract]
    public class Worker : Employee {

		[DataMember]
        public int Salary { get; set; }
    }

	[DataContract]
	public class Manager : Employee {

		[DataMember]
        public int YearBonusRate { get; set; } 
    }

}

using System.Collections.Generic;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

 
    public class Company
    {
        public string Name { get; set; }
        public IList<Employee> Employee { get; set; }
    }

    public abstract class Employee {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Manager Manager { get; set; }
    }

    public class Worker : Employee {
        public int Salary { get; set; }
    }

    public class Manager : Employee {
        public int YearBonusRate { get; set; } 
    }

}

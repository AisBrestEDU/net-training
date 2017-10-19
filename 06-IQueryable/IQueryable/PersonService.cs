using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace IQueryableTask
{
    /// <summary>
    /// Data Access Service
    /// </summary>
    public class PersonService
    {
        public IEnumerable<Person> Search(string sql)
        {
            var search = new List<Person>();
            using (SQLiteConnection con = new SQLiteConnection("Data Source=people.db", true))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        search.Add(new Person
                        {
                            Id = rdr.GetInt32(0),
                            FirstName = rdr.GetString(1),
                            LastName = rdr.GetString(2),
                            Sex = (Sex) rdr.GetInt32(3),
                            Age = rdr.GetInt32(4)
                        });
                    }
                }
                con.Close();
            }
            return search;
        }
    }
}

using System.Collections.Generic;
using System.Data.SQLite;

namespace IQueryableTask
{
    /// <summary>
    ///     Data Access Service
    /// </summary>
    public class PersonService
    {
        public IEnumerable<Person> Search(string sql)
        {
            var search = new List<Person>();
            using (var con = new SQLiteConnection("Data Source=people.db", true))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        search.Add(new Person
                        {
                            Id = rdr.GetInt32(0),
                            FirstName = rdr.GetString(1),
                            LastName = rdr.GetString(2),
                            Sex = (Sex) rdr.GetInt32(3),
                            Age = rdr.GetInt32(4)
                        });
                }
                con.Close();
            }
            return search;
        }
    }
}
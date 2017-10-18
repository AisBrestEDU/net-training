namespace IQueryableTask
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }

        public string FullName => FirstName + " " + LastName;
    }

    public enum Sex
    {
        Male,
        Female
    }
}
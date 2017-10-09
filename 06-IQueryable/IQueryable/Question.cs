namespace IQueryableTask
{
    public class Question
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public QuestionType Type { get; set; }
        public string ChosenAnswer { get; set; }
    }

    public enum QuestionType
    {
        All = 0,
        Resolved,
        Open,
        Undecided
    }
}

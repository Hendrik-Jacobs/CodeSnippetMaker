namespace CodeSnippetMaker.Models
{
    public class LiteralModel
    {
        public LiteralModel(string id, string def)
        {
            ID = id;
            Default = def;
        }

        public string ID { get; set; }
        public string Default { get; set; }
    }
}
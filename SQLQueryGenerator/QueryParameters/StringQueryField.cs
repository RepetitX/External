namespace SQLQueryGenerator.QueryParameters
{
    public class StringQueryField : IQueryField
    {
        public string Expression { get; set; }
        public string Alias { get; set; }

        public StringQueryField(string Expression, string Alias)
        {
            this.Expression = Expression;
            this.Alias = Alias;
        }

        public string GetQueryPart()
        {
            throw new System.NotImplementedException();
        }
       
        public string GetTypeName()
        {
            return "String";
        }
    }
}

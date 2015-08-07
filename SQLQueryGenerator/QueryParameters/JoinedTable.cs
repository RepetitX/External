
namespace SQLQueryGenerator.QueryParameters
{
    public enum JoinType
    {
        Inner,
        Left,
        Right,
        Full
    }

    public class JoinedTable
    {
        public Table OuterTable { get; set; }
        public string InnerKey { get; set; }
        public string OuterKey { get; set; }
        public JoinType Type { get; set; }

        public JoinedTable(JoinType Type, Table OuterTable, string InnerKey, string OuterKey)
        {
            this.OuterTable = OuterTable;
            this.InnerKey = InnerKey;
            this.OuterKey = OuterKey;
            this.Type = Type;
        }
    }
}

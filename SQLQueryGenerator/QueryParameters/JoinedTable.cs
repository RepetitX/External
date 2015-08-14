
using System.Text;

namespace SQLQueryGenerator.QueryParameters
{
    public enum JoinType
    {
        Inner,
        Left,
        Right,
        Full
    }

    public class JoinedTable : IQueryPart
    {
        public IQueryTable OuterTable { get; set; }
        public string InnerKey { get; set; }
        public string OuterKey { get; set; }
        public JoinType Type { get; set; }

        public JoinedTable(JoinType Type, IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            this.OuterTable = OuterTable;
            this.InnerKey = InnerKey;
            this.OuterKey = OuterKey;
            this.Type = Type;
        }

        public string GetQueryPart()
        {
            StringBuilder queryPart = new StringBuilder();
            switch (Type)
            {
                case JoinType.Full:
                    queryPart.Append("full outer join ");
                    break;
                case JoinType.Inner:
                    queryPart.Append("inner join ");
                    break;
                case JoinType.Left:
                    queryPart.Append("left join ");
                    break;
                case JoinType.Right:
                    queryPart.Append("right join ");
                    break;
            }
            queryPart.Append(OuterTable.GetQueryPart());
            queryPart.AppendFormat(" on {0}={1}", InnerKey, OuterKey);
            return queryPart.ToString();
        }
    }
}

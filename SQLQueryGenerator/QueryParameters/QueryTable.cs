
using System.Collections.Generic;
using System.Text;

namespace SQLQueryGenerator.QueryParameters
{
    public class QueryTable : IQueryTable
    {
        protected List<JoinedTable> joinedTables { get; set; }

        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public QueryTable(string Expression, string Alias)
        {
            joinedTables = new List<JoinedTable>();
            this.Expression = Expression;
            this.Alias = Alias;
        }

        public QueryTable Join(JoinType Type, IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            joinedTables.Add(new JoinedTable(Type, OuterTable, InnerKey, OuterKey));
            return this;
        }

        public QueryTable InnerJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Inner, OuterTable, InnerKey, OuterKey);
        }

        public QueryTable LeftJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Left, OuterTable, InnerKey, OuterKey);
        }

        public QueryTable RightJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Right, OuterTable, InnerKey, OuterKey);
        }

        public QueryTable InnerJoin(string TableName, string TableAlias, string InnerKey, string OuterKey)
        {
            QueryTable outerTable = new QueryTable(TableName, TableAlias);
            return Join(JoinType.Inner, outerTable, InnerKey, OuterKey);
        }

        public QueryTable LeftJoin(string TableName, string TableAlias, string InnerKey, string OuterKey)
        {
            QueryTable outerTable = new QueryTable(TableName, TableAlias);
            return Join(JoinType.Left, outerTable, InnerKey, OuterKey);
        }

        public QueryTable RightJoin(string TableName, string TableAlias, string InnerKey, string OuterKey)
        {
            QueryTable outerTable = new QueryTable(TableName, TableAlias);
            return Join(JoinType.Right, outerTable, InnerKey, OuterKey);
        }

        public string GetQueryPart()
        {
            StringBuilder queryPart = new StringBuilder();

            queryPart.Append(Expression);
            if (!string.IsNullOrWhiteSpace(Alias) &&
                Expression != Alias)
            {
                queryPart.AppendFormat(" {0}", Alias);
            }
            foreach (var joined in joinedTables)
            {
                queryPart.AppendFormat("\n{0}", joined.GetQueryPart());
            }
            return queryPart.ToString();
        }
    }
}
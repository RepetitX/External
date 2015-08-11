
using System.Collections.Generic;

namespace SQLQueryGenerator.QueryParameters
{
    public class Table : IQueryTable
    {
        protected List<JoinedTable> joinedTables { get; set; }

        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public Table Join(JoinType Type, IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            joinedTables.Add(new JoinedTable(Type, OuterTable, InnerKey, OuterKey));
            return this;
        }
        public Table InnerJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Inner, OuterTable, InnerKey, OuterKey);
        }
        public Table LeftJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Left, OuterTable, InnerKey, OuterKey);
        }
        public Table RightJoin(IQueryTable OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Right, OuterTable, InnerKey, OuterKey);
        }

        public string GetQueryPart()
        {
            throw new System.NotImplementedException();
        }
    }
}
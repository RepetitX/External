
using System.Collections.Generic;

namespace SQLQueryGenerator.QueryParameters
{
    public class Table
    {
        protected List<JoinedTable> joinedTables { get; set; }

        public string Name { get; set; }
        public string Alias { get; set; }

        public Table Join(JoinType Type, Table OuterTable, string InnerKey, string OuterKey)
        {
            joinedTables.Add(new JoinedTable(Type, OuterTable, InnerKey, OuterKey));
            return this;
        }
        public Table InnerJoin(Table OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Inner, OuterTable, InnerKey, OuterKey);
        }
        public Table LeftJoin(Table OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Left, OuterTable, InnerKey, OuterKey);
        }
        public Table RightJoin(Table OuterTable, string InnerKey, string OuterKey)
        {
            return Join(JoinType.Right, OuterTable, InnerKey, OuterKey);
        }
    }
}

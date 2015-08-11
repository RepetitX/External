using System.Collections.Generic;
using SQLQueryGenerator.QueryParameters;

namespace SQLQueryGenerator.QueryGenerators
{
    public class SelectQuery
    {
        protected Dictionary<string, Table> tables;

        public SelectQuery()
        {
            tables  = new Dictionary<string, Table>();
        }
    }
}

using System.Collections.Generic;
using SQLQueryGenerator.QueryParameters;

namespace SQLQueryGenerator.QueryGenerators
{
    public class SelectQueryGenerator
    {
        protected Dictionary<string, Table> tables;

        public SelectQueryGenerator()
        {
            tables  = new Dictionary<string, Table>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLQueryGenerator.QueryParameters;

namespace SQLQueryGenerator.Queries
{
    public class SubQuery : IQueryTable
    {
        public string Expression { get; private set; }
        public string Alias { get; private set; }

        protected SelectQuery query;

        public SubQuery(SelectQuery Query, string Alias)
        {
            this.Alias = Alias;
            query = Query;
        }

        public string GetQueryPart()
        {
            throw new NotImplementedException();
        }
    }
}

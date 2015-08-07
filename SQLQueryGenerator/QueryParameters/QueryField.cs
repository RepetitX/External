using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryGenerator.QueryParameters
{
    public class QueryField : IQueryField
    {
        public string Expression { get; set; }
        public string Alias { get; set; }
        public bool IsSelect { get; set; }
    }
}

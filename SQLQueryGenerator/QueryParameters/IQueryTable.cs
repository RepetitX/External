using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryGenerator.QueryParameters
{
    public interface IQueryTable : IQueryPart
    {
        string Expression { get; }
        string Alias { get; }
    }
}
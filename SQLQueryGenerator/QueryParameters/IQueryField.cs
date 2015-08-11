using System;

namespace SQLQueryGenerator.QueryParameters
{
    public interface IQueryField
    {
        string Expression { get; set; }
        string Alias { get; set; }
    }
}
using System;

namespace SQLQueryGenerator.QueryParameters
{
    public class QueryField<T> : IQueryField
    {
        public string Expression { get; set; }
        public string Alias { get; set; }

        public QueryField(string Expression, string Alias)
        {
        }

        public string GetTypeName()
        {
            return typeof (T).ToString();
        }

        public string GetQueryPart()
        {
            return Expression;
        }
    }
}
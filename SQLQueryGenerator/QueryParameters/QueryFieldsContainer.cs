using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLQueryGenerator.QueryParameters
{
    public class QueryFieldsContainer
    {
        protected Dictionary<string, IQueryField> fields;

        public QueryFieldsContainer()
        {
            fields = new Dictionary<string, IQueryField>();
        }

        public IQueryField GetField(string Alias)
        {
            IQueryField field;
            if (!fields.ContainsKey(Alias))
            {
                field = new StringQueryField(Alias, Alias);
                AddField(field);
            }
            else
            {
                field = fields[Alias];
            }
            return field;
        }
        public QueryField<T> GetQueryField<T>(string Alias) where T : struct
        {
            QueryField<T> field;
            if (!fields.ContainsKey(Alias))
            {
                field = new QueryField<T>(Alias, Alias);
                AddField(field);
            }
            else
            {
                field = (QueryField<T>)fields[Alias];
            }
            return field;
        }

        public void AddField(IQueryField Field)
        {
            if (!fields.ContainsKey(Field.Alias))
            {
                fields.Add(Field.Alias, Field);
            }
        }
    }
}

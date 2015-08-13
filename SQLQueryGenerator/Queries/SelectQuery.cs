using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLQueryGenerator.QueryParameters;

namespace SQLQueryGenerator.Queries
{
    public class SelectQuery
    {
        protected Dictionary<string, IQueryTable> tables;
        protected Dictionary<string, IQueryField> fields;

        protected List<string> selectFields;
        protected List<string> orderFields;

        public SelectQuery()
        {
            tables = new Dictionary<string, IQueryTable>();
        }

        public void AddTable(IQueryTable Table)
        {
            tables.Add(Table.Alias, Table);
        }

        public string GetQueryString()
        {
            StringBuilder queryString = new StringBuilder();

            queryString.Append("select ");

            queryString.Append(
                string.Join(", ", selectFields.Select(GetSelectField))
                );

            queryString.Append("\nfrom\n");
            queryString.Append(
                string.Join(", ", tables.Select(pair => pair.Value.GetQueryPart()))
                );

            return queryString.ToString();
        }

        protected string GetSelectField(string FieldName)
        {
            IQueryField field = fields[FieldName];
            if (field.Expression == field.Alias)
            {
                return FieldName;
            }
            else
            {
                return string.Format("{0} as {1}", field.Expression, field.Alias);
            }
        }
    }
}
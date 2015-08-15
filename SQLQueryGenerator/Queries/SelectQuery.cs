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
        protected List<SortingField> sortingFields;
        protected List<IQueryCondition> conditions;

        public SelectQuery()
        {
            tables = new Dictionary<string, IQueryTable>();
            fields = new Dictionary<string, IQueryField>();
            selectFields = new List<string>();
            sortingFields = new List<SortingField>();
            conditions = new List<IQueryCondition>();
        }

        public void AddTable(IQueryTable Table)
        {
            tables.Add(Table.Alias, Table);
        }

        public QueryTable AddTable(string Name, string Alias)
        {
            QueryTable table = new QueryTable(Name, Alias);
            tables.Add(table.Alias, table);
            return table;
        }

        public void AddCondition(IQueryCondition Condition)
        {
            conditions.Add(Condition);
        }

        public void AddSortingField(IQueryField Field, SortDirection Direction)
        {
            SortingField sField = new SortingField(Field, Direction);
            AddField(Field);

            sortingFields.Add(sField);
        }
        public void AddSortingField(string Alias, SortDirection Direction)
        {
            IQueryField field = GetField(Alias);
            SortingField sField = new SortingField(field, Direction);

            sortingFields.Add(sField);
        }

        protected IQueryField GetField(string Alias)
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

        public void AddField(IQueryField Field)
        {
            if (!fields.ContainsKey(Field.Alias))
            {
                fields.Add(Field.Alias, Field);
            }
        }

        public string GetQueryString()
        {
            StringBuilder queryString = new StringBuilder();

            queryString.Append("select ");

            if (selectFields.Count > 0)
            {
                queryString.Append(
                    string.Join(", ", selectFields.Select(GetSelectField))
                    );
            }
            else
            {
                queryString.Append("*");
            }

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
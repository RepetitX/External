using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using SQLQueryGenerator.QueryParameters;

namespace SQLQueryGenerator.Queries
{
    public class SelectQuery
    {
        protected Dictionary<string, IQueryTable> tables;
        protected QueryFieldsContainer fieldsContainer;

        protected List<string> selectFields;
        protected Dictionary<string, SortDirection> sortingFields;

        public ConditionGroup RootCondition { get; private set; }

        public List<IQueryField> GetSelectFields()
        {
            return selectFields.Select(alias => fieldsContainer.GetField(alias)).ToList();
        }

        public bool Distinct { get; set; }

        public SelectQuery()
            : this(ConditionGroupType.And)
        {
        }

        public SelectQuery(ConditionGroupType RootConditionsType)
        {
            tables = new Dictionary<string, IQueryTable>();
            fieldsContainer = new QueryFieldsContainer();
            selectFields = new List<string>();
            sortingFields = new Dictionary<string, SortDirection>();
            RootCondition = new ConditionGroup(RootConditionsType, fieldsContainer);
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

        public void AddSortingField(IQueryField Field, SortDirection Direction)
        {
            fieldsContainer.AddField(Field);

            sortingFields.Add(Field.Alias, Direction);
        }
        public void AddSortingField(string Alias, SortDirection Direction)
        {
            IQueryField field = fieldsContainer.GetField(Alias);

            sortingFields.Add(Alias, Direction);
        }
        public void AddSelectField(IQueryField Field)
        {
            fieldsContainer.AddField(Field);
            selectFields.Add(Field.Alias);
        }
        public void AddSelectField(string Alias)
        {
            IQueryField field = fieldsContainer.GetField(Alias);
            selectFields.Add(field.Alias);
        }

        public string GetQueryString()
        {
            StringBuilder queryString = new StringBuilder();

            queryString.Append("select ");
            if (Distinct)
            {
                queryString.Append("distinct ");
            }

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

            string conditions = RootCondition.GetQueryPart();
            if (!string.IsNullOrWhiteSpace(conditions))
            {
                queryString.AppendFormat("\nwhere {0}", conditions);
            }
            if (sortingFields.Count > 0)
            {
                queryString.AppendFormat("\norder by {0}",
                    string.Join("\n,", sortingFields.Select(GetSortingField)));
            }

            return queryString.ToString();
        }

        protected string GetSelectField(string Alias)
        {
            IQueryField field = fieldsContainer.GetField(Alias);
            if (field.Expression == field.Alias)
            {
                return Alias;
            }
            else
            {
                return string.Format("{0} as {1}", field.Expression, field.Alias);
            }
        }
        protected string GetSortingField(KeyValuePair<string, SortDirection> SortingField)
        {
            IQueryField field = fieldsContainer.GetField(SortingField.Key);

            return string.Format("{0} {1}",
                selectFields.Contains(field.Alias) ? field.Alias : field.Expression,
                SortingField.Value == SortDirection.Ascending ? "asc" : "desc");
        }
    }
}
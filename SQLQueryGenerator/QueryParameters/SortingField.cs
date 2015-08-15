
using System;

namespace SQLQueryGenerator.QueryParameters
{
    public class SortingField : IQueryPart
    {
        protected IQueryField field;
        protected SortDirection direction;

        public SortingField(IQueryField Field, SortDirection Direction)
        {
            field = Field;
            direction = Direction;
        }

        public string GetQueryPart()
        {
            return string.Format("{0} {1}",
                string.IsNullOrWhiteSpace(field.Alias) ? field.Expression : field.Alias,
                direction == SortDirection.Ascending ? "asc" : "desc");
        }
    }
}
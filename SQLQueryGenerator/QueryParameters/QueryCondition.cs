
using System;
using SQLQueryGenerator.Extensions;

namespace SQLQueryGenerator.QueryParameters
{
    public enum CompareCondition
    {        
        Less,
        LessOrEqual,
        Equal,
        MoreOrEqual,
        More,
        NotEqual
    }

    public enum ListCondition
    {
        In,
        NotIn
    }

    public class QueryCondition<T> : BaseCondition where T : struct
    {
        public QueryCondition(QueryField<T> Field, CompareCondition Condition, T? Value)
        {
            if (!Value.HasValue)
            {
                queryPart = "";
                return;
            }

            queryPart = String.Format("{0} {1} {2}", Field.GetQueryPart(), Condition.GetSign(), Value.Value);
        }
    }
}
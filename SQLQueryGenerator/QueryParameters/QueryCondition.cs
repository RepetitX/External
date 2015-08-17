﻿
using System;
using SQLQueryGenerator.Extensions;
using System.Collections.Generic;

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
        public QueryCondition(QueryField<T> Field, CompareCondition Comparsion, T? Value)
        {
            if (!Value.HasValue)
            {
                queryPart = "";
                return;
            }

            queryPart = String.Format("{0} {1} {2}", Field.GetQueryPart(), Comparsion.GetSign(), Value.Value);
        }
        public QueryCondition(QueryField<T> Field, ListCondition Condition, IEnumerable<T> Values)
        {
            string values = string.Join(", ", Values);

            if (String.IsNullOrWhiteSpace(values))
            {
                queryPart = "";
            }
            else
            {
                queryPart = String.Format("{0} {1} ({2})", Field.GetQueryPart(),
                    Condition == ListCondition.In ? "in" : "not in", values);
            }
        }
    }    
}
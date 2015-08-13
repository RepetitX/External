using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryGenerator.QueryParameters
{
    public enum NullCondition
    {
        IsNull,
        IsNotNull
    }

    public class BaseCondition : IQueryCondition
    {
        protected string queryPart;

        protected BaseCondition()
        {
            queryPart = "";
        }

        public BaseCondition(IQueryField Field, NullCondition Condition)
        {
            queryPart = string.Format("{0} is {1}null", Field.GetQueryPart(),
                Condition == NullCondition.IsNull ? "" : "not ");
        }

        public string GetQueryPart()
        {
            return queryPart;
        }
    }
}
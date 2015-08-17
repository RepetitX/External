using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLQueryGenerator.QueryParameters
{
    public class CustomCondition : IQueryCondition
    {
        protected string expression;

        public CustomCondition(string Expression)
        {
            expression = Expression;
        }

        public string GetQueryPart()
        {
            return expression;
        }
    }
}

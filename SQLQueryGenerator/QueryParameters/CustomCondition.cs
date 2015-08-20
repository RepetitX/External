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
            if (!string.IsNullOrWhiteSpace(expression))
            {
                IsEmpty = false;
            }
        }

        public string GetQueryPart()
        {
            return expression;
        }

        public bool IsEmpty { get; private set; }
        
    }
}

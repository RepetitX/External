using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLQueryGenerator.QueryParameters
{
    public enum ConditionGroupType
    {
        And, 
        Or
    }

    public class ConditionGroup : IQueryCondition
    {
        protected ConditionGroupType type;
        protected List<IQueryCondition> conditions;

        public ConditionGroup(ConditionGroupType Type)
        {
            type = Type;
            conditions = new List<IQueryCondition>();
        }

        public void AddCondition(IQueryCondition Condition)
        {
            conditions.Add(Condition);
        }

        public string GetQueryPart()
        {
            return string.Join("\nand", conditions.Select(cnd => string.Format("({0})", cnd.GetQueryPart())));
        }
    }
}

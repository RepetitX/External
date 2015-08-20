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
        public ConditionGroupType Type { get; set; }

        protected List<IQueryCondition> conditions;
        protected QueryFieldsContainer fieldsContainer;

        public bool IsEmpty
        {
            get { return conditions.All(cnd => cnd.IsEmpty); }
        }

        public ConditionGroup(ConditionGroupType Type, QueryFieldsContainer FieldsContainer)
        {
            this.Type = Type;
            conditions = new List<IQueryCondition>();
            fieldsContainer = FieldsContainer;
        }

        public void AddCondition(IQueryCondition Condition)
        {
            conditions.Add(Condition);
        }

        public void AddCondition<T>(string FieldName, CompareCondition Comparsion, T? Value) where T : struct
        {
            QueryField<T> field = (QueryField<T>) fieldsContainer.GetQueryField<T>(FieldName);
            conditions.Add(new QueryCondition<T>(field, Comparsion, Value));
        }


        public string GetQueryPart()
        {
            return string.Join("\nand ",
                conditions.Where(cnd => !cnd.IsEmpty).Select(cnd => string.Format("{0}", cnd.GetQueryPart())));
        }
    }
}

namespace SQLQueryGenerator.QueryParameters
{
    public enum CompareConditions
    {
        Less,
        LessOrEqual,
        Equal,
        MoreOrEqual,
        More
    }
    public class QueryCondition<T> : BaseCondition
    {
        public QueryCondition(QueryField<T> Field)
        {

        }

        public string GetQueryPart()
        {
            throw new System.NotImplementedException();
        }
    }
}
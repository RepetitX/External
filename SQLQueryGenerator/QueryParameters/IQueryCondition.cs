
namespace SQLQueryGenerator.QueryParameters
{
    public interface IQueryCondition : IQueryPart
    {
        bool IsEmpty { get; }
    }
}

namespace SQLQueryGenerator.QueryParameters
{
    public interface IQueryField : IQueryPart
    {
        string Expression { get; set; }
        string Alias { get; set; }

        string GetTypeName();
    }
}
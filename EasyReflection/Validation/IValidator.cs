namespace EasyReflection.Validation
{
    public interface IValidator
    {
        bool Verbose { get; set; }
        bool Validate(object Object);
        string GetValidationResult(object Object);
    }
}
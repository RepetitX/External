namespace EasyReflection.Validation
{
    public interface IValidator
    {
        bool Validate(object Object);
        ValidationResult GetValidationResult(object Object);
    }
}
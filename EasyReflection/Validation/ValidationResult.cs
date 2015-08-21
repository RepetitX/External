using System.Collections.Generic;

namespace EasyReflection.Validation
{
    public class ValidationResult
    {
        public bool Success { get; internal set; }
        public List<ValidationError> Errors { get; internal set; }

        internal ValidationResult()
        {
            Errors = new List<ValidationError>();
            Success = true;
        }

        internal ValidationResult(bool Success, List<ValidationError> Errors)
        {
            this.Success = Success;
            this.Errors = Errors;
        }
    }
}
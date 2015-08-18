using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyReflection.Validation
{
    public abstract class BaseValidator : IValidator
    {
        public bool Verbose { get; set; }
        public List<string> ValidationErrors { get; private set; }

        protected BaseValidator()
        {
            ValidationErrors = new List<string>();
        }

        public abstract bool Validate(object Object);
        public abstract string GetValidationResult(object Object);
    }
}
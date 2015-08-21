using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyReflection.Validation
{
    public abstract class BaseValidator : IValidator
    {        
        public abstract bool Validate(object Object);
        public abstract ValidationResult GetValidationResult(object Object);

        public static bool ValidateMember(ValidationCondition Condition, object Object, string MemberName,
            out string validationMessage)
        {
            //Check conditions

            validationMessage = "";
            return true;
        }
    }
}
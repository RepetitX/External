using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyReflection.Attributes;

namespace EasyReflection.Validation
{
    public class CustomObjectValidator : BaseValidator
    {
        protected object validationObject;
        protected string memberValidationComments;

        public string ValidationMessage { get; set; }

        public CustomObjectValidator(object ValidationObject)
        {
            validationObject = ValidationObject;
            ValidationMessage = "Неправильно заполнены поля :";
        }

        public CustomObjectValidator(object ValidationObject, string ValidationMessage)
            : this(ValidationObject)
        {
            this.ValidationMessage = ValidationMessage;
        }

        public override bool Validate(object Object)
        {
            var props = validationObject.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (!CheckProperty(prop, Object))
                {
                    return false;
                }
            }
            return true;
        }

        public override string GetValidationResult(object Object)
        {
            var props = validationObject.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                CheckProperty(prop, Object);
            }
            if (ValidationErrors.Count > 0)
            {
                return string.Format("{0} {1}", ValidationMessage, string.Join(", ", ValidationErrors));
            }
            return "";
        }

        protected bool CheckProperty(PropertyInfo ValidationProperty, object Object)
        {
            object validationPropertyValue = ValidationProperty.GetValue(validationObject, null);
            if (validationPropertyValue == null)
            {
                return true;
            }

            var attrs = ValidationProperty.GetCustomAttributes(typeof(BaseValidationAttribute), false);
            if (attrs.Length == 0)
            {
                return true;
            }

            bool result = true;//По умолчанию валидация прошла

            foreach (BaseValidationAttribute attr in attrs)
            {
                if (attr is ComparsionValidationAttribute)
                {
                    result = CheckPropertyByComparsionAttribute(validationPropertyValue, (ComparsionValidationAttribute)attr, Object);
                }
                if (attr is ContainsValidationAttribute)
                {
                    result = CheckPropertyByContainsAttribute(validationPropertyValue, (ContainsValidationAttribute)attr, Object);
                }
                if (!result)
                {
                    
                    if (Verbose && !string.IsNullOrEmpty(memberValidationComments))
                    {
                        ValidationErrors.Add(string.Format("{0}({1})", attr.DisplayName, memberValidationComments));
                        memberValidationComments = "";
                    }
                    else
                    {
                        ValidationErrors.Add(attr.DisplayName);
                    }
                }
            }
            
            return result;
        }

        protected bool CheckPropertyByContainsAttribute(object ValidationPropertyValue,
            ContainsValidationAttribute Attribute,
            object Object)
        {
            //object objectPropertyValue = ReflectionHelper.GetPropertyValue(Attribute.TargetPropertyName, Object);

            IEnumerable objectPropertyValues = ReflectionHelper.GetAllMemberValues(Attribute.TargetMemberName,
                Object);
            var values = objectPropertyValues.Cast<object>().ToArray();

            if (ValidationPropertyValue is IEnumerable)
            {
                foreach (var val in ValidationPropertyValue as IEnumerable)
                {
                    if (!values.Contains(val))
                    {
                        //не содержит
                        memberValidationComments = string.Format("[{0}] не содержит [{1}]",
                            string.Join(", ", values), val);
                        return false;
                    }
                }
                return true;
            }
            if (values.Contains(ValidationPropertyValue))
            {
                return true;
            }
            memberValidationComments = string.Format("[{0}] не содержит {1}",
                string.Join(", ", values), ValidationPropertyValue);
            return false;
        }

        protected bool CheckPropertyByComparsionAttribute(object ValidationPropertyValue,
            ComparsionValidationAttribute Attribute,
            object Object)
        {
            object objectPropertyValue = ReflectionHelper.GetMemberValue(Attribute.TargetMemberName, Object);

            switch (Attribute.Type)
            {
                case ComparsionValidationType.Equal:
                case ComparsionValidationType.LessOrEqual:
                case ComparsionValidationType.Less:
                case ComparsionValidationType.MoreOrEqual:
                case ComparsionValidationType.More:
                case ComparsionValidationType.NotEqual:
                    if (objectPropertyValue is IComparable && ValidationPropertyValue is IComparable)
                    {
                        return GetComparsionResult((IComparable) objectPropertyValue,
                            (IComparable) ValidationPropertyValue,
                            Attribute.Type);
                    }
                    return true;
                case ComparsionValidationType.IsIn:
                    bool result;
                    if (ValidationPropertyValue is IEnumerable)
                    {
                        result = ((IEnumerable) ValidationPropertyValue).Cast<object>().Contains(objectPropertyValue);
                        if (!result)
                        {
                            memberValidationComments = string.Format("{0} не содержится в [{1}]", objectPropertyValue,
                                string.Join(", ", ((IEnumerable) ValidationPropertyValue).Cast<object>()));
                        }
                    }
                    else
                    {
                        result = objectPropertyValue.Equals(ValidationPropertyValue);
                        if (!result)
                        {
                            memberValidationComments = string.Format("{0} не равно {1}", objectPropertyValue,
                                ValidationPropertyValue);
                        }
                    }
                    return result;
            }
            return true;
        }

        protected bool GetComparsionResult(IComparable ObjectValue, IComparable FilterValue,
            ComparsionValidationType Type)
        {
            bool result;
            switch (Type)
            {
                case ComparsionValidationType.Equal:
                    result = ObjectValue.CompareTo(FilterValue) == 0;
                    break;
                case ComparsionValidationType.LessOrEqual:
                    result = ObjectValue.CompareTo(FilterValue) <= 0;
                    break;
                case ComparsionValidationType.Less:
                    result = ObjectValue.CompareTo(FilterValue) < 0;
                    break;
                case ComparsionValidationType.MoreOrEqual:
                    result = ObjectValue.CompareTo(FilterValue) >= 0;
                    break;
                case ComparsionValidationType.More:
                    result = ObjectValue.CompareTo(FilterValue) > 0;
                    break;
                case ComparsionValidationType.NotEqual:
                    result = ObjectValue.CompareTo(FilterValue) != 0;
                    break;
                default:
                    result = false;
                    break;
            }
            if (result)
            {
                return true;
            }
            memberValidationComments = string.Format("{0} не соответствует {1}", ObjectValue, FilterValue);
            return false;
        }
    }
}

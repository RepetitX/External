using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EasyReflection.Attributes;
using System.Collections;
using System.Linq;

namespace EasyReflection.Validation
{
    public class CustomObjectValidator : BaseValidator
    {
        protected object validationObject;

        public string ValidationMessage { get; set; }

        public CustomObjectValidator(object ValidationObject)
            : base()
        {
            validationObject = ValidationObject;
            this.ValidationMessage = "Неправильно заполнены поля :";
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
            else
            {
                return "";
            }
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
                    ValidationErrors.Add(attr.DisplayName);
                }
            }
            
            return result;
        }
         protected bool CheckPropertyByContainsAttribute(object ValidationPropertyValue, ContainsValidationAttribute Attribute,
            object Object)
        {
            //object objectPropertyValue = ReflectionHelper.GetPropertyValue(Attribute.TargetPropertyName, Object);

             IEnumerable objectPropertyValues = ReflectionHelper.GetAllPropertyValues(Attribute.TargetPropertyName,
                 Object);
             if (ValidationPropertyValue is IEnumerable)
             {
                 foreach (var val in ValidationPropertyValue as IEnumerable)
                 {
                     if (!objectPropertyValues.Cast<object>().Contains(val))
                     {
                         return false;
                     }                     
                 }
                 return true;
             }
             else
             {
                 return objectPropertyValues.Cast<object>().Contains(ValidationPropertyValue);
             }

             /*if(!(objectPropertyValue is IEnumerable))
             {
                 //Не содержит
                 return false;
             }
             foreach (object obj in (IEnumerable)objectPropertyValue)
             {
                 if(string.IsNullOrWhiteSpace(Attribute.InnerPropertyName))
                 {
                     //Проверяем сам объект
                     if(obj.Equals(ValidationPropertyValue))
                     {
                         return true;
                     }
                 }
                 else
                 {
                     //Проверяем его свойство
                     object innerPropertyValue = ReflectionHelper.GetPropertyValue(Attribute.InnerPropertyName, obj);
                     if (innerPropertyValue != null && innerPropertyValue.Equals(ValidationPropertyValue))
                     {
                         return true;
                     }
                 }
             }*/
        }

        protected bool CheckPropertyByComparsionAttribute(object ValidationPropertyValue, ComparsionValidationAttribute Attribute,
            object Object)
        {
            object objectPropertyValue = ReflectionHelper.GetPropertyValue(Attribute.TargetPropertyName, Object);

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
                        return GetComparsionResult((IComparable)objectPropertyValue, (IComparable)ValidationPropertyValue,
                            Attribute.Type);
                    }
                    return true;
            }
            return true;
        }

        protected bool GetComparsionResult(IComparable ObjectValue, IComparable FilterValue, ComparsionValidationType Type)
        {
            switch (Type)
            {
                case ComparsionValidationType.Equal:
                    return ObjectValue.CompareTo(FilterValue) != 0;
                case ComparsionValidationType.LessOrEqual:
                    return ObjectValue.CompareTo(FilterValue) <= 0;
                case ComparsionValidationType.Less:
                    return ObjectValue.CompareTo(FilterValue) < 0;
                case ComparsionValidationType.MoreOrEqual:
                    return ObjectValue.CompareTo(FilterValue) >= 0;
                case ComparsionValidationType.More:
                    return ObjectValue.CompareTo(FilterValue) > 0;
                case ComparsionValidationType.NotEqual:
                    return ObjectValue.CompareTo(FilterValue) != 0;
                default:
                    return false;
            }
        }
    }
}

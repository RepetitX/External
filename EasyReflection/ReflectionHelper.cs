using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyReflection
{
    public class ReflectionHelper
    {
        public static object GetPropertyValue(string PropertyName, Object Object)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                return null;
            }
            //Если свойства типа Object.Property1.Property2...
            string[] propertyNameParts = PropertyName.Split('.');
            string propName = propertyNameParts[0];

            //Проверяем, есть ли такое свойство
            PropertyInfo prop = Object.GetType().GetProperty(propName);
            if (prop == null)
            {
                return null;
            }
            object val = prop.GetValue(Object, null);
            if (val == null)
            {
                //Дальше искать нечего
                return null;
            }

            if (propertyNameParts.Length > 1)
            {
                //Убираем первое свойство из названия
                Regex regex = new Regex(String.Format(@"^{0}\.", propName));
                return GetPropertyValue(regex.Replace(PropertyName, ""), val);
            }
            return val;
        }
    }
}

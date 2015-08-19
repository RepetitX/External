using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EasyReflection
{
    public class ReflectionHelper
    {
        public static object GetPropertyValue(string PropertyName, object Object)
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

        public static IEnumerable GetAllPropertyValues(string PropertyName, object Object)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                yield return null;
                yield break;
            }
            object val = GetFirstPropertyValue(ref PropertyName, Object);

            if (val == null)
            {
                yield return null;
                yield break;
            }

            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                //Дошли до конца
                if (val is IEnumerable)
                {
                    foreach (var obj in val as IEnumerable)
                    {
                        yield return obj;
                    }
                }
                else
                {
                    yield return val;
                }
            }
            else
            {
                if (val is IEnumerable)
                {
                    foreach (var obj in val as IEnumerable)
                    {
                        IEnumerable values = GetAllPropertyValues(PropertyName, obj);
                        foreach (var val2 in values)
                        {
                            yield return val2;
                        }
                    }
                }
                else
                {
                    IEnumerable values = GetAllPropertyValues(PropertyName, val);
                    foreach (var val2 in values)
                    {
                        yield return val2;
                    }
                }
            }
        }

        protected static object GetFirstPropertyValue(ref string PropertyName, object Object)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                return null;
            }            
            string[] nameParts = PropertyName.Split('.');
            string name = nameParts[0];
            object result;

            //Обрабатываем массивы и коллекции
            //TODO Добавить поддержку нецелочисленных индексов
            Regex regex = new Regex(@"\[(?<index>\d*)\]");
            Match match = regex.Match(name);

            string index = null;
            
            if (match.Success)
            {
                //Это коллекция
                name = regex.Replace(name, "");
                index = match.Groups["index"].Value;
                
            }
            PropertyInfo property = Object.GetType().GetProperty(name);
            
            if (property == null)
            {
                //Дальше идти некуда
                PropertyName = "";
                return null;
            }

            if (string.IsNullOrWhiteSpace(index))
            {
                //Возвращаем объект или всю коллекцию
                result = property.GetValue(Object, null);
            }
            else
            {
                //возвращаем элемент
                result = property.GetValue(Object, new object[] {int.Parse(index)});
            }
            //Убираем использованную часть PropertyName

            PropertyName = string.Join(".",
                nameParts.Where((part, ind) => ind > 0));                

            return result;
        }
    }
}
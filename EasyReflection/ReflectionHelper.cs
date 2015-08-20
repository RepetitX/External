using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace EasyReflection
{
    public class ReflectionHelper
    {
        public static object GetMemberValue(string MemberName, object Object)
        {
            if (string.IsNullOrWhiteSpace(MemberName))
            {
                return null;
            }
            object val = GetFirstMemberValue(ref MemberName, Object);
            if (val == null)
            {
                //Дальше искать нечего
                return null;
            }

            if (!string.IsNullOrWhiteSpace(MemberName))
            {
                return GetMemberValue(MemberName, val);
            }
            return val;
        }

        public static IEnumerable GetAllMemberValues(string MemberName, object Object)
        {
            if (string.IsNullOrWhiteSpace(MemberName))
            {
                yield return null;
                yield break;
            }
            object val = GetFirstMemberValue(ref MemberName, Object);

            if (val == null)
            {
                yield return null;
                yield break;
            }

            if (string.IsNullOrWhiteSpace(MemberName))
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
                        IEnumerable values = GetAllMemberValues(MemberName, obj);
                        foreach (var val2 in values)
                        {
                            yield return val2;
                        }
                    }
                }
                else
                {
                    IEnumerable values = GetAllMemberValues(MemberName, val);
                    foreach (var val2 in values)
                    {
                        yield return val2;
                    }
                }
            }
        }

        protected static object GetFirstMemberValue(ref string MemberName, object Object)
        {
            if (string.IsNullOrWhiteSpace(MemberName))
            {
                return null;
            }
            string[] nameParts = MemberName.Split('.');
            string name = nameParts[0];
            object result;

            Regex regexMethod = new Regex(@"\(\)");
            Match matchMethod = regexMethod.Match(name);

            if (matchMethod.Success)
            {
                //Метод
                result = GetMethodValue(regexMethod.Replace(name, ""), Object);
            }
            else
            {
                result = GetPropertyValue(name, Object);
            }
            if (result == null)
            {
                //Дальше не двигаемся
                MemberName = "";
            }
            else
            {
                //Убираем использованную часть PropertyName
                MemberName = string.Join(".",
                    nameParts.Where((part, ind) => ind > 0));
            }
            return result;
        }

        protected static object GetPropertyValue(string Name, object Object)
        {
            //Обрабатываем массивы и коллекции
            //TODO Добавить поддержку нецелочисленных индексов
            Regex regexArray = new Regex(@"\[(?<index>\d*)\]");
            Match matchArray = regexArray.Match(Name);

            string index = null;

            if (matchArray.Success)
            {
                //Это коллекция
                Name = regexArray.Replace(Name, "");
                index = matchArray.Groups["index"].Value;
            }
            PropertyInfo property = Object.GetType().GetProperty(Name);

            if (property == null)
            {
                //Дальше идти некуда
                return null;
            }

            if (string.IsNullOrWhiteSpace(index))
            {
                //Возвращаем объект или всю коллекцию
                return property.GetValue(Object, null);
            }
            else
            {
                //возвращаем элемент
                return property.GetValue(Object, new object[] {int.Parse(index)});
            }
        }

        protected static object GetMethodValue(string Name, object Object)
        {
            //TODO добавить параметры
            MethodInfo method = Object.GetType().GetMethod(Name);

            if (method == null)
            {
                return null;
            }
            return method.Invoke(Object, null);
        }
    }
}
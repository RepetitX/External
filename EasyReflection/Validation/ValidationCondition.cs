using System.Text.RegularExpressions;
using EasyReflection.Attributes;

namespace EasyReflection.Validation
{
    public class ValidationCondition
    {
        public string MemberName { get; set; }
        public ComparsionValidationType Type { get; set; }
        public object Value { get; set; }

        public ValidationCondition(string MemberName, ComparsionValidationType Type)
        {
            this.MemberName = MemberName;
            this.Type = Type;
        }

        public ValidationCondition(string ConditionString, object ValidationObject)
        {
            Regex reCondition = new Regex("(?<memberName>)(?<condition>[<>=]=)(?<value>)");
        }

    }
}
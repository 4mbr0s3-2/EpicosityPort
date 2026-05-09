using System;
using System.Text.RegularExpressions;

namespace EpicosityPort.Systems.Utilities
{
    public class EnumHelper
    {
        public static string SplitCamelCase(string inputCamelCaseString)
        {
            string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
            return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
        // RS: ??? Oddly specific
        public static string GetEnumValueNameAndSplitCamelCase<T>(T enumValue)
        {
            return EnumHelper.SplitCamelCase(Enum.GetName(typeof(T), enumValue));
        }
    }
}

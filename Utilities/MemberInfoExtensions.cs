using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace Structura.SharedComponents.Utilities
{
    public static class MemberInfoExtensions
    {
        public static string GetDisplayNameOrName(this MemberInfo memberInfo)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute != null ? displayAttribute.Name : memberInfo.Name;
        }
    }
}

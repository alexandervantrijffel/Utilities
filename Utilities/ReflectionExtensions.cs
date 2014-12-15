using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structura.SharedComponents.Utilities
{
    public static class ReflectionExtensions
    {
        public static string GetDisplayNameOrName(this MemberInfo memberInfo)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute != null ? displayAttribute.Name : memberInfo.Name;
        }

        public static string GetValueEx(this PropertyInfo propertyInfo, object value)
        {
            var subPropertyValue = propertyInfo.GetValue(value, null);
            if (subPropertyValue == null)
            {
                return "NULL";
            }

            var nullableUnderlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
            if ((nullableUnderlyingType != null && nullableUnderlyingType.IsEnum) || propertyInfo.PropertyType.IsEnum)
            {
                var name = Enum.GetName(nullableUnderlyingType ?? propertyInfo.PropertyType, subPropertyValue);
                return !string.IsNullOrEmpty(name) ? name : subPropertyValue.ToString();
            }
            return subPropertyValue.ToString();
        }

        public static dynamic GetKey(object o)
        {
            foreach (PropertyInfo member in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (member.GetCustomAttributes(typeof(KeyAttribute), false).Any())
                {
                    return member.GetValue(o);
                }
            }

            throw new Exception("No key attribute found for object " + ObjectToString.DumpTypeAndFields(o));
        }

       
        public static string GetMemberName<T>(
            this T instance,
            Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(
            this T instance,
            Expression<Action<T>> expression)
        {
            return GetMemberName(expression);
        }

        public static string GetMemberName<T>(
            Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(
            UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }
    }
}

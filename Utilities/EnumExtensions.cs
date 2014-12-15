using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Structura.SharedComponents.Utilities
{
    public static class EnumExtensions
    {
        public static TEnumType ConvertToEnum<TEnumType>(this String enumValue)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), enumValue);
        }

        public static String ConvertToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }


        public static string GetDisplayName(this Enum e)
        {
            if (e == null)
            {
                return null;
            }
            return EnumHelper.GetDisplayName(e, false);
        }


        /// <summary>
        /// This function checks, if the enumeration contains the specific value as its member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int) (object) type & (int) (object) value) == (int) (object) value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function checks, if the enumeration is of the same type as the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int) (object) type == (int) (object) value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds flagged enum value to the enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T) (object) (((int) (object) type | (int) (object) value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Could not append value from enumerated type '{0}'.",
                                  typeof (T).Name), ex);
            }
        }

        /// <summary>
        /// Removes flagged enum value from the enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T) (object) (((int) (object) type & ~(int) (object) value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Could not remove value from enumerated type '{0}'.",
                                  typeof (T).Name), ex);
            }
        }

        /// <summary>
        /// Returns the value associated with the enum member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value">Enum member in string format</param>
        /// <returns></returns>
        /// <remarks>
        /// Suppress unused parameter becuase this is an extension method , so 'this' is required as parameter
        /// </remarks>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "type")]
        public static T GetEnum<T>(this Enum type, string value) where T : struct
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var outPutValue = (T) Enum.Parse(typeof (T), value, true);
            return outPutValue;
        }


        /// <summary>
        /// The function adds [optionally] disctinct item to the list.
        /// </summary>
        /// <typeparam name="T">type of item to be added in the list</typeparam>
        /// <param name="list">IList instance on which this extension method would be invoked</param>
        /// <param name="item">item to be added into the list</param>
        /// <param name="distinct">Whether to add distinct item</param>
        public static void Add<T>(this IList<T> list, T item, bool distinct)
        {
            if (list != null)
            {
                if (!distinct || !list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// This function removes specific item or all items from the list.
        /// </summary>
        /// <typeparam name="T">type of item to be removed in the list</typeparam>
        /// <param name="list">IList instance on which this extension method would be invoked</param>
        /// <param name="item">item to be removed into the list</param>
        /// <param name="all">Remove all items?</param>
        /// <returns></returns>
        public static bool Remove<T>(this IList<T> list, T item, bool all)
        {
            bool success = false;
            if (!all)
            {
                if (list != null)
                {
                    return list.Remove(item);
                }
            }

            if (list != null)
            {
                while (list.Contains(item))
                {
                    success &= list.Remove(item);
                }
            }

            return success;
        }
    }
}
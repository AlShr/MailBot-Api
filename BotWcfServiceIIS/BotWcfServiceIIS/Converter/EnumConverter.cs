using System;

namespace MailBot.Service.Converter
{
    public static class EnumConverter
    {
        /// <exception cref="ArgumentNullException"><paramref name="enumType" /> or <paramref name="value" /> is null. </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or-
        ///     <paramref name="value" /> is either an empty string ("") or only contains white space.-or-
        ///     <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     <paramref name="value" /> is outside the range of the underlying type of
        ///     <paramref name="enumType" />.
        /// </exception>
        public static TEnum ConvertEnum<TEnum>(this Enum source)
        {
            return (TEnum) Enum.Parse( typeof(TEnum), source.ToString(), true );
        }
    }
}
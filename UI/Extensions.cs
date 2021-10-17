using KP.Cookbook.UI.Utilities;
using System;
using System.Globalization;

namespace KP.Cookbook.UI
{
    public static class Extensions
    {
        private readonly static DecimalFormatProvider _formatProvider = new();
        private readonly static CultureInfo _russianCulture = CultureInfo.CreateSpecificCulture("ru-RU");

        public static string ToCustomString(this decimal value) =>
            string.Format(_formatProvider, "{0}", value);

        public static string ToRussianDateTimeString(this DateTime dateTime) =>
            dateTime.ToString("d", _russianCulture);
    }
}

using System;
using System.Linq;

namespace KitProjects.Cookbook.UI
{
    public class DecimalFormatProvider : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string numericString = arg.ToString();

            var splits = numericString.Split('.');
            if (splits[1].All(_char => _char == '0'))
                return splits[0];

            return numericString;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }
    }
}

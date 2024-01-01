using System;
using System.Globalization;

namespace Invest.Domain.Share
{
    public static class Helper
    {
        public static decimal ConvertDecimal(this string str)
        {
            return decimal.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static string ConvertString(this object obj)
        {
            return Convert.ToString(obj, CultureInfo.InvariantCulture);
        }

        public static string ConvertString(this decimal? dec)
        {
            if (dec.HasValue)
                return ConvertString(dec.Value);
            else
                return "0";
        }
        public static string ConvertString(this decimal dec)
        {
            return Convert.ToString(dec, CultureInfo.InvariantCulture);
        }
    }
}

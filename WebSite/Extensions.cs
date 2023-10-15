using System;
using System.Globalization;
using System.Linq;
using WebSite.DB;

namespace WebSite
{
    public static class Extensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string SubString(this string line, string from, string to)
        {
            var start = line.IndexOf(from, StringComparison.OrdinalIgnoreCase) + from.Length;
            var count = line.IndexOf(to, Math.Min(start, line.Length), StringComparison.OrdinalIgnoreCase) - start;

            return line.IndexOf(from, StringComparison.OrdinalIgnoreCase) >= 0 ? line.Substring(start, count) : string.Empty;
        }

        public static string FormatDate(this string date, string login)
        {
            try
            {
                var dateFormat = DB.Constants.Get("dateFormat").ElementAt(Settings.Get(login).DateFormat).Replace("m", "M");
                var dateTime = DateTime.ParseExact(date, DB.DB.DateFormats, null, DateTimeStyles.None);
                return dateTime.ToString(dateFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
                return date;
            }
        }

        public static int ToInt(this string value) => int.Parse(value);

        public static decimal ToDecimal(this string number) => decimal.Parse(number.Replace(',', '.'), CultureInfo.InvariantCulture);

        public static decimal ParseNumber(this string number, string login)
        {
            var dateFormat = DB.Constants.Get("numberFormat").ElementAt(Settings.Get(login).NumberFormat);
            var result = "0";

            switch (dateFormat)
            {
                case "123,456,789.00":
                    result = number.Replace(",", "").Replace('.', ',');
                    break;

                case "123.456.789,00":
                    result = number.Replace(".", "");
                    break;

                case "123 456 789.00":
                    result = number.Replace(" ", "").Replace('.', ',');
                    break;

                case "123 456 789,00":
                    result = number.Replace(" ", "");
                    break;
            }

            return result.ToDecimal();
        }

        public static string FormatNumber(this decimal number, string login)
        {
            var dateFormat = DB.Constants.Get("numberFormat").ElementAt(Settings.Get(login).NumberFormat);
            var result = Math.Round(number, 2, MidpointRounding.AwayFromZero)
                .ToString("N", CultureInfo.InvariantCulture);

            switch (dateFormat)
            {
                case "123.456.789,00":
                    return result.Replace(',', ' ').Replace('.', ',').Replace(' ', '.');

                case "123 456 789.00":
                    return result.Replace(',', ' ');

                case "123 456 789,00":
                    return result.Replace(',', ' ').Replace('.', ',');
            }

            return result;
        }
    }
}
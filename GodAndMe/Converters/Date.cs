using System;
using System.Globalization;
using Xamarin.Forms;
namespace GodAndMe.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return CommonFunctions.i18n("Today");

            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                if (dt.Date == DateTime.Today)
                {
                    return CommonFunctions.i18n("Today");
                }
                else if (dt.Date == DateTime.Today.AddDays(-2))
                {
                    return CommonFunctions.i18n("YesterdayDayBefore");
                }
                else if (dt.Date == DateTime.Today.AddDays(-1))
                {
                    return CommonFunctions.i18n("Yesterday");
                }
                else if (dt.Date == DateTime.Today.AddDays(1))
                {
                    return CommonFunctions.i18n("Tomorrow");
                }
                else if (dt.Date == DateTime.Today.AddDays(2))
                {
                    return CommonFunctions.i18n("TomorrowDayAfter");
                }
                else
                {
                    return dt.Date.ToString("D");
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

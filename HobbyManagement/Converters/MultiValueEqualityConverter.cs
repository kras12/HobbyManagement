using System.Globalization;
using System.Windows.Data;

namespace HobbyManagement.Converters
{
    public class MultiValueEqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
            {
                return true;
            }

            return values.All(x => x?.Equals(values[0]) ?? false) || values.All(x => x.Equals(null));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

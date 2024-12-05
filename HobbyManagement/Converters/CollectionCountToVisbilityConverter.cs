using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HobbyManagement.Converters;

/// <summary>
/// Converter that converts the count of a collection to visibility. 
/// </summary>
public class CollectionCountToVisbilityConverter : IValueConverter
{
    /// <inheritdoc/>        
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int count)
        { 
            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    /// <inheritdoc/>    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

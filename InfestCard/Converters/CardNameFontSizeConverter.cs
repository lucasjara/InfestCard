using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace InfestCard.Converters;

public class CardNameFontSizeConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var name = value?.ToString() ?? string.Empty;

        return name.Length switch
        {
            <= 15 => 16,
            <= 25 => 14,
            <= 35 => 12,
            _ => 10
        };
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
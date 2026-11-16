using System.Globalization;
using Microsoft.Maui.Controls;

namespace InfestCard.Converters;

public class StatusToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "scheduled" => "Programado",
                "cancelled" => "Cancelado",
                "completed" => "Finalizado",
                "in_progress" => "En curso",
                "upcoming" => "Próximo",
                "finished" => "Finalizado",
                _ => status
            };
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
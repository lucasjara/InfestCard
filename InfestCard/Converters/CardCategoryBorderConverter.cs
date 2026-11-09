using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace InfestCard.Converters
{
    internal class CardCategoryBorderConverter : IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            var category = value?.ToString()?.ToLowerInvariant();

            return category switch
            {
                "fire" => Colors.Red,
                "water" => Colors.DodgerBlue,
                "grass" => Colors.Green,
                "electric" => Colors.Gold,
                "ice" => Colors.LightSkyBlue,
                "fighting" => Colors.OrangeRed,
                "poison" => Colors.MediumPurple,
                "ground" => Colors.SaddleBrown,
                "flying" => Colors.SkyBlue,
                "psychic" => Colors.Purple,
                "bug" => Colors.OliveDrab,
                "rock" => Colors.Gray,
                "ghost" => Colors.DarkViolet,
                "dragon" => Colors.DarkOrange,
                "dark" => Colors.DarkSlateGray,
                "steel" => Colors.Silver,
                "fairy" => Colors.HotPink,
                "normal" => Colors.Bisque,

                // Categorías personalizadas de INFEST
                "nature" => Colors.LimeGreen,
                "magic" => Colors.MediumOrchid,

                _ => Colors.Cyan
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
}

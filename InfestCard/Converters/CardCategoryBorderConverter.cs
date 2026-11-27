using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace InfestCard.Converters
{
    public class CardCategoryBorderConverter : IValueConverter
    {
        private class CategoryStyle
        {
            public string Label { get; set; } = string.Empty;
            public Color Color { get; set; } = Colors.Gray;
        }

        private static CategoryStyle CreateStyle(
            string label,
            string color)
        {
            return new CategoryStyle
            {
                Label = label,
                Color = Color.FromArgb(color)
            };
        }

        // ==========================================
        // CATEGORÍAS PERSONALIZADAS INFEST
        // ==========================================

        private static readonly Dictionary<string, CategoryStyle>
            MagicCategories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["magic"] = CreateStyle("MAGIA", "#8b5cf6"),
                    ["fire"] = CreateStyle("FUEGO", "#ef4444"),
                    ["water"] = CreateStyle("AGUA", "#0ea5e9"),
                    ["nature"] = CreateStyle("NATURALEZA", "#22c55e"),
                    ["dark"] = CreateStyle("OSCURIDAD", "#64748b")
                };

        // ==========================================
        // CATEGORÍAS POKÉMON
        // ==========================================

        private static readonly Dictionary<string, CategoryStyle>
            PokemonCategories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["normal"] = CreateStyle("NORMAL", "#a8a29e"),
                    ["fire"] = CreateStyle("FUEGO", "#ef4444"),
                    ["water"] = CreateStyle("AGUA", "#0ea5e9"),
                    ["electric"] = CreateStyle("ELÉCTRICO", "#eab308"),
                    ["grass"] = CreateStyle("PLANTA", "#22c55e"),
                    ["ice"] = CreateStyle("HIELO", "#67e8f9"),
                    ["fighting"] = CreateStyle("LUCHA", "#dc2626"),
                    ["poison"] = CreateStyle("VENENO", "#a855f7"),
                    ["ground"] = CreateStyle("TIERRA", "#d97706"),
                    ["flying"] = CreateStyle("VOLADOR", "#818cf8"),
                    ["psychic"] = CreateStyle("PSÍQUICO", "#ec4899"),
                    ["bug"] = CreateStyle("BICHO", "#84cc16"),
                    ["rock"] = CreateStyle("ROCA", "#a16207"),
                    ["ghost"] = CreateStyle("FANTASMA", "#7c3aed"),
                    ["dragon"] = CreateStyle("DRAGÓN", "#4f46e5"),
                    ["steel"] = CreateStyle("ACERO", "#94a3b8"),
                    ["fairy"] = CreateStyle("HADA", "#f472b6")
                };

        // ==========================================
        // CATEGORÍAS STAR WARS
        // ==========================================

        private static readonly Dictionary<string, CategoryStyle>
            StarWarsCategories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["jedi"] = CreateStyle("JEDI", "#0ea5e9"),
                    ["sith"] = CreateStyle("SITH", "#ef4444"),
                    ["bounty_hunter"] = CreateStyle(
                        "CAZARECOMPENSAS", "#22c55e"),
                    ["imperial"] = CreateStyle("IMPERIAL", "#64748b"),
                    ["rebel"] = CreateStyle("REBELDE", "#f59e0b"),
                    ["droid"] = CreateStyle("DROIDE", "#eab308")
                };

        // ==========================================
        // CATEGORÍAS DRAGON BALL
        // ==========================================

        private static readonly Dictionary<string, CategoryStyle>
            DragonBallCategories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["saiyan"] = CreateStyle("SAIYAN", "#eab308"),
                    ["namekian"] = CreateStyle("NAMEKIANO", "#22c55e"),
                    ["human"] = CreateStyle("HUMANO", "#0ea5e9"),
                    ["majin"] = CreateStyle("MAJIN", "#f472b6"),
                    ["frieza_race"] = CreateStyle(
                        "RAZA DE FREEZER", "#a855f7"),
                    ["android"] = CreateStyle("ANDRÓIDE", "#64748b"),
                    ["god"] = CreateStyle("DIOS", "#4f46e5"),
                    ["angel"] = CreateStyle("ÁNGEL", "#67e8f9"),
                    ["evil"] = CreateStyle("MALIGNO", "#dc2626"),
                    ["nucleico"] = CreateStyle("NUCLÉICO", "#84cc16"),
                    ["other"] = CreateStyle("OTRO", "#94a3b8")
                };

        // ==========================================
        // CATEGORÍAS MINECRAFT
        // ==========================================

        private static readonly Dictionary<string, CategoryStyle>
            MinecraftCategories =
                new(StringComparer.OrdinalIgnoreCase)
                {
                    ["passive"] = CreateStyle("PASIVO", "#22c55e"),
                    ["neutral"] = CreateStyle("NEUTRAL", "#eab308"),
                    ["hostile"] = CreateStyle("HOSTIL", "#ef4444"),
                    ["boss"] = CreateStyle("JEFE", "#a855f7")
                };

        // ==========================================
        // CONVERTER: OBTENER COLOR AUTOMÁTICAMENTE
        // ==========================================

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null)
                return Colors.Cyan;

            var cardType = value.GetType();

            // Obtener tipo de carta
            var typeProperty = cardType.GetProperty("Type");

            string type = typeProperty?
                .GetValue(value)?
                .ToString()?
                .Trim()
                .ToLowerInvariant() ?? "custom";

            // Obtener categoría
            var categoryProperty = cardType.GetProperty("Category");

            string category = categoryProperty?
                .GetValue(value)?
                .ToString()?
                .Trim()
                .ToLowerInvariant() ?? string.Empty;

            var categories = GetCategories(type);

            if (categories.TryGetValue(category, out var style))
                return style.Color;

            return Colors.Cyan;
        }

        // ==========================================
        // OBTENER NOMBRE DE CATEGORÍA
        // ==========================================

        public static string GetCategoryLabel(
            string? category,
            string? cardType)
        {
            if (string.IsNullOrWhiteSpace(category))
                return "SIN CATEGORÍA";

            var categories = GetCategories(
                cardType?.Trim().ToLowerInvariant() ?? "custom");

            if (categories.TryGetValue(category.Trim(), out var style))
                return style.Label;

            return category.Trim().ToUpperInvariant();
        }

        // ==========================================
        // SELECCIONAR CONJUNTO DE CATEGORÍAS
        // ==========================================

        private static Dictionary<string, CategoryStyle>
            GetCategories(string cardType)
        {
            return cardType switch
            {
                "pokemon" => PokemonCategories,

                "starwars" or
                "star_wars" or
                "star-wars" => StarWarsCategories,

                "dragonball" or
                "dragon_ball" or
                "dragon-ball" => DragonBallCategories,

                "minecraft" => MinecraftCategories,

                _ => MagicCategories
            };
        }

        // ==========================================
        // CONVERT BACK
        // ==========================================

        public object? ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

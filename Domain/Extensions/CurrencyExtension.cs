using Domain.Enums;

namespace Domain.Extensions
{
    public static class CurrencyExtension
    {
        public static string GetSymbol(this CurrencyType currencyType)
        {
            return currencyType switch
            {
                CurrencyType.ARS => "$",
                CurrencyType.USD => "US$",
                CurrencyType.EUR => "€",
                CurrencyType.BRL => "R$",
                _ => string.Empty
            };
        }

        public static string GetCurrencyCode(this CurrencyType currencyType)
        {
            return currencyType switch
            {
                CurrencyType.ARS => "Peso Argentinos",
                CurrencyType.USD => "Dólar Estadounidense",
                CurrencyType.EUR => "Euro",
                CurrencyType.BRL => "Real Brasileño",
                _ => string.Empty
            };
        }
    }
}

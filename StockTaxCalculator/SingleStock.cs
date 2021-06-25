using System;

namespace StockTaxCalculator
{
    public class SingleStock
    {

        public SingleStock(Double priceBought, Double priceSold, String symbol)
        {
            PriceBuy = priceBought;
            PriceSell = priceSold;
            StockSymbol = symbol;
        }

        public Double PriceBuy { get; set; }

        public Double PriceSell { get; set; }

        public DateTime DateBuy { get; set; }

        public DateTime DateSell { get; set; }

        public String StockSymbol { get; set; }

    }
}

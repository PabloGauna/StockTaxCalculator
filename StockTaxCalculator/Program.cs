using System;
using System.Collections.Generic;

namespace StockTaxCalculator
{
    class SingleStock {
        public SingleStock(Double priceBought, Double priceSold)
        {
            priceBuy = priceBought;
            priceSell = priceSold;
        }

        public Double priceBuy { get; set; }

        public Double priceSell { get; set; }

        public DateTime dateBuy { get; set; }

        public DateTime dateSell { get; set; }
    }

    class Program
    {
        static List<Array> readCsv(string filePath)
        {
            var result = new List<Array>();

            var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath);
            parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            parser.SetDelimiters(new string[] { "," });

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();

                result.Add(row);
            }

            return result;
        }

        static Double CalculateGainTax(Double totalGain)
        {
            return totalGain * 0.15;
        }

        static void GenerateReport(List<SingleStock> soldStocks)
        {
            Console.WriteLine("Capital Gains Report: \n");

            Double totalGain = 0;

            foreach (SingleStock s in soldStocks)
            {
                Double stockGain = s.priceSell - s.priceBuy;
                Console.WriteLine("DateBuy: " + s.dateBuy + " , DateSell: " + s.dateSell + " , BuyPrice: " + s.priceBuy + " , SellPrice: " + s.priceSell + " , Gain: " + stockGain);
                totalGain += stockGain;
            }

            Console.WriteLine("Total Gain: " + totalGain);
            Console.WriteLine("Tax: " + CalculateGainTax(totalGain));
        }

        static void Main(string[] args)
        {
            var data = readCsv("operations.csv");

            Queue<SingleStock> stocks = new Queue<SingleStock>();
            List<SingleStock> soldStocks = new List<SingleStock>();

            foreach(Array operation in data)
            {
                var operationType = operation.GetValue(0).ToString();
                var qtty = Int32.Parse(operation.GetValue(1).ToString());
                var price = Double.Parse(operation.GetValue(2).ToString());

                if (operationType == "Buy")
                {
                    for (int index = 0; index < qtty; index++)
                    {
                        stocks.Enqueue(new SingleStock(price, 0));
                    }
                }
                else if (operationType == "Sell")
                {
                    for (int index = 0; index < qtty; index++)
                    {
                        var stock = stocks.Dequeue();
                        stock.priceSell = price;

                        soldStocks.Add(stock);
                    }
                }
            }

            GenerateReport(soldStocks);

            Console.WriteLine("Number of transactions: " + data.Count);
        }
    }
}
    
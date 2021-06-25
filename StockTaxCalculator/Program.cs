using System;
using System.Collections.Generic;
using System.Linq;

namespace StockTaxCalculator
{
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

        static void GenerateReport(List<SingleStock> soldStocks, String stocksSymbol)
        {
            Console.WriteLine("Capital Gains Report (" + stocksSymbol + "):\n");

            Double totalGain = 0;

            var groupedSoldStocks = soldStocks.GroupBy(s => s.PriceBuy + " - " + s.PriceSell);

            foreach(var group in groupedSoldStocks)
            {
                Double groupGain = 0;

                Console.WriteLine(group.Key + " (" + group.Count() + " Stocks)");
                foreach (SingleStock s in group.AsEnumerable())
                {
                    Double stockGain = s.PriceSell - s.PriceBuy;

                    if (stockGain > 0)
                    {
                        groupGain += stockGain;
                        totalGain += stockGain;
                    }
                }
                Console.WriteLine("Partial Gain: " + groupGain);
                Console.WriteLine(String.Empty);
            }

            Console.WriteLine("Total Gain: " + totalGain);
            Console.WriteLine("Tax: " + CalculateGainTax(totalGain));
        }

        static void Main(string[] args)
        {
            var stocksSymbol = "FB";
            var data = readCsv("OperacionesFinalizadas.csv");

            Queue<SingleStock> stocks = new Queue<SingleStock>();
            List<SingleStock> soldStocks = new List<SingleStock>();

            foreach (Array operation in data)
            {
                var operationType = operation.GetValue(0).ToString();
                var symbol = operation.GetValue(3).ToString();
                var qtty = Double.Parse(operation.GetValue(4).ToString()) / 10000;
                var price = Double.Parse(operation.GetValue(6).ToString()) / 100;

                if (symbol == stocksSymbol)
                {
                    if (operationType == "Compra")
                    {
                        for (int index = 0; index < qtty; index++)
                        {
                            stocks.Enqueue(new SingleStock(price, 0, symbol));
                        }
                    }
                    else if (operationType == "Venta")
                    {
                        for (int index = 0; index < qtty; index++)
                        {
                            var stock = stocks.Dequeue();
                            stock.PriceSell = price;

                            soldStocks.Add(stock);
                        }
                    }
                }
            }

            GenerateReport(soldStocks, stocksSymbol);

            Console.WriteLine("CSV Number of transactions: " + data.Count);
        }
    }
}
    
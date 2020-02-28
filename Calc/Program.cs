using System;
using System.Globalization;

namespace CalcTest
{
  class Program
  {
    static void Main(string[] args)
    {
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

      string line;
      Console.WriteLine("Enter expression to calculate it. Enter empty string to exit.");
      while (true)
      { 
        line = Console.ReadLine();
        if (line.Length == 0) break;
        try
        {
          Console.WriteLine($"Result: {Parser.Parse(line).Eval()}");
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }
  }
}
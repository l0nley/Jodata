using System;
using System.Data.Services;

namespace Jodata
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var serviceType = typeof(JiraAccessService);
      var baseAddress = new Uri("http://localhost:6000/");
      var baseAddresses = new[] { baseAddress };

      // Create a new hosting instance for the Northwind 
      // data service at the specified address. 
      var host = new DataServiceHost(
         serviceType,
         baseAddresses);
      host.Open();

      // Keep the data service host open while the console is open. 
      Console.WriteLine(
        "Navigate to the following URI to see the service.");
      Console.WriteLine(baseAddress);
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();

      // Close the host. 
      host.Close(); 
    }
  }
}

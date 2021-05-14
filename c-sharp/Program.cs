using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElectronCgi.DotNet;
using Microsoft.Extensions.Logging;


public record Message(string Text, string Date, int Count);


namespace ElectronCgi
{
  class Program
  {
    static Connection connection = new ConnectionBuilder()
                .WithLogging(minimumLogLevel: LogLevel.Trace, logFilePath: "cgi-log.txt")
                .Build();
    static int number = 1;
    static string timeString = DateTime.Now.ToUniversalTime().ToString();
    static string time = DateTime.UtcNow.ToFileTime().ToString();

    static void Main(string[] args)
    {


      connection.On<string, string>("greeting", (string name) =>
      {
        var message = new Message($"Hello {name}", timeString, number);

        var json = JsonSerializer.Serialize(message);

        return json;
      });

      new Timer(_ => connection.Send<string>("emis-ping", $"Emis CSharp says {time}"), null, 0, 3000);

      Test();
      connection.Listen();

    }

    public static async void Test()
    {

      await Task.Delay(1000);
    }
  }
}
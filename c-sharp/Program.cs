using System;
using System.Text.Json;
using System.Threading;
using ElectronCgi.DotNet;
using Microsoft.Extensions.Logging;


class Message
{
  public string text { get; set; }
  public string date { get; set; }
  public int count { get; set; }
}

namespace ElectronCgi
{
  class Program
  {

    static int number = 1;
    static string timeString = DateTime.Now.ToUniversalTime().ToString();
    static string time = DateTime.UtcNow.ToFileTime().ToString();

    static void Main(string[] args)
    {


      var connection = new ConnectionBuilder()
                .WithLogging(minimumLogLevel: LogLevel.Trace, logFilePath: "cgi-log.txt")
                .Build();

      connection.On<string, string>("greeting", (string name) =>
      {
        var message = new Message();
        message.text = $"Hello {name}!";
        message.date = timeString;
        message.count = number++;

        var json = JsonSerializer.Serialize(message);

        return json;
      });

      new Timer(_ => connection.Send("emis-ping", $"Emis CSharp says {time}"), null, 0, 3000);


      connection.Listen();

    }


  }
}

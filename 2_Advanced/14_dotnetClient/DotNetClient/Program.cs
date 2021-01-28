using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace DotNetClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Background Changer App!");

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/hub/background")
                .Build();

            connection.On<string>("changeBackground", (color) =>
            {
                switch (color.ToUpper())
                {
                    case "RED": Console.BackgroundColor = ConsoleColor.Red; break;
                    case "GREEN": Console.BackgroundColor = ConsoleColor.Green; break;
                    case "BLUE": Console.BackgroundColor = ConsoleColor.Blue; break;
                    default: Console.BackgroundColor = ConsoleColor.Black; break;
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Changed color to {color}");
            });

            await connection.StartAsync();

            Console.WriteLine("[R]ed | [G]reen | [B]lue] | E[x]it");

            var keepGoing = true;
            do
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.R:
                        await connection.SendAsync("ChangeBackground", "red");
                        break;
                    case ConsoleKey.G:
                        await connection.SendAsync("ChangeBackground", "green");
                        break;
                    case ConsoleKey.B:
                        await connection.SendAsync("ChangeBackground", "blue");
                        break;
                    case ConsoleKey.X:
                        keepGoing = false;
                        break;
                    default: Console.WriteLine("No..."); break;
                }
            } while (keepGoing);

            await connection.StopAsync();
        }
    }
}

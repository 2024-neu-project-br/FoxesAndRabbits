// See https://aka.ms/new-console-template for more information
using System.Net;
using FoxesAndRabbits.FAR.Display;

byte[] a = Array.Empty<byte>();
Console.WriteLine(a.Length);

Console.WriteLine("Hello, World!");

HTTPServer server = new HTTPServer(IPAddress.Loopback, 4060);
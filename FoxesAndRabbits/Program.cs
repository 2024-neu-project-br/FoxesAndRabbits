﻿using System.Net;
using FoxesAndRabbits.FAR.Display;

byte[] a = [];
Console.WriteLine(a.Length);

Console.WriteLine("Hello, World!");

HTTPServer server = new(IPAddress.Loopback, 4060);
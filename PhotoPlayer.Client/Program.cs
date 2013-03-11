﻿using System;
using System.IO;
using System.Linq;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace PhotoPlayer.Client
{
    static class Program
    {
        private static HubConnection _connection;
        private static IHubProxy _hubProxy;
        static void Main()
        {
            _connection = new HubConnection("http://localhost:64124/");
            _hubProxy = _connection.CreateHubProxy("PhotoPlayerHub");

            _connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("Failed to start: {0}", task.Exception.GetBaseException());
                    return;
                }

                Console.WriteLine("Connection Started");

            }).Wait();

            while (true)
                foreach (var imagePath in Directory.GetFiles(@"..\..\samples").OrderBy(x => x))
                {
                    FileStream fileStream = File.OpenRead(imagePath);
                    var memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);

                    _hubProxy.Invoke<string>("Send", memoryStream.ToArray()).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("Send failed {0}", task.Exception.GetBaseException());
                            return;
                        }

                        Console.WriteLine(imagePath);

                    }).Wait();
                    System.Threading.Thread.Sleep(45);
                }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZMQ;

namespace WeatherUpdateServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket publisher = context.Socket(SocketType.PUB))
                {
                    publisher.Bind("tcp://*:5556");

                    var rand = new Random();

                    while (true)
                    {
                        var id = rand.Next(0, 100).ToString();
                        var message = rand.Next(0, 100).ToString("000");

                        string format = string.Format("{0} {1}", id, message);
                        publisher.Send(format, Encoding.Unicode);
                        Console.WriteLine("Temp sent: {0}...", format);
                        //Thread.Sleep(new TimeSpan(0, 0, 30));
                    }
                }
            }
        }
    }
}

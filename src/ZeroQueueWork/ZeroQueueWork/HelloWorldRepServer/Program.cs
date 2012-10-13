using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace HelloWorldRepServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket receiver = context.Socket(SocketType.REP))
                {
                    receiver.Bind("tcp://*:5555");

                    while (true)
                    {
                        var message = receiver.Recv(Encoding.Unicode);
                        Console.WriteLine("Received request: {0}...", message);

                        string reply = "world";
                        receiver.Send(reply, Encoding.Unicode);
                    }
                }
            }
        }
    }
}

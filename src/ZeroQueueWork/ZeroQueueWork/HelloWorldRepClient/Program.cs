using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace HelloWorldRepClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket requester = context.Socket(SocketType.REQ))
                {
                    requester.Connect("tcp://localhost:5555");

                    const string requestMessage = "Hello";
                    const int requestsToSend = 10;

                    for (int requestNumber = 0; requestNumber < requestsToSend; requestNumber++)
                    {
                        Console.WriteLine("Sending Request {0}...", requestNumber);
                        requester.Send(requestMessage, Encoding.Unicode);

                        string reply = requester.Recv(Encoding.Unicode);
                        Console.WriteLine("Received reply {0}: {1}", requestNumber, reply);
                    }
                }

            }
        }
    }
}

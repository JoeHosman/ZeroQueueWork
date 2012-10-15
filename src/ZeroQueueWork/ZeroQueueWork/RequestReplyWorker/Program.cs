using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace RequestReplyWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket socket = context.Socket(SocketType.REP))
                {
                    socket.Connect("tcp://localhost:5560");

                    while (true)
                    {
                        string message = socket.Recv(Encoding.Unicode);
                        Console.WriteLine("Received request: {0}", message);
                        socket.Send("World", Encoding.Unicode);
                    }
                }
            }
        }
    }
}

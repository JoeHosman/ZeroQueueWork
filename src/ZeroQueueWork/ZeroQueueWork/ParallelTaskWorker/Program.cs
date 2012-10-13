using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZMQ;

namespace ParallelTaskWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket
                        receiver = context.Socket(SocketType.PULL),
                        sender = context.Socket(SocketType.PUSH))
                {

                    receiver.Connect("tcp://localhost:5557");
                    sender.Connect("tcp://localhost:5558");

                    while (true)
                    {
                        string task = receiver.Recv(Encoding.Unicode);

                        Console.WriteLine("{0}.", task);

                        int sleepTime = Convert.ToInt32(task);
                        Thread.Sleep(sleepTime);

                        sender.Send("", Encoding.Unicode);
                    }

                }
            }
        }
    }
}

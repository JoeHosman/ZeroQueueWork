using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZMQ;

namespace MultithreadedService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket
                    clients = context.Socket(SocketType.ROUTER),
                    workers = context.Socket(SocketType.DEALER))
                {
                    clients.Bind("tcp://*:5555");
                    workers.Bind("inproc://workers");   // note! FYI, inproc requires that bind is performed before connect

                    var workerThreads = new Thread[100];
                    for (int threadId = 0; threadId < workerThreads.Length; threadId++)
                    {
                        workerThreads[threadId] = new Thread(WorkerRoutine);
                        workerThreads[threadId].Start(context);
                    }

                    // Connect work threads to client threads via a queue
                    // Devices will be depricated from 3.x
                    Socket.Device.Queue(clients, workers);
                }
            }
        }

        private static int workercount = 0;
        private static void WorkerRoutine(object context)
        {
            var index = workercount;
            workercount++;

            Socket receiver = ((Context)context).Socket(SocketType.REP);
            receiver.Connect("inproc://workers");
            Random rand = new Random(index);
            while (true)
            {
                string message = receiver.Recv(Encoding.Unicode);

                //Thread.Sleep(rand.Next(1, 10) * 10); // Simulate 'work'

                receiver.Send("World", Encoding.Unicode);
                Console.WriteLine("{1}\t{0} send reply", DateTime.UtcNow.ToLongTimeString(), index);
            }
        }
    }
}

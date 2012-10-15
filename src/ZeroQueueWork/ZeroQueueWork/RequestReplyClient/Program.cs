using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZMQ;

namespace RequestReplyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {

                Console.WriteLine("Start. {0}", DateTime.UtcNow.ToLongTimeString());
                var workerThreads = new Thread[100];
                for (int threadId = 0; threadId < workerThreads.Length; threadId++)
                {
                    workerThreads[threadId] = new Thread(WorkerRoutine);
                    workerThreads[threadId].Start(context);
                }

                while (true) { }

            }
        }

        private static int workerCount = 0;
        private static void WorkerRoutine(object context)
        {
            var index = workerCount;
            workerCount++;
            Random rand = new Random(index);
            const int requestsToSend = 100;
            for (int requestNumber = 0; requestNumber < requestsToSend; requestNumber++)
            {

                using (Socket socket = ((Context)context).Socket(SocketType.REQ))
                {
                    socket.Connect("tcp://localhost:5555");


                    socket.Send("Hello", Encoding.Unicode);
                    string message = socket.Recv(Encoding.Unicode);
                    //Console.WriteLine("{1} Received reply: {0}", DateTime.UtcNow.ToLongTimeString(), index.ToString("0000"));
                    //Thread.Sleep(rand.Next(1, 10) * 10);
                }
            }
            if (index == workerCount - 1)
                Console.WriteLine("Done. {0}", DateTime.UtcNow.ToLongTimeString());
        }
    }
}

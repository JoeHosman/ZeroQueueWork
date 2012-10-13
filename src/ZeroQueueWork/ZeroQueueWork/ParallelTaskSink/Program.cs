using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZMQ;

namespace ParallelTaskSink
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (Socket receiver = context.Socket(SocketType.PULL))
                {
                    receiver.Bind("tcp://*:5558");

                    receiver.Recv();

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    const int tasksToConfig = 100;
                    for (int taskNumber = 0; taskNumber < tasksToConfig; taskNumber++)
                    {
                        string message = receiver.Recv(Encoding.Unicode);
                        Console.WriteLine(taskNumber % 10 == 0 ? ":" : ".");
                    }

                    stopwatch.Stop();

                    Console.WriteLine("Total elapsed time: {0}msec", stopwatch.ElapsedMilliseconds);
                }
            }
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace WeatherUpdateClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            while (true )
            {

                int id = rand.Next(1, 100);

                using (var context = new Context(1))
                {
                    using (Socket subscriber = context.Socket(SocketType.SUB))
                    {
                        subscriber.Subscribe(id.ToString(), Encoding.Unicode);
                        subscriber.Connect("tcp://localhost:5556");

                        const int updatesToCollect = 100;

                        uint totalTemp = 0;

                        for (int updateNumber = 0; updateNumber < updatesToCollect; updateNumber++)
                        {
                            Console.Write(".");
                            string update = subscriber.Recv(Encoding.Unicode);
                            totalTemp += (uint)Convert.ToInt32(update.Split()[1]);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Avg temp for code [{0}] = {1}", id, totalTemp / updatesToCollect);
                    }
                } 
            }

        }
    }
}

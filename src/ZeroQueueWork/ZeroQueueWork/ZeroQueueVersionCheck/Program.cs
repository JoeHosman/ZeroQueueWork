using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace ZeroQueueVersionCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ZHelpers.Version());

            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }
    }
}

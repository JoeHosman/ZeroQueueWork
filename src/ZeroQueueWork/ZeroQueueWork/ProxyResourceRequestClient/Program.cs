using System;
using ProxyResourcesLibrary;
using ZMQ;

namespace ProxyResourceRequestClient
{
    class Program
    {
        private static bool _interrupted;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate { _interrupted = true; };

            while (!_interrupted)
            {
                using (var context = new Context(1))
                {
                    var proxyStoreFactory = new ProxyStoreZeroMQFactory(context);

                    var proxyResourceService = proxyStoreFactory.Build();

                    var getProxyResourceRequest = new GetProxyResourceRequest { TargetUrl = "http://www.ticketmaster.com/example" };

                    const int requestsToSend = 1000;

                    DateTime start = DateTime.UtcNow;
                    for (int requestNumber = 0; requestNumber < requestsToSend; requestNumber++)
                    {
                        var resource = proxyResourceService.GetProxyResource(getProxyResourceRequest);

                        ReleaseProxyResourceRequest releaseRequest = new ReleaseProxyResourceRequest() { ProxyResource = resource.ProxyResource };
                        proxyResourceService.ReleaseProxyResource(releaseRequest);


                    }
                    DateTime end = DateTime.UtcNow;

                    Console.WriteLine("{0} took {1}", requestsToSend, end.Subtract(start));

                }
            }
        }
    }

    internal class ProxyStoreZeroMQFactory
    {
        private readonly Context _context;

        public ProxyStoreZeroMQFactory(Context context)
        {
            _context = context;
        }

        public IProxyResourceService Build()
        {
            return new ZeroMQProxyResourceStore(_context);
        }
    }
}

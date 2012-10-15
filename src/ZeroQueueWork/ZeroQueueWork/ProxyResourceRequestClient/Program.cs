using System.Text;
using ProxyResourcesLibrary;
using ServiceStack.Text;
using ZMQ;

namespace ProxyResourceRequestClient
{
    class Program
    {
        static void Main(string[] args)
        {


            using (var context = new Context(1))
            {
                var proxyStoreFactory = new ProxyStoreZeroMQFactory(context);

                var proxyResourceService = proxyStoreFactory.Build();

                var getProxyResourceRequest = new GetProxyResourceRequest { TargetUrl = "http://www.ticketmaster.com/example" };

                const int requestsToSend = 10;

                for (int requestNumber = 0; requestNumber < requestsToSend; requestNumber++)
                {
                    proxyResourceService.GetProxyResource(getProxyResourceRequest);
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

    internal class ZeroMQProxyResourceStore : IProxyResourceService
    {
        const string proxyStoreAddress = "tcp://localhost:5555";
        private readonly Context _zMqContext;

        public ZeroMQProxyResourceStore(Context zMQContext)
        {
            _zMqContext = zMQContext;

        }

        public GetProxyResourceResponse GetProxyResource(GetProxyResourceRequest request)
        {
            var jsonRequest = JsonSerializer.SerializeToString(request);

            var response = new GetProxyResourceResponse(request);
            using (Socket proxyResourceService = _zMqContext.Socket(SocketType.REQ))
            {
                proxyResourceService.Connect(proxyStoreAddress);

                proxyResourceService.SendMore("GET_PROXY_RESOURCE", Encoding.Unicode);
                proxyResourceService.Send(jsonRequest, Encoding.Unicode);

                string jsonResponse = proxyResourceService.Recv(Encoding.Unicode);

                response = JsonSerializer.DeserializeFromString<GetProxyResourceResponse>(jsonResponse);
            }
            return response;
        }

        public ReleaseProxyResourceResponse ReleaseProxyResource(ReleaseProxyResourceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}

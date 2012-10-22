using System.Text;
using ProxyResourcesLibrary;
using ServiceStack.Text;
using ZMQ;

namespace ProxyResourceRequestClient
{
    internal class ZeroMQProxyResourceStore : IProxyResourceService
    {
        const string ProxyStoreGetResourceAddress = "tcp://localhost:5555";
        const string ProxyStoreReleaseResourceAddress = "tcp://localhost:5556";
        private readonly Context _zMqContext;

        public ZeroMQProxyResourceStore(Context zMqContext)
        {
            _zMqContext = zMqContext;

        }

        public GetProxyResourceResponse GetProxyResource(GetProxyResourceRequest request)
        {
            var jsonRequest = JsonSerializer.SerializeToString(request);

            var response = new GetProxyResourceResponse(request);
            using (Socket proxyResourceService = _zMqContext.Socket(SocketType.REQ))
            {
                proxyResourceService.Connect(ProxyStoreGetResourceAddress);

                //proxyResourceService.SendMore("GET_PROXY_RESOURCE", Encoding.Unicode);
                proxyResourceService.Send(jsonRequest, Encoding.Unicode);

                string jsonResponse = proxyResourceService.Recv(Encoding.Unicode);

                response = JsonSerializer.DeserializeFromString<GetProxyResourceResponse>(jsonResponse);
            }
            return response;
        }

        public ReleaseProxyResourceResponse ReleaseProxyResource(ReleaseProxyResourceRequest request)
        {
            var jsonRequest = JsonSerializer.SerializeToString(request);

            var response = new ReleaseProxyResourceResponse(request);
            using (Socket proxyResourceService = _zMqContext.Socket(SocketType.REQ))
            {
                proxyResourceService.Connect(ProxyStoreReleaseResourceAddress);

                //proxyResourceService.SendMore("GET_PROXY_RESOURCE", Encoding.Unicode);
                proxyResourceService.Send(jsonRequest, Encoding.Unicode);

                string jsonResponse = proxyResourceService.Recv(Encoding.Unicode);

                response = JsonSerializer.DeserializeFromString<ReleaseProxyResourceResponse>(jsonResponse);
            }
            return response;
        }
    }
}
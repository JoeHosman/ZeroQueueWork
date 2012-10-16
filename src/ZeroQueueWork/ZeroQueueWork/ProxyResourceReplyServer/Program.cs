using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxyResourcesLibrary;
using ServiceStack.Text;
using ZMQ;

namespace ProxyResourceReplyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string listeningAddress = "tcp://*:5555";
            using (var context = new Context(1))
            {
                using (Socket proxyRequester = context.Socket(SocketType.REP))
                {
                    string requestType = "GET_PROXY_RESOURCE";
                    proxyRequester.StringToIdentity(requestType, Encoding.Unicode);
                    proxyRequester.Bind(listeningAddress);


                    while (true)
                    {
                        var jsonRequest = proxyRequester.Recv(Encoding.Unicode);
                        Console.WriteLine("Received {0}.", requestType);

                        var request = JsonSerializer.DeserializeFromString<GetProxyResourceRequest>(jsonRequest);

                        var response = new GetProxyResourceResponse(request)
                                           {
                                               ProxyResource =
                                                   new ProxyResource()
                                                       {
                                                           ProxyAddress = "http://localhost:3128",
                                                           ProxyId = 10,
                                                           ResourceTypeId = 10
                                                       }
                                           };

                        var jsonResponse = JsonSerializer.SerializeToString(response);
                        proxyRequester.Send(jsonResponse, Encoding.Unicode);
                    }
                }
            }
        }
    }
}

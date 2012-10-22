using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ProxyResourcesLibrary;
using ServiceStack.Text;
using ZMQ;

namespace ProxyResourceReplyServer
{
    class Program
    {
        private static bool _interrupted;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate { _interrupted = true; };
            using (var context = new Context(1))
            {
                Thread getProxyResourceListnerThread = new Thread(ListenForGetProxyResourceRequests);
                getProxyResourceListnerThread.Start(context);

                Thread releaseProxyResourceListnerThread = new Thread(ListenForReleaseProxyResourceRequests);
                releaseProxyResourceListnerThread.Start(context);

                while (!_interrupted)
                {

                }
            }
        }

        private static void ListenForGetProxyResourceRequests(object obj)
        {
            var context = obj as Context;

            if (null != context)
                ListenForGetProxyResourceRequests(context);
        }

        public static void ListenForReleaseProxyResourceRequests(object obj)
        {
            var context = obj as Context;

            if (null != context)
                ListenForReleaseProxyResourceRequests(context);
        }

        public static void ListenForGetProxyResourceRequests(Context context)
        {
            const string listeningAddress = "tcp://*:5555";
            using (Socket proxyRequester = context.Socket(SocketType.REP))
            {
                //string requestType = "GET_PROXY_RESOURCE";
                //proxyRequester.StringToIdentity(requestType, Encoding.Unicode);
                proxyRequester.Bind(listeningAddress);


                while (!_interrupted)
                {
                    var jsonRequest = proxyRequester.Recv(Encoding.Unicode);
                    //jsonRequest = proxyRequester.Recv(Encoding.Unicode);
                    //Console.WriteLine("Received {0}.", jsonRequest);

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

        public static void ListenForReleaseProxyResourceRequests(Context context)
        {
            const string listeningAddress = "tcp://*:5556";
            using (Socket proxyReleaser = context.Socket(SocketType.REP))
            {
                //string requestType = "GET_PROXY_RESOURCE";
                //proxyRequester.StringToIdentity(requestType, Encoding.Unicode);
                proxyReleaser.Bind(listeningAddress);


                while (!_interrupted)
                {
                    var jsonRequest = proxyReleaser.Recv(Encoding.Unicode);
                    //jsonRequest = proxyRequester.Recv(Encoding.Unicode);
                    //Console.WriteLine("Releaseing {0}.", jsonRequest);

                    var request = JsonSerializer.DeserializeFromString<ReleaseProxyResourceRequest>(jsonRequest);

                    var response = new ReleaseProxyResourceResponse(request);

                    var jsonResponse = JsonSerializer.SerializeToString(response);
                    proxyReleaser.Send(jsonResponse, Encoding.Unicode);
                }
            }
        }
    }
}

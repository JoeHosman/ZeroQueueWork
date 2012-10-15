namespace ProxyResourcesLibrary
{
    public interface IProxyResourceService
    {
        GetProxyResourceResponse GetProxyResource(GetProxyResourceRequest request);
        ReleaseProxyResourceResponse ReleaseProxyResource(ReleaseProxyResourceRequest request);
    }

    public class ReleaseProxyResourceRequest
    {
        public ProxyResource ProxyResource { get; set; }
    }

    public class ReleaseProxyResourceResponse
    {
        public ReleaseProxyResourceRequest Request { get; set; }
    }

    public class GetProxyResourceRequest
    {
        public string TargetUrl { get; set; }
        public ulong ResourceTypeId { get; set; }
    }

    public class GetProxyResourceResponse
    {
        public GetProxyResourceResponse(GetProxyResourceRequest request)
        {
            Request = request;
        }

        public GetProxyResourceRequest Request { get; set; }
        public ProxyResource ProxyResource { get; set; }
    }

    public class ProxyResource
    {
        public ulong ProxyId { get; set; }
        public ulong ResourceTypeId { get; set; }
        public string ProxyAddress { get; set; }
    }
}

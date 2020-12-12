namespace SrvClient
{
    public class RpcRequest
    {
        public string Active { get;  set; }
        public int Id { get;  set; }
        public string SrvName { get;  set; }

        public string ReqJson { get; set; }

        public byte[] ReqBytes { get; set; }
    }
}
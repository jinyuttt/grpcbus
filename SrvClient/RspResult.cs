namespace SrvClient
{
    public class RspResult
    {
        public int Id { get;  set; }
        public int ReqId { get;  set; }
        public string RspJson { get;  set; }
        public byte[] RspBytes { get;  set; }
    }
}
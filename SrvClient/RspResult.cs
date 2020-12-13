namespace SrvClient
{
    /// <summary>
    /// 返回封装结构
    /// </summary>
    public class RspResult
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public int Id { get;  set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public int ReqId { get;  set; }

        /// <summary>
        /// 返回数据的JSON字符串
        /// </summary>
        public string RspJson { get;  set; }

        /// <summary>
        /// 返回数据的Byte[]
        /// </summary>
        public byte[] RspBytes { get;  set; }
    }
}
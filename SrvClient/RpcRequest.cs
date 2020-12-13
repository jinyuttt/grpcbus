namespace SrvClient
{

    /// <summary>
    /// 定义封装对象
    /// </summary>
    public class RpcRequest
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string Active { get;  set; }

        /// <summary>
        /// 编号ID
        /// </summary>
        public int Id { get;  set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string SrvName { get;  set; }

        /// <summary>
        /// 数据JSON
        /// </summary>
        public string ReqJson { get; set; }

        /// <summary>
        /// 数据Byte[]
        /// </summary>
        public byte[] ReqBytes { get; set; }
    }
}
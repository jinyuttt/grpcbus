namespace SrvClient
{

    /// <summary>
    /// 客户端处理双向批量接口
    /// </summary>
    /// <typeparam name="T">接收的数据类型</typeparam>
    public interface IChatSrv<T>
    {
        /// <summary>
        /// 传输
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        void Push<V>(V data);

        /// <summary>
        /// 是否有数据读取
        /// </summary>
        bool IsCan { get; }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// 关闭客户端
        /// </summary>
        void Close();
    }
}
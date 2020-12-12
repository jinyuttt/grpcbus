namespace SrvPush
{

     /// <summary>
     /// 客户端双向流
     /// </summary>
     /// <typeparam name="T"></typeparam>
    public interface IChatClient<T>
    {
        /// <summary>
        /// 传输客户端
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        void Push<V>(V data);

        /// <summary>
        /// 是否有数据取回
        /// </summary>
        bool IsCan { get; }

        /// <summary>
        /// 获取客户端数据
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// 关闭服务端通信
        /// </summary>
        void Close();
    }
}
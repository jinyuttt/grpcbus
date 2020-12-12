using System.Collections.Generic;

namespace SrvPush
{

    /// <summary>
    /// 服务端批量推送接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISrvPush<T>
    {
       /// <summary>
       /// 客户端发送的参数
       /// </summary>
        T RequestArg { get; set; }

        /// <summary>
        /// 推送接口
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        void Push<V>(V data);

        /// <summary>
        /// 推送接口
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lst"></param>
        void PushBatch<V>(List<V> lst);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}

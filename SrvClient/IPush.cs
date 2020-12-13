namespace SrvClient
{

    /// <summary>
    /// 客户端批量推送接口
    /// </summary>
    public interface IPush
    {
        /// <summary>
        /// 推送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
         void Push<T>(T obj);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}
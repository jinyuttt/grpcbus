namespace SrvClient
{

    /// <summary>
    /// 客户端抽取数据接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoll<T>
    {

        /// <summary>
        /// 有数据读取
        /// </summary>
         bool IsCan { get;  }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
         T Get();
    }
}
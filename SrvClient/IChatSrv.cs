namespace SrvClient
{
    public interface IChatSrv<T>
    {
        void Push<V>(V data);
        bool IsCan { get; }

        T Get();
    }
}
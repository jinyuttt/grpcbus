namespace SrvClient
{
    public interface IPush
    {
         void Push<T>(T obj);
        void Close();
    }
}
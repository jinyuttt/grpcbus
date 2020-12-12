namespace SrvClient
{
    public interface IPoll<T>
    {
         bool IsCan { get;  }
         T Get();
    }
}
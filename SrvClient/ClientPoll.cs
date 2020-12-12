using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SrvClient
{
    public class ClientPoll<T> : IPoll<T>
    {
        private readonly AsyncServerStreamingCall<BusReply> result;
        private readonly Channel channel;
        private readonly BlockingCollection<RspResult> block = null;
        public ClientPoll(AsyncServerStreamingCall<BusReply> result, Channel channel)
        {
            this.result = result;
            this.channel = channel;
            block = new BlockingCollection<RspResult>();
            Start();
        }
        private void Start()
        {
            Task.Run(async () =>
            {

                while (await result.ResponseStream.MoveNext())
                {
                    block.Add(result.ResponseStream.Current.ConvertResult());
                }
              //  block.CompleteAdding();
             // await  channel.ShutdownAsync();
            });
        }

        public bool IsCan
        {
            get { return !block.IsCompleted; }
        }

        public T Get()
        {

            foreach (var p in block.GetConsumingEnumerable())
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(p.RspJson);
            }
            return default(T);
        }

    }
}

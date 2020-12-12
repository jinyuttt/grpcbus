using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SrvClient
{
    internal class ClientChat<T> : IChatSrv<T>
    {
        private readonly AsyncDuplexStreamingCall<BusRequest, BusReply> result;
        private readonly Channel channel;
        private readonly BlockingCollection<BusRequest> blockPush = null;

        private readonly BlockingCollection<RspResult> blockPoll = null;
        public ClientChat(AsyncDuplexStreamingCall<BusRequest, BusReply> result, Channel channel)
        {
            this.result = result;
            this.channel = channel;
            Start();
        }
        private void Start()
        {
            Task.Run(async () =>
            {
                foreach (var p in blockPush.GetConsumingEnumerable())
                {
                    await result.RequestStream.WriteAsync(p);
                }
               
            });

            Task.Run(async () =>
            {
                while (await result.ResponseStream.MoveNext())
                {
                    blockPoll.Add(result.ResponseStream.Current.ConvertResult());
                }
                blockPoll.CompleteAdding();
            });
        }
        public void Close()
        {
            blockPush.CompleteAdding();
            channel.ShutdownAsync();
        }

        public void Push<V>(V data)
        {
            var p = new BusRequest();
            var json = System.Text.Json.JsonSerializer.Serialize<V>(data);
            p.ReqJson.Add(json);
            blockPush.Add(p);
        }
        public bool IsCan
        {
            get { return !blockPoll.IsCompleted; }
        }

        public T Get()
        {

            foreach (var r in blockPoll.GetConsumingEnumerable())
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(r.RspJson);
            }
            return default(T);
        }
    }
}
using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SrvClient
{

    /// <summary>
    /// 客户端处理双向批量传输
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ClientChat<T> : IChatSrv<T>
    {
        private readonly AsyncDuplexStreamingCall<BusRequest, BusReply>  streamingCall;
        private readonly Channel channel;

        
        private readonly BlockingCollection<BusRequest> blockPush = null;//发送队列

        private readonly BlockingCollection<RspResult> blockPoll = null;//接收队列
        public ClientChat(AsyncDuplexStreamingCall<BusRequest, BusReply> result, Channel channel)
        {
            this.streamingCall = result;
            this.channel = channel;
            blockPush = new BlockingCollection<BusRequest>();
            blockPoll = new BlockingCollection<RspResult>();
            Start();
        }
        private void Start()
        {
            Task.Run(async () =>
            {
                foreach (var p in blockPush.GetConsumingEnumerable())
                {
                    await streamingCall.RequestStream.WriteAsync(p);
                }
               
            });

            Task.Run(async () =>
            {
                while (await streamingCall.ResponseStream.MoveNext())
                {
                    blockPoll.Add(streamingCall.ResponseStream.Current.ConvertResult());
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
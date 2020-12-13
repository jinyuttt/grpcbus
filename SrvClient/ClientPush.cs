using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SrvClient
{

    /// <summary>
    /// 客户端推送数据
    /// </summary>
    internal class ClientPush : IPush
    {
        private readonly AsyncClientStreamingCall<BusRequest, BusReply>   asyncClient;
        private readonly Channel channel;
        private readonly BlockingCollection<BusRequest> block = null;
        public ClientPush(AsyncClientStreamingCall<BusRequest, BusReply>   streamingCall, Channel channel)
        {
            this.asyncClient = streamingCall;
            this.channel = channel;
            block = new BlockingCollection<BusRequest>();
            Start();
        }


        private void  Start()
        {
            Task.Run(async () =>
            {
                foreach (var p in block.GetConsumingEnumerable())
                {
                   await asyncClient.RequestStream.WriteAsync(p);
                }
                await  channel.ShutdownAsync();
            });
        }

        public void Push<T>(T data)
        {
            var p = new BusRequest();
            var json =System.Text.Json.JsonSerializer.Serialize<T>(data);
            p.ReqJson.Add(json);
           
            block.Add(p);
        }

       

        public void Close()
        {
            block.CompleteAdding();
           
        }
    }
}
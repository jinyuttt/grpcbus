using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SrvPush
{
    public class SrvChat<T> : IChatClient<T>
    {
        private readonly IAsyncStreamReader<BusRequest>  streamReader;
        readonly IServerStreamWriter<BusReply> serverStream;
        readonly ServerCallContext context = null;
      
        /// <summary>
        /// 需要向客户端推送的数据
        /// </summary>
        private readonly BlockingCollection<BusReply> blockPush = null;

        /// <summary>
        /// 需要从客户端取出的数据
        /// </summary>
        private readonly BlockingCollection<BusRequest> blockPoll = null;
        public SrvChat(IAsyncStreamReader<BusRequest> requestStream, IServerStreamWriter<BusReply> responseStream, ServerCallContext contextl)
        {
            this.streamReader = requestStream;
            this.serverStream = responseStream;
            this.context = contextl;
            blockPoll = new BlockingCollection<BusRequest>();
            blockPush = new BlockingCollection<BusReply>();
            Start();
        }

        /// <summary>
        /// 启动向客户端推送
        /// </summary>
        private void Start()
        {
            Task.Run(async () =>
            {
                foreach (var p in blockPush.GetConsumingEnumerable())
                {
                    await serverStream.WriteAsync(p);
                }
               
            });

            Task.Run(async () =>
            {
                while (await streamReader.MoveNext())
                {
                    blockPoll.Add(streamReader.Current);
                }
                blockPoll.CompleteAdding();
            });
        }
        public void Close()
        {
            blockPush.CompleteAdding();
           
        }

        public void Push<V>(V data)
        {
            var p = new BusReply();
            var json = System.Text.Json.JsonSerializer.Serialize<V>(data);
            p.RspJson.Add(json);
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
                return System.Text.Json.JsonSerializer.Deserialize<T>(r.ReqJson[0]);
            }
            return default(T);
        }
    }
}
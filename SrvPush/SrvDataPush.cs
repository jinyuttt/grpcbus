using Grpc.Core;
using gRpcBus;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SrvPush
{
    /// <summary>
    /// 向客户端推送的类
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SrvDataPush<R> : ISrvPush<R>
    {
        public R RequestArg { get ; set ; }

        readonly IServerStreamWriter<BusReply> serverStream=null;
        readonly ServerCallContext callContext = null;
        private readonly BlockingCollection<BusReply> block = null;
        int num = 0;

        public SrvDataPush(IServerStreamWriter<BusReply> responseStream, ServerCallContext context)
        {
            this.serverStream = responseStream;
            this.callContext = context;
            block = new BlockingCollection<BusReply>();
           // Start();
        }


        public Task Start()
        {
          return  Task.Run(async () =>
            {
                foreach (var p in block.GetConsumingEnumerable())
                {
                    await  serverStream.WriteAsync(p);
                }
            });
        }

        /// <summary>
        /// 有数据推送
        /// </summary>
        public bool IsCan
        {
            get { return !block.IsCompleted; }
        }

        public BusReply GetBusReply()
        {
            foreach (var p in block.GetConsumingEnumerable())
            {
                return p;
            }
            return null;
        }

        /// <summary>
        /// 推送数据
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="data"></param>
        public void Push<V>(V data)
        {
            BusReply busReply = new BusReply();
            var json= System.Text.Json.JsonSerializer.Serialize<V>(data);
            busReply.RspJson.Add(json);
            busReply.Id = Interlocked.Increment(ref num);
            block.Add(busReply);
        }

       /// <summary>
       /// 推送数据
       /// </summary>
       /// <typeparam name="V"></typeparam>
       /// <param name="lst"></param>
      public  void PushBatch<V>(List<V> lst)
        {
            foreach (var p in lst)
            {
                BusReply busReply = new BusReply();
                var json = System.Text.Json.JsonSerializer.Serialize<V>(p);
                busReply.RspJson.Add(json);
                busReply.Id = Interlocked.Increment(ref num);
                block.Add(busReply);
               
            }
        }


        public void Close()
        {
            block.CompleteAdding();
        }
    }
}

using Grpc.Core;
using gRpcBus;
using SrvPush;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RpcBusService
{
    public delegate BusReply RequestService(BusRequest request, ServerCallContext context);

   
    public delegate void PollService<T>(ISrvPush<T> push);

    public delegate void ChatService<T>(IChatClient<T> chatClient);

    public class RpcBusImpl<T> : gRpcBus.RpcBus.RpcBusBase
    {
        public RequestService requestService;
        public PollService<T> pollService;
        public ChatService<T> chatService;
        public override Task<BusReply> RequestSrv(BusRequest request, ServerCallContext context)
        {
           var result= Task.Run(() =>
            {
                return requestService(request, context);
            });
            return result;
        }

        public override Task<BusReply> PushSrv(IAsyncStreamReader<BusRequest> requestStream, ServerCallContext context)
        {
           
                while (requestStream.MoveNext().Result)
                {
                     RequestSrv(requestStream.Current, context);
                }
            
                Console.WriteLine("退出");
           

            
            return Task.FromResult(new BusReply());
        }

        public override Task PollSrv(BusRequest request, IServerStreamWriter<BusReply> responseStream, ServerCallContext context)
        {
           
            var push = new SrvDataPush<T>(responseStream, context);
            var result = Task.Run(async () =>
             {
                
                 pollService(push);
                 while (push.IsCan)
                 {
                     var obj = push.GetBusReply();

                     await responseStream.WriteAsync(obj);
                 }
                 
             });
            return result;
        }
        public override Task ChatSrv(IAsyncStreamReader<BusRequest> requestStream, IServerStreamWriter<BusReply> responseStream, ServerCallContext context)
        {
            var push = new SrvChat<T>(requestStream, responseStream, context);
           
            var result = Task.Run(async () =>
            {
                chatService(push);
                while (push.IsCan)
                {
                    var obj = push.GetBusReply();

                    await responseStream.WriteAsync(obj);
                }
            });
            
            return result;
        }
    }
}

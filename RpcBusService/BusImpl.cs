using Grpc.Core;
using gRpcBus;
using SrvPush;
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
            var result = Task.Run(async () =>
            {
                while (await requestStream.MoveNext())
                {
                    await RequestSrv(requestStream.Current, context);
                }
            });
            return Task.FromResult(new BusReply());
        }

        public override Task PollSrv(BusRequest request, IServerStreamWriter<BusReply> responseStream, ServerCallContext context)
        {
            var result = Task.Run(() =>
            {
                var push = new SrvDataPush<T>(responseStream, context);
                pollService(push);
            });
            return result;
        }
        public override Task ChatSrv(IAsyncStreamReader<BusRequest> requestStream, IServerStreamWriter<BusReply> responseStream, ServerCallContext context)
        {
            var result = Task.Run(() =>
            {
                var push = new SrvChat<T>(requestStream,responseStream, context);
                chatService(push);
            });
            return result;
        }
    }
}

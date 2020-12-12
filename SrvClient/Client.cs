using Grpc.Core;

namespace SrvClient
{
    public class Client
    {
        public string Address { get; set; }
        public RspResult RequetSrv(RpcRequest request)
        {
            Channel channel = new Channel(Address, ChannelCredentials.Insecure);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.RequestSrv(request.ConvertRequest());
             
            channel.ShutdownAsync().Wait();
            return result.ConvertResult();

        }
        public IPoll<T> PollRsv<T>(RpcRequest request)
        {
            Channel channel = new Channel(Address, ChannelCredentials.Insecure);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.PollSrv(request.ConvertRequest());

            return new ClientPoll<T>(result, channel);

        }

        public IPush PushRsv<T>(RpcRequest request)
        {
            Channel channel = new Channel(Address, ChannelCredentials.Insecure);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.PushSrv();

            return new ClientPush(result, channel);

        }

        public IChatSrv<T> ChatRsv<T>(RpcRequest request)
        {
            Channel channel = new Channel(Address, ChannelCredentials.Insecure);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.ChatSrv();

            return new ClientChat<T>(result, channel);

        }

    }
}

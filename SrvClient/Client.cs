using Grpc.Core;
using Grpc.Core.Interceptors;
using gRpcBus;
using System;
using System.Threading.Tasks;

namespace SrvClient
{

    /// <summary>
    /// 客户端连接
    /// </summary>
    public class Client
    {
        public const int MaxRecviceSize = 1024 * 1024 * 1024;
        public const int MaxSendSize = 1024 * 1024 * 1024;
        public string Address { get; set; }

        /// <summary>
        /// 请求服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RspResult RequetSrv(RpcRequest request)
        {
            ChannelOption[] channelOptions = new
                 ChannelOption[2] { new ChannelOption("MaxReceiveMessageSize",MaxRecviceSize) ,
                 new ChannelOption("MaxSendMessageSize",MaxSendSize) };
            Channel channel = new Channel(Address, ChannelCredentials.Insecure, channelOptions);
            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.RequestSrv(request.ConvertRequest());
             
            channel.ShutdownAsync().Wait();
            return result.ConvertResult();

        }

        /// <summary>
        /// 抽取服务端数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public IPoll<T> PollRsv<T>(RpcRequest request)
        {
            ChannelOption[] channelOptions = new
                ChannelOption[2] { new ChannelOption("MaxReceiveMessageSize",MaxRecviceSize) ,
                 new ChannelOption("MaxSendMessageSize",MaxSendSize) };
            Channel channel = new Channel(Address, ChannelCredentials.Insecure,channelOptions);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.PollSrv(request.ConvertRequest());

            return new ClientPoll<T>(result, channel);

        }

       /// <summary>
       /// 批量传输数据
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="request"></param>
       /// <returns></returns>
        public IPush PushRsv<T>(RpcRequest request)
        {
            ChannelOption[] channelOptions = new
                ChannelOption[2] { new ChannelOption("MaxReceiveMessageSize",MaxRecviceSize) ,
                 new ChannelOption("MaxSendMessageSize",MaxSendSize) };
            Channel channel = new Channel(Address, ChannelCredentials.Insecure,channelOptions);

            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var push = client.PushSrv();
            Task.Run(async () =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var p = new BusRequest();
                    var json = System.Text.Json.JsonSerializer.Serialize(DateTime.Now);
                    p.ReqJson.Add(json);
                   await push.RequestStream.WriteAsync(p);
                }
                await push.RequestStream.CompleteAsync();

            });

            return new ClientPush(push, channel);

        }


        /// <summary>
        /// 双向批量处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public IChatSrv<T> ChatRsv<T>(RpcRequest request)
        {
            ChannelOption[] channelOptions = new
                ChannelOption[2] { new ChannelOption("MaxReceiveMessageSize",MaxRecviceSize) ,
                 new ChannelOption("MaxSendMessageSize",MaxSendSize) };
            Channel channel = new Channel(Address, ChannelCredentials.Insecure,channelOptions);
            channel.Intercept(new ClientLoggerInterceptor());
            var client = new gRpcBus.RpcBus.RpcBusClient(channel);
            var result = client.ChatSrv();

            return new ClientChat<T>(result, channel);

        }

    }
}

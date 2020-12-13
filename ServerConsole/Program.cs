using Grpc.Core;
using Grpc.HealthCheck;
using Grpc.Core.Interceptors;
using gRpcBus;
using RpcBusService;
using SrvPush;
using System;
using System.Threading;
using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;

namespace ServerConsole
{
    class Program
    {
        public const int MaxRecviceSize = 1024 * 1024 * 1024;
        public const int MaxSendSize = 1024 * 1024 * 1024;
        static int num = 0;
        static void Main(string[] args)
        {
            var srv = new RpcBusImpl<string>();
            srv.requestService += Process;
            srv.chatService += ChatCom;
            srv.pollService += Poll;
            var serviceImpl = new HealthServiceImpl();
            //  server = new Server();
            // server.AddServiceDefinition(Grpc.Health.V1.Health.BindService(serviceImpl));
            ChannelOption[] channelOptions = new
                ChannelOption[2] { new ChannelOption("MaxReceiveMessageSize",MaxRecviceSize) ,
                 new ChannelOption("MaxSendMessageSize",MaxSendSize) };
            LoggerFactory loggerFactory = new LoggerFactory();
            Server server = new Server(channelOptions)
            {
                Services = { gRpcBus.RpcBus.BindService(srv).Intercept(new ServerLoggerInterceptor(loggerFactory.CreateLogger<ServerLoggerInterceptor>())) },
                Ports = { new ServerPort("127.0.0.1", 5501, ServerCredentials.Insecure) }
            };
            server.Services.Add(Grpc.Health.V1.Health.BindService(serviceImpl));
           
            server.Start();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
        static BusReply Process(BusRequest request, ServerCallContext context)
        {
            Console.WriteLine(request.ReqJson[0]);
            return new BusReply() {  Id=num++, Reqid=request.Id};
        }
        static void Poll(ISrvPush<string> chatClient)
        {
           

           
                for (int i = 0; i < 1000; i++)
                {
                    chatClient.Push(DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }

                chatClient.Close();
           
         
        }

        static void ChatCom(IChatClient<string> chatClient)
        {
             Thread rec = new Thread(() =>
              {
                  while(chatClient.IsCan)
                  {
                     var str= chatClient.Get();
                      Console.WriteLine(str);
                  }
              });
            rec.IsBackground = true;
            rec.Start();

            Thread sd = new Thread(() =>
            {
                for(int i=0;i<1000;i++)
                {
                    chatClient.Push(DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
                chatClient.Close();
            });
            sd.IsBackground = true;
            sd.Start();
        }
   
    }
}

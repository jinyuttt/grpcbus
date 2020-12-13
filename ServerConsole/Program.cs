using Grpc.Core;
using gRpcBus;
using RpcBusService;
using SrvPush;
using System;
using System.Threading;

namespace ServerConsole
{
    class Program
    {
        static int num = 0;
        static void Main(string[] args)
        {
            var srv = new RpcBusImpl<string>();
            srv.requestService += Process;
            srv.chatService += ChatCom;
            srv.pollService += Poll;
            Server server = new Server
            {
                Services = { gRpcBus.RpcBus.BindService(srv) },
                Ports = { new ServerPort("localhost", 5501, ServerCredentials.Insecure) }
            };
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

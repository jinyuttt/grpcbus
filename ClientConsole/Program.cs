using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SrvClient.Client client = new SrvClient.Client();
            client.Address = "127.0.0.1:5501";
            Poll(client);
            //Push(client);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
        private static void Poll(SrvClient.Client client)
        {
             Thread thread = new Thread(() =>
              {
                  Random random = new Random();
                  var poll = client.PollRsv<string>(new SrvClient.RpcRequest() { Id = random.Next(), Active="",  SrvName="" });
                  while(poll.IsCan)
                  {
                     var str= poll.Get();
                      Console.WriteLine(str);
                  }
              });
            thread.IsBackground = true;
            thread.Start();
        }
        private static void Push(SrvClient.Client client)
        {
            Thread thread = new Thread(() =>
            {
                Random random = new Random();
                var poll = client.PushRsv<string>(new SrvClient.RpcRequest() { Id = random.Next() });
                for (int i = 0; i < 1000; i++)
                {
                    poll.Push(DateTime.Today);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private static void Chat(SrvClient.Client client)
        {
            Thread thread = new Thread(() =>
            {
                Random random = new Random();
                var poll = client.ChatRsv<string>(new SrvClient.RpcRequest() { Id = random.Next() });
                Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        poll.Push(DateTime.Today);
                    }
                });

                Task.Run(() =>
                {
                    while(poll.IsCan)
                    {
                        var str = poll.Get();
                        Console.WriteLine(str);
                    }
                });

            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}

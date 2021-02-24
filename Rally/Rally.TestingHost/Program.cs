using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rally.Core;
using Rally.Core.Client;
using Rally.Core.Server;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Rally.TestingHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HostOptions>(option =>
                        {
                            option.ShutdownTimeout = System.TimeSpan.FromSeconds(20);
                        })
                        .AddRally(rallyBuilder =>
                        {
                            rallyBuilder.AddAssembly(typeof(ChatActor).Assembly);
                        })
                        .AddHostedService<ChatService>();
                })
                .Build();

            await host.RunAsync();
        }
    }

    public class ChatService : IHostedService
    {
        private readonly IActorFactory _actorFactory;

        public ChatService(IActorFactory actorFactory)
        {
            this._actorFactory = actorFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var actor = _actorFactory.GetActor<IChatActor>("a");

            for (int i = 0; i < 10; i++)
            {
                var msg = await actor.Hello("陈");
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 收到反馈：{msg}");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} byebye1");
            await Task.Delay(10 * 1000, cancellationToken);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} byebye2");
        }
    }

    public class ChatActor : Actor, IChatActor
    {
        public async Task<string> Hello(string msg)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 收到消息：{msg}");
            await Task.Delay(1000);
            throw new Exception("我出错啦！");
            return $"Hello, {msg}!";
        }
    }

    public interface IChatActor : IActor
    {
        Task<string> Hello(string msg);
    }
}

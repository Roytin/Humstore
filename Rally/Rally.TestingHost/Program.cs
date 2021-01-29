using Rally.Core;
using System;
using System.Threading.Tasks;

namespace Rally.TestingHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var actorFactory = new ActorFactory();
            actorFactory.AddAssembly(typeof(ChatActor).Assembly);
            var actor = actorFactory.GetCell<IChatActor>("a");

            for (int i = 0; i < 10; i++)
            {
                var msg = await actor.Hello("世界");
                Console.WriteLine(msg);
            }

            Console.ReadLine();
        }
    }

    public class ChatActor : Actor, IChatActor
    {
        public async Task<string> Hello(string msg)
        {
            await Task.Delay(1000);
            return $"Hello, {msg}!";
        }
    }

    public interface IChatActor : IActor
    {
        Task<string> Hello(string msg);
    }
}

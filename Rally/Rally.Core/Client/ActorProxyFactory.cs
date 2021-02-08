using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Castle.DynamicProxy;

namespace Rally.Core.Client
{
    public class ActorProxyFactory : IActorFactory
    {
        private readonly PostMan _postMan;

        public ActorProxyFactory(PostMan postMan)
        {
            this._postMan = postMan;
        }

        public TActorInterface GetActor<TActorInterface>(string actorId) where TActorInterface : class, IActor
        {
            ProxyGenerator proxyGenerator = new ProxyGenerator();
            var proxyClass = proxyGenerator.CreateInterfaceProxyWithoutTarget<TActorInterface>(new ActorProxy(actorId, _postMan));
            return proxyClass;
        }
    }

}

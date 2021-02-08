using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Castle.DynamicProxy;
using System.Collections.Concurrent;

namespace Rally.Core.Server
{
    public class ActorFactory
    {
        private Dictionary<string, ActorInfo> _interfaceToActorInfo = new Dictionary<string, ActorInfo>();

        private ConcurrentDictionary<string, Actor> _actors = new ConcurrentDictionary<string, Actor>();

        internal void RegisterActor(string interfaceName, ActorInfo actorInfo)
        {
            _interfaceToActorInfo.Add(interfaceName, actorInfo);
        }

        public TActorInterface GetActor<TActorInterface>(string actorId) where TActorInterface : class, IActor
        {
            var actor = this.GetActor(typeof(TActorInterface).FullName, actorId);
            return actor as TActorInterface;
        }

        internal Actor GetActor(string interfaceName, string actorId) 
        {
            if (_interfaceToActorInfo.TryGetValue(interfaceName, out var actorInfo))
            {
                //todo: 路由
                string actorKey = $"{interfaceName}:{actorId}";
                return _actors.GetOrAdd(actorKey, (key)=> {

                    var actor = Activator.CreateInstance(actorInfo.ActorType) as Actor;
                    actor.Active(actorId);
                    return actor;
                });
            }
            throw new NotImplementedException($"there is no actor implemented by {interfaceName}");
        }
    }


    public struct ActorInfo
    {
        public Type ActorType;
        public Type InterfaceType;
    }
}

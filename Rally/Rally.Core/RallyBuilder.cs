using Microsoft.Extensions.DependencyInjection;
using Rally.Core.Client;
using Rally.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rally.Core
{
    public interface IRallyBuilder
    {
        IRallyBuilder AddAssembly(Assembly assembly);
    }

    public class RallyBuilder : IRallyBuilder
    {
        private readonly IServiceCollection _services;
        private List<Assembly> assemblies = new List<Assembly>();
        private ActorFactory _actorFactory;

        internal RallyBuilder(IServiceCollection services)
        {
            this._services = services;
            _actorFactory = new ActorFactory();
        }

        internal IServiceCollection Build()
        {
            _services.AddSingleton<PostMan>();
            _services.AddSingleton<ActorFactory>(_actorFactory);
            _services.AddSingleton<IActorFactory, ActorProxyFactory>();
            return _services;
        }

        public IRallyBuilder AddAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsActor())
                {
                    var interfaces = type.GetInterfaces();
                    var actorInterfaces = interfaces.Where(x => x != typeof(IActor)).Where(x => x.GetInterface(nameof(IActor)) != null).ToArray();
                    foreach (var actorInterface in actorInterfaces)
                    {
                        var actorInfo = new ActorInfo
                        {
                            ActorType = type,
                            InterfaceType = actorInterface
                        };
                        _actorFactory.RegisterActor(actorInterface.FullName, actorInfo);
                        //var methods = type.GetMethods();
                    }
                }
            }
            return this;
        }
    }
}

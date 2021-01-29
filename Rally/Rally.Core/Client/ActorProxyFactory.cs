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
        //private List<Assembly> assemblies = new List<Assembly>();
        //private Dictionary<Type, Type> _interfaceToClass = new Dictionary<Type, Type>();

        //public void AddAssembly(Assembly assembly)
        //{
        //    //scan assemblies, find out all cells
        //    assemblies.Add(assembly);
        //    foreach(var type in assembly.GetTypes())
        //    {
        //        if(type.IsActor())
        //        {
        //            var interfaces = type.GetInterfaces();
        //            var cellInterfaces = interfaces.Where(x => x != typeof(IActor)).Where(x=> x.GetInterface(nameof(IActor)) != null).ToArray();
        //            foreach (var cellInterface in cellInterfaces) 
        //            {
        //                _interfaceToClass.Add(cellInterface, type);
        //            }
        //            var methods = type.GetMethods();
        //        }
        //    }
        //}

        public TActorInterface GetCell<TActorInterface>(string actorId) where TActorInterface : class, IActor
        {
            ProxyGenerator proxyGenerator = new ProxyGenerator();
            var proxyClass = proxyGenerator.CreateInterfaceProxyWithoutTarget<TActorInterface>(new ActorProxy(actorId));
            return proxyClass;
        }
    }

}

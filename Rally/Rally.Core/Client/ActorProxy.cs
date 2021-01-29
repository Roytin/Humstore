using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rally.Core.Client
{
    public class ActorProxy : IInterceptor
    {
        private readonly string _actorId;

        public ActorProxy(string actorId)
        {
            this._actorId = actorId;
        }


        public void Intercept(IInvocation invocation)
        {
            //Do before...
            try
            {
                var actor = Activator.CreateInstance(invocation.TargetType);
                invocation.Method.Name
                ////invocation.Proceed();
                //Console.WriteLine("你你你你要跳舞吗？");
                //invocation.ReturnValue = Task.FromResult("哦哦哦");

            }
            catch
            {
                //...                
            }
            //Do after...
        }
    }
}

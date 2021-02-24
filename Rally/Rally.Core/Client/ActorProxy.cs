using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rally.Core.Client
{
    public class ActorProxy : IInterceptor
    {
        private readonly string _actorId;
        private readonly PostMan _postMan;

        public ActorProxy(string actorId, PostMan postMan)
        {
            this._actorId = actorId;
            this._postMan = postMan;
        }


        public void Intercept(IInvocation invocation)
        {
            //Do before...
            try
            {
                var mail = new Mail
                {
                    ActorId = _actorId,
                    InterfaceName = invocation.Method.DeclaringType.FullName,
                    MethodName = invocation.Method.Name,
                    Parameters = invocation.Arguments,
                };
                if (invocation.Method.ReturnType == typeof(void))
                {
                    _postMan.PostMail(mail);
                }
                else
                {
                    var returnType = invocation.Method.ReturnType.GetProperty("Result").PropertyType;
                    var returnValue = _postMan.PostMailAsync(returnType, mail);
                    invocation.ReturnValue = returnValue;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                }
                //...                
            }
        }
    }
}

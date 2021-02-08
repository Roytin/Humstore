using Rally.Core.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rally.Core
{
    public class PostMan
    {
        private readonly ActorFactory _actorFactory;

        public PostMan(ActorFactory actorFactory)
        {
            this._actorFactory = actorFactory;
        }


        public void PostMail(Mail mail)
        {
            OnReceivedMail(mail);
        }

        public object PostMailAsync(Type returnType, Mail mail)
        {
            mail.Receipt = new TaskCompletionSource<string>();
            OnReceivedMail(mail);
            //var taskReturnType = typeof(Task<>).MakeGenericType(returnType);
            //var taskReturnValue = Activator.CreateInstance(taskReturnType);
            //taskReturnValue
            return mail.Receipt.Task;
        }

        internal void OnReceivedMail(Mail mail)
        {
            var actor = _actorFactory.GetActor(mail.InterfaceName, mail.MethodName);
            actor.ReceiveMail(mail);
        }
    }
}

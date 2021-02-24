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
            mail.Receipt = new TaskCompletionSource<object>();
            OnReceivedMail(mail);
            //var taskReturnType = typeof(TaskCompletionSource<>).MakeGenericType(returnType);
            //var taskReturnValue = Activator.CreateInstance(taskReturnType);

            var method = typeof(TaskCompletionSourceEx).GetMethod("GenericTaskResult", 1,
                             new Type[] {
                                 typeof(Task<object>)
                             });
            var actor = method.MakeGenericMethod(returnType);
            var task = actor.Invoke(null, new object[] { mail.Receipt.Task });
            return task;
        }

        internal void OnReceivedMail(Mail mail)
        {
            var actor = _actorFactory.GetActor(mail.InterfaceName, mail.MethodName);
            actor.ReceiveMail(mail);
        }
    }

    public class TaskCompletionSourceEx
    {
        public static Task<TResult> GenericTaskResult<TResult>(Task<object> originalTask)
        {
            //var task = new TaskCompletionSource<TResult>();
            //await originalTask.ContinueWith(act => task.SetResult((TResult)act.Result));
            //return await task.Task;

            return originalTask.ContinueWith<TResult>(act => (TResult)act.Result);

            //await originalTask;
            //var resultProperty = originalTask.GetType().GetProperty("Result");
            //if (resultProperty == null)
            //{
            //    return default;
            //}
            //else
            //{
            //    var result = resultProperty.GetValue(originalTask, null);
            //    if (result == default)
            //        return default;
            //    return (TResult)result;
            //}
        }
    }
}

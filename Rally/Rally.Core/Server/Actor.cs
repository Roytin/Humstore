using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rally.Core.Server
{
    public abstract class Actor
    {
        protected string Id { get; private set; }

        private BlockingCollection<Mail> _mailbox = new BlockingCollection<Mail>(new ConcurrentQueue<Mail>());


        public async Task<object> ReceiveMail(Mail mail)
        {
            mail.Receipt = new TaskCompletionSource<object>();
            _mailbox.Add(mail);
            var result = await mail.Receipt.Task;
            return result;
        }

        internal void Active(string id)
        {
            Id = id;
            //todo: 读档

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var mail = _mailbox.Take();
                    var methodInfo = this.GetType().GetMethod(mail.MethodName, BindingFlags.Public|BindingFlags.Instance);
                    //find method
                    if(methodInfo != null)
                    {
                        //execute method
                        var result = methodInfo.Invoke(this, mail.Parameters);
                        //return result
                        mail.Receipt.SetResult(result);
                    }
                    else
                    {
                        mail.Receipt.SetException(new Exception($"actor [{this.GetType().FullName}] method [{mail.MethodName}] can't found!"));
                    }
                }
            }, 
            CancellationToken.None, 
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }
    }
}

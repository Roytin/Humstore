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


        public void ReceiveMail(Mail mail)
        {
            _mailbox.Add(mail);
        }

        internal void Active(string id)
        {
            Id = id;
            //todo: 读档

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var mail = _mailbox.Take();
                    try
                    {
                        var methodInfo = this.GetType().GetMethod(mail.MethodName, BindingFlags.Public | BindingFlags.Instance);
                        //find method
                        if (methodInfo != null)
                        {
                            //execute method
                            var task = methodInfo.Invoke(this, mail.Parameters) as Task;
                            await task;
                            if (mail.Receipt != null)
                            {
                                var resultProperty = methodInfo.ReturnType.GetProperty("Result");
                                if (resultProperty == null)
                                {
                                    //mail.Receipt.SetResult(task);
                                }
                                else
                                {
                                    var result = (string)resultProperty.GetValue(task, null);
                                    mail.Receipt.SetResult(result);
                                }
                            }
                        }
                        else
                        {
                            mail.Receipt?.SetException(new Exception($"actor [{this.GetType().FullName}] method [{mail.MethodName}] can't found!"));
                        }
                    }
                    catch(Exception ex)
                    {
                        mail.Receipt?.SetException(ex);
                    }
                }
            }, 
            CancellationToken.None, 
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }
    }
}

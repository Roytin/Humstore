using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rally.Core
{
    public class Mail
    {
        /// <summary>
        /// 邮件内容
        /// </summary>
        public object[] Parameters { get; set; }

        public string ActorId { get; set; }

        public string InterfaceName { get; set; }

        public string MethodName { get; set; }

        public TaskCompletionSource<object> Receipt { get; set; }
    }

    public class Request
    {
        public object[] Parameters { get; set; }

        public string ActorId { get; set; }

        public string InterfaceName { get; set; }

        public string MethodName { get; set; }
        public DateTime SendTime { get; set; }
    }

    public class Response
    {
        public object Result { get; set; }
    }
}

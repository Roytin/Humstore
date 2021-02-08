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

        public TaskCompletionSource<string> Receipt { get; set; }
    }
}

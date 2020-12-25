using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntX.WeChat.Dtos
{
    public class WeChatTokenRequest
    {
        public string Signature { get; set; }
        public string EchoStr { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }
    }
}

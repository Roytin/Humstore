using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntX.WeChat.Dtos
{
    public class WeChatContentResponse
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public long CreateTime { get; set; }
        public string Content { get; set; }
    }
}

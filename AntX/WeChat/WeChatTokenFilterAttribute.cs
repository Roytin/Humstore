using AntX.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntX.WeChat
{
    public class WeChatTokenFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string timestamp = context.HttpContext.Request.Query["timestamp"];
            string nonce = context.HttpContext.Request.Query["nonce"];
            string signature = context.HttpContext.Request.Query["signature"];

            List<string> ss = new List<string> { WeChatConstants.Token, timestamp, nonce };
            ss.Sort();
            var sign = string.Concat(ss).Sha1();
            if (sign != signature)
                context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }
    }
}

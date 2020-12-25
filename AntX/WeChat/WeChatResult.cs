using AntX.Controllers;
using AntX.WeChat.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AntX.WeChat
{
    public class WeChatTextResult : ActionResult, IStatusCodeActionResult, IActionResult
    {
        Tencent.WXBizMsgCrypt _wxcpt = new Tencent.WXBizMsgCrypt(WeChatConstants.Token, WeChatConstants.EncodingAESKey, WeChatConstants.AppID);
        
        private readonly WeChatContentResponse _resp;

        public WeChatTextResult(WeChatContentResponse resp)
        {
            this._resp = resp;
        }

        public int? StatusCode => 200;

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if(_resp.CreateTime == 0)
            {
                _resp.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            string sRespData = $"<xml><ToUserName><![CDATA[{_resp.ToUserName}]]></ToUserName><FromUserName><![CDATA[{_resp.FromUserName}]]></FromUserName><CreateTime>{_resp.CreateTime}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{_resp.Content}]]></Content></xml>";
            string sEncryptMsg = ""; //xml格式的密文
            var ret = _wxcpt.EncryptMsg(sRespData, _resp.CreateTime.ToString(), Guid.NewGuid().ToString(), ref sEncryptMsg);

            var reponse = context.HttpContext.Response;
            reponse.ContentType = "text/xml";
            reponse.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(sEncryptMsg));
            return base.ExecuteResultAsync(context);
        }
    }
}

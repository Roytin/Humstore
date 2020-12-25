using AntX.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AntX.WeChat
{
    public class WeChatContentModelBinder : IModelBinder
    {
        Tencent.WXBizMsgCrypt _wxcpt = new Tencent.WXBizMsgCrypt(WeChatConstants.Token, WeChatConstants.EncodingAESKey, WeChatConstants.AppID);

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var context = bindingContext.HttpContext;
            if (context.Request.ContentLength > 0)
            {
                string timestamp = context.Request.Query["timestamp"];
                string nonce = context.Request.Query["nonce"];
                string signature = context.Request.Query["signature"];
                string msgSignature = context.Request.Query["msg_signature"];

                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                // Leave the body open so the next middleware can read it.
                string sMsg = "";  //解析之后的明文
                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 10240,
                    leaveOpen: true))
                {
                    string bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    int ret = _wxcpt.DecryptMsg(msgSignature, timestamp, nonce, bodyStr, ref sMsg);
                    //<xml><ToUserName><![CDATA[gh_3a0c51b1684a]]></ToUserName>
                    //<FromUserName><![CDATA[oFtqWuCE9p-YhZC-NUCblRtW3_6I]]></FromUserName>
                    //<CreateTime>1608775479</CreateTime>
                    //<MsgType><![CDATA[text]]></MsgType>
                    //<Content><![CDATA[哈哈哈]]></Content>
                    //<MsgId>23032124066257641</MsgId>
                    //</xml>
                    if(ret < 0)
                    {
                        bindingContext.Result = ModelBindingResult.Failed();
                    }
                    else
                    {
                        var mySerializer = new XmlSerializer(bindingContext.ModelType);
                        using var myTextStream = new MemoryStream();
                        myTextStream.Write(Encoding.UTF8.GetBytes(sMsg));
                        myTextStream.Position = 0;
                        var model = mySerializer.Deserialize(myTextStream);
                        bindingContext.Result = ModelBindingResult.Success(model);
                    }
                }
            }
        }
    }
}

using AntX.Utils;
using AntX.WeChat;
using AntX.WeChat.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AntX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [WeChatTokenFilter]//验证在此
    public class WechatController : ControllerBase
    {
        private readonly ILogger<WechatController> _logger;

        public WechatController(ILogger<WechatController> logger)
        {
            this._logger = logger;
        }
        //?signature=a8a6172c44e03979101b5f2eceaeb1afdad14672&echostr=6380982091464760319&timestamp=1608700359&nonce=779057696
        //1）将token、timestamp、nonce三个参数进行字典序排序
        //2）将三个参数字符串拼接成一个字符串进行sha1加密
        //3）开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        [HttpGet]
        public IActionResult Get([FromQuery] WeChatTokenRequest req)
        {
            return Content(req.EchoStr);
        }

        [HttpPost]
        public IActionResult Post([FromQuery] WeChatTokenRequest req, WeChatContentRequest contentRequest)
        {
            return new WeChatTextResult(new WeChatContentResponse
            {
                Content = "你好",
                CreateTime = contentRequest.CreateTime,
                FromUserName = WeChatConstants.UserName,
                ToUserName = contentRequest.FromUserName
            });
        }
    }

}

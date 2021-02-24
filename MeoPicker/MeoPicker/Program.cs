using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;

namespace MeoPicker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var wcs = new WebClient[10];
            for(int i = 0; i< wcs.Length; i++)
            {
                wcs[i] = new WebClient();
            }

            for (int page = 1; page < 100; page++)
            {
                var url = $"http://moeimg.net/page/{page}";

                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(url);
                var picSetNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/div[@class='post']");

                int setNum = 1;
                foreach (var picSetNode in picSetNodes)
                {
                    var picSetA = picSetNode.SelectSingleNode("./div[@class='box list']/a");
                    if (picSetA != null)
                    {
                        var picSetUrl = picSetA.GetAttributeValue<string>("href", null);
                        var picSetTitle = picSetA.GetAttributeValue<string>("title", null);
                        if (!Directory.Exists(picSetTitle))
                        {
                            Directory.CreateDirectory(picSetTitle);
                        }
                        Console.WriteLine($"[{page}-{setNum++}][{picSetTitle}]{picSetUrl}");

                        HtmlWeb picWeb = new HtmlWeb();
                        var picHtmlDoc = web.Load(picSetUrl);

                        var picNodes = picHtmlDoc.DocumentNode.SelectNodes("/html/body/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/div[2]/div[@class='box']");
                        foreach(var picNode in picNodes)
                        {
                            var picA = picNode.SelectSingleNode("./a[@rel='noopener']");
                            if (picA != null)
                            {
                                var picUrl = picA.GetAttributeValue<string>("href", null);
                                var picNameIndex = picUrl.LastIndexOf('/');
                                var picName = picUrl.Substring(picNameIndex+1, picUrl.Length - picNameIndex - 1);
                                WebClient wc = new WebClient();
                                wc.DownloadFile(picUrl, $"{picSetTitle}/{picName}");
                            }
                        }

                    }
                }

                Console.WriteLine("一页扫描完毕,是否进入下一页?");
                Console.ReadLine();

            }

        }
    }
}

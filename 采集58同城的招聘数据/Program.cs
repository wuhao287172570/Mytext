using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions; //正则表达式的命名空间
using System.Threading;
using System.Threading.Tasks;

namespace 采集58同城的招聘数据
{
    class Program
    {
        static void Main(string[] args)
        {
            //控制台应用程序，调用异步方法
            Task task = Test1Async();
            task.Wait();
        }

        public async static Task Test1Async()
        {
            HttpClient http = new HttpClient();
            var html = await http.GetStringAsync("https://cs.58.com/tech/?&cmcskey=asp.net&final=1&jump=1&searchtype=3&sourcetype=5&key=asp.net");


          
            //去头
            int idx = html.IndexOf("id=\"list_con\"");
            html = html.Substring(idx);

         
            //去尾
            idx = html.IndexOf("class=\"pagesout\"");
            html = html.Substring(0, idx);
          
            //分割成一个一个的招聘信息
            string[] arr = Regex.Split(html, "</li>");

            //循环采集每一行数据
            for (int i = 0; i < arr.Length; i++)
            {
                Job job = new Job();
                //标题
                job.title = Regex.Match(arr[i], "<span class=\"name\">([^<]+)</span>").Groups[1].Value;
                //地址
                job.address = Regex.Match(arr[i], "<span class=\"address\">([^<]+)</span>").Groups[1].Value;
                //薪资待遇
                Match match = Regex.Match(arr[i], "<p class=\"job_salary\">([^<]+)<i class=\"unit\">([^<]*)</i></p>");
                job.salary = match.Groups[1].Value + " " + match.Groups[2].Value;
                //其它福利待遇
                job.others = new List<string>();
                match = Regex.Match(arr[i], "<div class=\"job_wel clearfix\">(?: *<span>[^>]+</span> *)*</div>");
                MatchCollection coll = Regex.Matches(arr[i], "<span>([^<]+)</span>");
                foreach (Match item in coll)
                    job.others.Add(item.Groups[1].Value);
                //获取详情URL
                job.detail_url = Regex.Match(arr[i], "<a href=\"([^\"]+)\" urlparams").Groups[1].Value;
                //获取详情页面的HTML代码
                html = await http.GetStringAsync(job.detail_url);
                match = Regex.Match(html, "<(?<HtmlTag>div) class=\"des\">((?<Nested><\\k<HtmlTag>[^>]*>)|</\\k<HtmlTag>>(?<-Nested>)|.*?)*</\\k<HtmlTag>>");
                job.description = match.Value;
                //输出
                Console.WriteLine(i + "." + job);

                //休眠10秒，再做下一次请求
                Thread.Sleep(10 * 1000);
            }

        }
    }
}

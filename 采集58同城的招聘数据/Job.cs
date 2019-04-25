using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 采集58同城的招聘数据
{
    public class Job
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 薪资
        /// </summary>
        public string salary { get; set; }
        /// <summary>
        /// 其它待遇
        /// </summary>
        public List<string> others { get; set; }

        /// <summary>
        /// 详情URL
        /// </summary>
        public string detail_url { get; set; }

        /// <summary>
        /// 岗位说明
        /// </summary>
        public string description { get; set; }

        public override string ToString()
        {
            //准备其它福利待遇的字符串
            string _others = "";
            if (others != null && others.Count > 0)
            {
                foreach (var item in others)
                {
                    _others += item + "；";
                }
            }
            //最终输出
            return string.Format(
                "==========================\r\n{0}|{1}\r\n {2}\r\n {3}\r\n {4}"
                , address, title, salary, _others, description);
        }
    }
}

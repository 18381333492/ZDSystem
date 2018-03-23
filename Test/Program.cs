using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayComon;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal s = decimal.Parse("9.8")*100;

            int t = Convert.ToInt32(s);// t.Parse(s.ToString());
            
        //    var t = PayRequest.Instance.bookOrder(new TenPayConfig()
        //    {
        //        appid = "wx094926e452737268",
        //        mch_id = "1448512202",
        //        key = "HJHSJHFSUOUSF564564HJJJHHJSDSDSD"
        //    }, "101.204.228.65", "ZD2017081819025323", "北京联通10元", decimal.Parse("9.8"), "http://101.204.228.65:8015/WxPayNotify.ashx", "http://101.204.228.65:8015/WxPayNotify.ashx",2);
        }
    }
}

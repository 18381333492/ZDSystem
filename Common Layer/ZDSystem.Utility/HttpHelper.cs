using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;

namespace ZDSystem.Utility
{
    public static class HttpHelper
    {
        /// <summary>
        /// 执行http Post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="referer">引用地址</param>
        /// <param name="paramses">参数</param>
        /// <returns></returns>
        public static string Post(string url, string referer, string paramses)
        {
            string html = "";
            try {
                CookieContainer _container = new CookieContainer();
                string strEncoding = Lib4Net.Framework.Settings.SettingHelper.Instance.GetData("Encoding");
                Encoding encoding = Encoding.GetEncoding(strEncoding);
                HttpWebRequest client = WebRequest.Create(url) as HttpWebRequest;
                client.CookieContainer = _container;
                byte[] postData = encoding.GetBytes(paramses);
                client.ContentLength = postData.Length;
                client.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                client.ContentType ="application/x-www-form-urlencoded";
                client.Referer = referer;
                client.Timeout = 0x186a0;
                using(Stream stream = client.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                    stream.Close();
                }
                using(HttpWebResponse response = client.GetResponse() as HttpWebResponse)
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream,encoding);
                    html = reader.ReadToEnd();
                    responseStream.Close();
                    reader.Close();
                }
            }
            catch
            {
                throw;
            }
            return html;
        }
        /// <summary>
        /// 执行Http Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string Get(string url, int timeout = 0x186a0)
        {
            string strResult="";
            try 
	        {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
                myReq.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(myStream, Encoding.Default);
                strResult = reader.ReadToEnd();
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
            return strResult;
        }
    }
}
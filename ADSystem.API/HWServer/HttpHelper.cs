using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using PayComon;

namespace ADSystem.API.HWServer
{
    /// <summary>
    /// http请求帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="sUrl">请求的链接</param>
        /// <param name="PostData">请求的参数</param>
        /// <returns></returns>
        public static string HttpPost(string sUrl, string PostData)
        {
            byte[] bPostData = System.Text.Encoding.UTF8.GetBytes(PostData);
            string sResult = string.Empty;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                if (bPostData != null)
                {
                    Stream postDataStream = webRequest.GetRequestStream();
                    postDataStream.Write(bPostData, 0, bPostData.Length);
                }
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else if (webResponse.ContentEncoding.ToLower() == "deflate")//如果使用了deflate则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream =
                            new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive))
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sResult;
        }


        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="sUrl">请求的链接</param>
        /// <returns></returns>
        public static string HttpGet(string sUrl)
        {
            string sResult = string.Empty;
            try
            {
                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "GET";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                string character = webResponse.CharacterSet;//获取响应的字符集;
                if (character == "ISO-8859-1") character = string.Empty;//iso-8859-1是西欧拉丁语，单字节编码，根本就不支持中文
                if (webResponse.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            if (string.IsNullOrEmpty(character))
                            {
                                using (StreamReader sr = new System.IO.StreamReader(zipStream))
                                {
                                    sResult = sr.ReadToEnd();
                                }
                            }
                            else
                            {
                                using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                                {
                                    sResult = sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                else if (webResponse.ContentEncoding.ToLower() == "deflate")//如果使用了deflate则先解压
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {
                        using (var zipStream = new System.IO.Compression.DeflateStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                        {
                            if (string.IsNullOrEmpty(character))
                            {
                                using (StreamReader sr = new System.IO.StreamReader(zipStream))
                                {
                                    sResult = sr.ReadToEnd();
                                }
                            }
                            else
                            {
                                using (StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding(webResponse.CharacterSet)))
                                {
                                    sResult = sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (System.IO.Stream streamReceive = webResponse.GetResponseStream())
                    {

                        if (string.IsNullOrEmpty(character))
                        {
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                        else
                        {
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding(webResponse.CharacterSet)))
                            {
                                sResult = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sResult;
        }


    }
}
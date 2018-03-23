﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Lib4Net.Logs;


namespace PayComon
{
    /// <summary>
    /// 微信支付帮助类
    /// </summary>
    public class TenPayHelp
    {
        private static readonly ILogger logger = LoggerManager.Instance.GetLogger("WxHttp");
      
        /// <summary>
        /// 从微信请求的参数中获取字典集合
        /// </summary>
        /// <param name="sXmlContent"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromXml(string sXmlContent)
        {
            if (!string.IsNullOrEmpty(sXmlContent))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXmlContent);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText);
                }
                return dic;
            }
            return null;
        }


        /// <summary>
        /// 将POST请求的返回的数据转化为字典集合
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromCDATAXml(string xmlData)
        {
            if (!string.IsNullOrEmpty(xmlData))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                //得到XML文档根节点
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (XmlNode node in nodeList)
                {
                    dic.Add(node.Name, node.InnerText.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty));
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 拼接的[CDATA]XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallCDATAXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + "><![CDATA[" + ht[key].ToString() + "]]></" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 拼接统一下单支付请求的XML数据
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string InstallXml(Dictionary<string, string> ht)
        {
            string xml = "<xml>";
            foreach (string key in ht.Keys)
            {
                xml += "<" + key + ">" + ht[key].ToString() + "</" + key + ">";
            }
            xml += "</xml>";
            return xml;
        }


        /// <summary>
        /// 根据节点名称读取节点的值
        /// </summary>
        /// <param name="sNodeName"></param>
        /// <returns></returns>
        public static string ReadXmlConfig(string sNodeName)
        {
            string sPayXmlPath = AppDomain.CurrentDomain.BaseDirectory + "PayConfig\\Pay.xml";
            string Value = string.Empty;
            XmlTextReader reader = new XmlTextReader(sPayXmlPath);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == sNodeName)
                    {
                        Value = reader.ReadElementString().Trim();//获取节点下面的值
                        break;
                    }
                }
            }
            reader.Close();
            return Value;
        }

        /// <summary>
        /// 对url进行urlencode处理
        /// </summary>
        /// <param name="sUrl">链接</param>
        /// <returns></returns>
        public static string UrlEncode(string sUrl)
        {
            if (!string.IsNullOrEmpty(sUrl))
            {
                sUrl = sUrl.Trim();
                sUrl = HttpUtility.UrlEncode(sUrl);
            }
            return sUrl;
        }

        /// <summary>
        /// Md5大写32加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5data.Length; i++)
            {
                sBuilder.Append(md5data[i].ToString("X2"));
                //X代表十六进制
                //2:代表每个数字2位
            }
            return sBuilder.ToString();
        }


        /// <summary>  
        /// AES解密(无向量)  
        /// </summary>  
        /// <param name="encryptedBytes">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns>明文</returns>  
        public static string AESDecrypt(String Data, String Key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(Data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );  
            //mStream.Seek( 0, SeekOrigin.Begin );  
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;  
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }  


        /// <summary>
        /// Post请求微信系统
        /// </summary>
        /// <param name="sUrl">请求的链接</param>
        /// <param name="PostData">请求的参数</param>
        /// <returns></returns>
        public static string HttpPost(string sUrl, string PostData, bool isUseCert=false,TenPayConfig config=null)
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

                if (isUseCert && config!=null)
                {//微信退款需要证书
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.SSLCERT_PATH);
                    X509Certificate2 cert = new X509Certificate2(path, config.mch_id);//证书的密码就是商户号
                    webRequest.ClientCertificates.Add(cert);
                }
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
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.UTF8))
                        {
                            sResult = sr.ReadToEnd();
                            logger.Info("请求链接:" + sUrl);
                            logger.Info("请求返回结果:" + sResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("请求异常:" + ex.Message);
                logger.Info(ex.Message,ex);
            }
            return sResult;
        }

    }
}

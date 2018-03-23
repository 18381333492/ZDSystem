using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace PayComon
{
    public class PingAnPayHelp
    {
        /// <summary>
        /// 组装data参数 及业务参数
        /// </summary>
        /// <param name="data">data json串字符串</param>
        /// <param name="key">open_key</param>
        /// <returns></returns>
        public static string MakeDataJson(string data, string key)
        {

            return PingAnEncrypt(data, key);
        }
        /// <summary>
        /// 组装data参数 及业务参数
        /// </summary>
        /// <param name="data">data json串字符串</param>
        /// <param name="key">open_key</param>
        /// <returns></returns>
        public static string GetDataJson(string data, string key)
        {

            return PingAnDecrypt(data, key);
        }

        /// <summary>
        /// 平安加密,先aes,再转16进制
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        static string PingAnEncrypt(String Data, String Key)
        {
            var aesDecrypt = Utilities.AESEncrypt(Data, Key);
            string decryptStr = Utilities.ByteToHexStr(aesDecrypt);
            return decryptStr;
        }

        //解析返回数据
        static string PingAnDecrypt(string data, string openKey)
        {
            byte[] orginBytes = Utilities.StrToToHexByte(data);
            string dataStr = Utilities.AesDecrypt(orginBytes, openKey);
            return dataStr;
        }


        //签名,并组装post 数据
        public static string SignAndPostData(string data, string openId, string openKey)
        {
            string timeStamp = Utilities.GetTimeStampTen().ToString();
            string origStr =
                string.Format("data={0}&open_id={1}&open_key={2}&timestamp={3}", data, openId, openKey, timeStamp);
            string sha1Str = Utilities.SHA1(origStr);
            string md5Str = Utilities.MD5(sha1Str.ToLower());
            string postData =
                string.Format("data={0}&open_id={1}&sign={2}&timestamp={3}", data, openId, md5Str, timeStamp);

            return postData;
        }

        //签名,并组装post 数据
        public static string RefundSignAndPostData(string data, string openId, string openKey)
        {

            long timeStamp = Utilities.GetTimeStampTen();
            string signType = "RSA";
            string origStr =
                string.Format("data={0}&open_id={1}&open_key={2}&sign_type={3}&timestamp={4}", data, openId, openKey, signType, timeStamp);
            //string sha1Str = Utilities.SHA1(origStr).ToLower();
            string priKey = ConfigurationManager.AppSettings["pa_pri_key"];
            string rsaStr = Utilities.RSAEncrypt(priKey, origStr);
            string postStr =
               string.Format("data={0}&open_id={1}&sign={2}&sign_type={3}&timestamp={4}", data, openId, rsaStr, signType, timeStamp);
            //JObject postData = new JObject();
            //postData.Add(new JProperty("data", data));
            //postData.Add(new JProperty("open_id", openId));
            //postData.Add(new JProperty("sign_type", signType));
            //postData.Add(new JProperty("sign", rsaStr));
            //postData.Add(new JProperty("timestamp", timeStamp));
            return postStr;
        }
        /// <summary>
        /// 返回结果封装成xml功机器人调用
        /// </summary>
        /// <param name="status"></param>
        /// <param name="refundId"></param>
        /// <param name="returnMsg"></param>
        /// <param name="errDesc"></param>
        /// <returns></returns>
        public static string MakeXmlRep(string returnCode, string resultCode, string refundId, string returnMsg, string errDesc)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("xml");

            XmlElement resultCodeE = doc.CreateElement("return_code");
            resultCodeE.InnerText = returnCode.Equals("0") ? "SUCCESS" : "Fail";
            root.AppendChild(resultCodeE);

            XmlElement result_code = doc.CreateElement("result_code");
            result_code.InnerText = resultCode.Equals("1") ? "SUCCESS" : "Fail";
            root.AppendChild(result_code);

            XmlElement return_msg = doc.CreateElement("return_msg");
            return_msg.InnerText = returnMsg;
            root.AppendChild(return_msg);

            XmlElement err_code_des = doc.CreateElement("err_code_des");
            err_code_des.InnerText = errDesc;
            root.AppendChild(err_code_des);

            XmlElement refund_id = doc.CreateElement("refundId");
            refund_id.InnerText = refundId;
            root.AppendChild(refund_id);

            doc.AppendChild(root);

            return ConvertXmlToString(doc);
        }
        /// <summary>       
        /// 
        /// 将XmlDocument转化为string 
        /// /// </summary>      
        /// /// <param name="xmlDoc"></param>  
        /// /// <returns></returns>   
        public static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }
        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errcode"></param>
        /// <param name="openKey"></param>
        /// <param name="msg"></param>
        /// <param name="getSignStr"></param>
        /// <returns></returns>
        public static bool CheckSign(string data, string errcode, string openKey, string msg, string getSignStr, string timeStamp)
        {
            string origStr =
                            string.Format("data={0}&errcode={1}&msg={2}&open_key={3}&timestamp={4}", data, errcode, msg, openKey, timeStamp);
            string sha1Str = Utilities.SHA1(origStr);
            string md5Str = Utilities.MD5(sha1Str.ToLower());
            if (!getSignStr.Equals(md5Str))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// post Json 数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonParas"></param>
        /// <returns></returns>
        public static string Post(string url, string jsonParas)
        {
            string strURL = url;

            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            //application/json;charset=UTF-8
            request.ContentType = "application/x-www-form-urlencoded";

            //设置参数，并进行URL编码 

            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流

            String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();


            //tream postData = Http.InputStream;
            StreamReader sRead = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string postContent = sRead.ReadToEnd();
            sRead.Close();


            return postContent;//返回Json数据
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;


    /// <summary>
    /// ** 描述：模拟HTTP POST GET请求并获取数据
    /// ** 创始时间：2019-1-2
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：
    /// </summary>
    public class WebRequestSugar 
    {
        /// <summary>
        /// 设置cookie
        /// </summary>
        private CookieContainer cookie;

        /// <summary>
        /// 是否允许重定向
        /// </summary>
        private bool allowAutoRedirect = true;

        /// <summary>
        /// contentType
        /// </summary>
        private string contentType = "application/json; charset=utf-8";

        /// <summary>
        /// accept
        /// </summary>
        private string accept = "*/*";

        /// <summary>
        /// 过期时间
        /// </summary>
        private int time = 50000000;

        /// <summary>
        /// 设置请求过期时间（单位：毫秒）（默认：5000）
        /// </summary>
        /// <param name="time"></param>
        public void SetTimeOut(int time)
        {
            this.time = time;
        }

        /// <summary>
        /// 设置accept(默认:*/*)
        /// </summary>
        /// <param name="accept"></param>
        public void SetAccept(string accept)
        {
            this.accept = accept;
        }

        /// <summary>
        /// 设置contentType(默认:application/x-www-form-urlencoded)
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            this.contentType = contentType;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(CookieContainer cookie)
        {
            this.cookie = cookie;
        }
        /// <summary>
        /// 是否允许重定向(默认:true)
        /// </summary>
        /// <param name="allowAutoRedirect"></param>
        public void SetIsAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
        }

        /// <summary>
        /// post请求返回
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpPost(string url, Dictionary<string, string> postdata, Dictionary<string, string> header = null)
        {
            DateTime dt = DateTime.Now;
            //string dicPath = "C:/HKPOCSLOG/DataLogDetails/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
            //Utils.WriteFile(dicPath + "/copy_" + "11第1" + ".txt", "", true);

            string postDataStr = null;
            if (postdata != null && postdata.Count > 0)
            {
                postDataStr = string.Join("&", postdata.Select(it => it.Key + "=" + it.Value).ToArray());
            }
            //Utils.WriteFile(dicPath + "/copy_" + "12第2" + ".txt", "", true);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = allowAutoRedirect;
            request.Method = "POST";
            request.Accept = accept;
            request.ContentType = this.contentType;
            request.Timeout = time;
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            if (header != null && header.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in header)
                {
                    request.Headers.Add(kvp.Key, kvp.Value);
                }
            }
            if (cookie != null)
                request.CookieContainer = cookie; //cookie信息由CookieContainer自行维护
            //Utils.WriteFile(dicPath + "/copy_" + "13第3" + ".txt", "", true);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            //Utils.WriteFile(dicPath + "/copy_" + "14第4" + ".txt", "", true);
            myStreamWriter.Close();
            HttpWebResponse response = null;
            //try
            //{
            this.SetCertificatePolicy();
            response = (HttpWebResponse)request.GetResponse();
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //获取重定向地址
            //string url1 = response.Headers["Location"];
            if (response != null)
            {
                //Utils.WriteFile(dicPath + "/copy_" + "15第5" + ".txt", "", true);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                //Utils.WriteFile(dicPath + "/copy_" + "16第6" + ".txt", "", true);
                myStreamReader.Close();
                myResponseStream.Close();
                //Utils.WriteFile(dicPath + "/copy_" + "17第7" + ".txt", "", true);
                return retString;
            }
            else
            {
                return null; //post请求返回为空
            }
        }

        public string HttpPost(string url, string postdata, Dictionary<string, string> header = null)
        {
            DateTime dt = DateTime.Now;
            //string dicPath = "C:/HKPOCSLOG/DataLogDetails/" + dt.Year + "/" + dt.Month + "/" + dt.Day;
            //Utils.WriteFile(dicPath + "/copy_" + "11第1" + ".txt", "", true);

            string postDataStr = postdata;

            //Utils.WriteFile(dicPath + "/copy_" + "12第2" + ".txt", "", true);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = allowAutoRedirect;
            request.Method = "POST";
            request.Accept = accept;
            request.ContentType = this.contentType;
            request.Timeout = time;
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            if (header != null && header.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in header)
                {
                    request.Headers.Add(kvp.Key, kvp.Value);
                }
            }
            if (cookie != null)
                request.CookieContainer = cookie; //cookie信息由CookieContainer自行维护
            //Utils.WriteFile(dicPath + "/copy_" + "13第3" + ".txt", "", true);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            //Utils.WriteFile(dicPath + "/copy_" + "14第4" + ".txt", "", true);
            myStreamWriter.Close();
            HttpWebResponse response = null;
            //try
            //{
            this.SetCertificatePolicy();
            response = (HttpWebResponse)request.GetResponse();
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //获取重定向地址
            //string url1 = response.Headers["Location"];
            if (response != null)
            {
                //Utils.WriteFile(dicPath + "/copy_" + "15第5" + ".txt", "", true);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                //Utils.WriteFile(dicPath + "/copy_" + "16第6" + ".txt", "", true);
                myStreamReader.Close();
                myResponseStream.Close();
                //Utils.WriteFile(dicPath + "/copy_" + "17第7" + ".txt", "", true);
                return retString;
            }
            else
            {
                return null; //post请求返回为空
            }
        }

        /// <summary>
        /// get请求获取返回的html
        /// </summary>
        /// <param name="url">无参URL</param>
        /// <param name="querydata">参数</param>
        /// <returns></returns>
        public string HttpGet(string url, Dictionary<string, string> querydata)
        {
            if (querydata != null && querydata.Count > 0)
            {
                url += "?" + string.Join("&", querydata.Select(it => it.Key + "=" + it.Value).ToArray());
            }

            return HttpGet(url);
        }
        /// <summary>
        /// get请求获取返回的html
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            request.Accept = this.accept;
            request.Timeout = time;
            this.SetCertificatePolicy();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file">文件路径</param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string file, Dictionary<string, string> postdata)
        {
            return HttpUploadFile(url, file, postdata, Encoding.UTF8);
        }
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file">文件路径</param>
        /// <param name="postdata">参数</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string file, Dictionary<string, string> postdata, Encoding encoding)
        {
            return HttpUploadFile(url, new string[] { file }, postdata, encoding);
        }
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files">文件路径</param>
        /// <param name="postdata">参数</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string[] files, Dictionary<string, string> postdata)
        {
            return HttpUploadFile(url, files, postdata, Encoding.UTF8);
        }
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files">文件路径</param>
        /// <param name="postdata">参数</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string[] files, Dictionary<string, string> postdata, Encoding encoding)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Accept = this.accept;
            request.Timeout = this.time;
            request.AllowAutoRedirect = this.allowAutoRedirect;
            if (cookie != null)
                request.CookieContainer = cookie;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (Stream stream = request.GetRequestStream())
            {
                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                if (postdata != null)
                {
                    foreach (string key in postdata.Keys)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, postdata[key]);
                        byte[] formitembytes = encoding.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    stream.Write(boundarybytes, 0, boundarybytes.Length);
                    string header = string.Format(headerTemplate, "file" + i, Path.GetFileName(files[i]));
                    byte[] headerbytes = encoding.GetBytes(header);
                    stream.Write(headerbytes, 0, headerbytes.Length);
                    using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }


        /// <summary>
        /// 获得响应中的图像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream GetResponseImage(string url)
        {
            Stream resst = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = true;
                req.Method = "GET";
                req.AllowAutoRedirect = allowAutoRedirect;
                req.CookieContainer = cookie;
                req.ContentType = this.contentType;
                req.Accept = this.accept;
                req.Timeout = time;
                Encoding myEncoding = Encoding.GetEncoding("UTF-8");
                this.SetCertificatePolicy();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                resst = res.GetResponseStream();
                return resst;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 正则获取匹配的第一个值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private string GetStringByRegex(string html, string pattern)
        {
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchs = re.Matches(html);
            if (matchs.Count > 0)
            {
                return matchs[0].Groups[1].Value;
            }
            else
                return "";
        }
        /// <summary>
        /// 正则验证返回的response是否正确
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private bool VerifyResponseHtml(string html, string pattern)
        {
            Regex re = new Regex(pattern);
            return re.IsMatch(html);
        }
        //注册证书验证回调事件，在请求之前注册
        private void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
        }
        /// <summary> 
        /// 远程证书验证，固定返回true
        /// </summary> 
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert,X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }

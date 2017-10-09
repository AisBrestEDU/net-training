/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�HTTPRequest.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��56��
 * * �ļ���ʶ��3CC6D7ED-740B-4A21-A5E6-1B6C7BF3A9F2
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace codest.Net.Web
{
    /// <summary>
    /// ����HTTP���󲢻����Ӧ�������
    /// </summary>
    public class HTTPRequest : BaseClass
    {
        #region ��Ա����
        /// <summary>
        /// ���浱ǰ���ӵ�HTTP Cookies
        /// </summary>
        protected CookieContainer currentCookies;
        private string domainURL;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ��վ��·�����磺http://www.easybroad.com�������Ҫ��/��
        /// </summary>
        public string DomainURL
        {
            get { return domainURL; }
            set { domainURL = value; }
        }
        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public HTTPRequest()
        {
            currentCookies = new CookieContainer();
        }
        /// <summary>
        /// ���죬��������վ��·��
        /// �磺http://www.easybroad.com
        /// �����Ҫ��/��
        /// </summary>
        /// <param name="domainURL">��վ��·��</param>
        public HTTPRequest(string domainURL)
        {
            currentCookies = new CookieContainer();
            this.domainURL = domainURL;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~HTTPRequest()
        {
            Dispose(false);
        }
        #endregion

        #region protected override void Dispose(bool disposing)
        /// <summary>
        /// �ͷ��ɵ�ǰ������Ƶ�������Դ
        /// </summary>
        /// <param name="disposing">��ʽ����</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //�ͷ��й���Դ
                currentCookies = null;
                domainURL = null;
            }
            //�ͷŷ��й���Դ
            base.Dispose(disposing);
        }
        #endregion

        #region protected string GetFullURL(string path)
        /// <summary>
        /// �������·����ȡ������URL
        /// </summary>
        /// <param name="path">���·�����硰/login.aspx��</param>
        /// <returns>������URL</returns>
        protected string GetFullURL(string path)
        {
            return DomainURL + path;
        }
        #endregion

        #region public void RefreshCookies()
        /// <summary>
        /// �Ͽ��Ự��ˢ�µ�ǰSession��Cookies
        /// </summary>
        public void RefreshCookies()
        {
            currentCookies = new CookieContainer();
        }
        #endregion

        #region public string GetData(string path)
        /// <summary>
        /// ʹ�� HTTP_GET ������ȡ����
        /// </summary>
        /// <param name="path">���·�����硰/index.aspx��</param>
        /// <returns>HTTP��Ӧ���</returns>
        public string GetData(string path)
        {
            string result;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(GetFullURL(path));
            httpRequest.CookieContainer = currentCookies;

            WebResponse webResponse = httpRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
            StreamReader readStream = new StreamReader(stream, encode);
            result = readStream.ReadToEnd();
            readStream.Close();
            stream.Close();

            return result;
        }
        #endregion

        #region public string PostData(string path, string data)
        /// <summary>
        /// ʹ�� HTTP_POST ������ȡ����
        /// </summary>
        /// <param name="path">���·�����硰/login.aspx��</param>
        /// <param name="data">��ҪPOST������</param>
        /// <returns>HTTP��Ӧ���</returns>
        public string PostData(string path, string data)
        {
            string result;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(GetFullURL(path));
            httpRequest.CookieContainer = currentCookies;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentLength = data.Length;
            //httpRequest.Referer = GetURL("/cn");
            httpRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(data);
            streamWriter.Flush();
            streamWriter.Close();
            WebResponse webResponse = httpRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
            StreamReader readStream = new StreamReader(stream, encode);
            result = readStream.ReadToEnd();
            readStream.Close();
            stream.Close();

            return result;
        }
        #endregion

    }
}

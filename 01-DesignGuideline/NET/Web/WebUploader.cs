/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�WebUploader.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��58��
 * * �ļ���ʶ��FC0090B0-D61A-4503-9952-5A1C215E3815
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace codest.Net.Web
{
    /// <summary>   
    /// ����ʵ�����ļ��ϴ����ܣ���Ҫָ��HtmlInputFile �ؼ�   
    /// ����1�����Զ��ļ����ͽ�������   
    /// ����2�����Զ��ļ���С���޽�������   
    ///    
    /// example:   
    ///  WebUploader up;   
    ///  up = new WebUploader(HtmlInputFile1);   
    ///  up.SvaePath = "c:\\inetpub\\wwwroot\\upload\\"; //����ָ���������ļ���·��   
    ///  up.AllowExtFile = ".jpg;.gif;"; //���������   
    ///  up.MaxSize = 500 * 1024; //��С����500k   
    ///  up.NewFileName = "newfile1"; //ָ���µ��ļ�������ָ�����޸�   
    ///  int errcode = up.Start(); //��ʼ�ϴ�   
    ///  string errmsg = up.GetErr(errcode); //��ô���������Ϣ   
    ///  Response.write(errmsg);��//��ʾ������Ϣ   
    /// </summary>   
    public class WebUploader : BaseClass
    {
        #region ��Ա����
        private System.Web.UI.HtmlControls.HtmlInputFile _scrfile;//HtmlInputFile �ؼ�   
        private string _savepath = "";//�����ļ���·��   
        private string _newfilename = "";//�ļ�������Ϊ   
        private string _newextfile = "";//�ļ���׺   
        private int _maxsize = 0;//�ļ���С����   
        private string _extfile = "";//����ĺ�׺�����á������ָ������.����Ϊ��ʱ����ȫ���ļ�����   
        #endregion

        #region �ӿڷ�װ

        #region public string SavePath
        /// <summary>
        /// ��ȡ��ָ���ļ�����·��   
        /// </summary>
        public string SavePath
        {
            get { return _savepath; }
            set
            {
                _savepath = value;
                if (_savepath.Substring(_savepath.Length) != "\\")
                {
                    _savepath += "\\";
                }
            }
        }
        #endregion public string SavePath

        #region public int MaxSize
        /// <summary>
        /// ��ȡ��ָ���ļ���С����
        /// </summary>
        public int MaxSize
        {
            get { return _maxsize; }
            set { _maxsize = value; }
        }
        #endregion

        #region public string AllowExtFile
        /// <summary>
        /// ��ȡ��ָ��������ļ���׺�б��á������ָ������.��   
        /// </summary>
        public string AllowExtFile
        {
            get { return _extfile; }
            set { _extfile = value; }
        }
        #endregion

        #region public string NewFileName
        /// <summary>
        /// ��ȡ��ָ���µ��ļ�������������׺   
        /// </summary>
        public string NewFileName
        {
            get { return _newfilename; }
            set { _newfilename = value; }
        }
        #endregion

        #region public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        /// <summary>
        /// ��ȡ��ָ��HtmlInputFile�ؼ�   
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        {
            get { return _scrfile; }
            set
            {
                string s;
                _scrfile = value;
                s = _scrfile.PostedFile.FileName;
                s = s.Substring(s.LastIndexOf('.'));
                _newextfile = s;
            }
        }
        #endregion
        
        #endregion

        #region ����/��������

        #region public WebUploader()
        /// <summary>
        /// ���캯������ָ���κ�����   
        /// </summary>
        public WebUploader()
        {

        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        {
            this.FileSource = scrFile;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        /// <summary>
        /// ���캯�����ϴ����ļ��������޸�   
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�</param>
        /// <param name="SavePath">����·��</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        {
            this.FileSource = scrFile;
            _savepath = SavePath;
            _newfilename = scrFile.PostedFile.FileName;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath, string NewFileName)
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�</param>
        /// <param name="SavePath">����·��</param>
        /// <param name="NewFileName">�µ��ļ�������������׺��</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath, string NewFileName)
        {
            this.FileSource = scrFile;
            _savepath = SavePath;
            _newfilename = NewFileName;
        }
        #endregion
        
        #endregion

        #region private bool CheckExt()
        /// <summary>
        /// ����׺�Ƿ����Ҫ��
        /// </summary>
        /// <returns></returns>
        private bool CheckExt()
        {
            if (_extfile == "") return true;
            string[] exts = null;
            exts = _extfile.Split(new char[] { ';' });
            int i = 0;
            for (i = 0; i <= exts.GetUpperBound(0); i++)
            {
                if (exts[i] == _newextfile) return true;
            }
            return false;
        }
        #endregion 

        #region public int Start()
        /// <summary>
        /// ׼�������󣬿�ʼ�ϴ�
        /// </summary>
        /// <returns></returns>
        public int Start()
        {
            if (_scrfile.PostedFile.ContentLength == 0)
            {
                return 504; //no source   
            }
            else if ((_scrfile.PostedFile.ContentLength >= _maxsize) && (_maxsize != 0))
            {
                return 501; //out of the range   
            }
            else if ((_savepath == "") || (_newfilename == ""))
            {
                return 505; //no filename or path    
            }
            else if (!CheckExt())
            {
                return 502; //ext is not allow   
            }
            try
            {
                _scrfile.PostedFile.SaveAs(_savepath + _newfilename + _newextfile);
                return 0;
            }
            catch
            {
                return 500; //unknow error   
            }
        }
        #endregion

        #region public string GetErr(int errCode)
        /// <summary>
        /// ����start()��������ֵ��Ϊ0�����ÿɻ�ȡ������Ϣ   
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        public string GetErr(int errCode)
        {
            switch (errCode)
            {
                case 500:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
                case 501:
                    return "�ļ���С��������";
                case 502:
                    return "�ļ����Ͳ����Ϲ涨��ֻ����" + _extfile + "���͵��ļ�";
                case 504:
                    return "û��ָ����Ҫ�ϴ����ļ�";
                default:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
            }
        }
        #endregion
    }  

}

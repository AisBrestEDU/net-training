/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：WebUploader.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分58秒
 * * 文件标识：FC0090B0-D61A-4503-9952-5A1C215E3815
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace codest.Net.Web
{
    /// <summary>   
    /// 该类实现了文件上传功能，需要指定HtmlInputFile 控件   
    /// 功能1：可以对文件类型进行限制   
    /// 功能2：可以对文件大小上限进行限制   
    ///    
    /// example:   
    ///  WebUploader up;   
    ///  up = new WebUploader(HtmlInputFile1);   
    ///  up.SvaePath = "c:\\inetpub\\wwwroot\\upload\\"; //必须指定，保存文件的路径   
    ///  up.AllowExtFile = ".jpg;.gif;"; //允许的类型   
    ///  up.MaxSize = 500 * 1024; //大小限制500k   
    ///  up.NewFileName = "newfile1"; //指定新的文件名，不指定则不修改   
    ///  int errcode = up.Start(); //开始上传   
    ///  string errmsg = up.GetErr(errcode); //获得错误描述信息   
    ///  Response.write(errmsg);　//显示错误信息   
    /// </summary>   
    public class WebUploader : BaseClass
    {
        #region 成员变量
        private System.Web.UI.HtmlControls.HtmlInputFile _scrfile;//HtmlInputFile 控件   
        private string _savepath = "";//保存文件的路径   
        private string _newfilename = "";//文件重命名为   
        private string _newextfile = "";//文件后缀   
        private int _maxsize = 0;//文件大小限制   
        private string _extfile = "";//允许的后缀名，用“；”分割，包含“.”，为空时允许全部文件类型   
        #endregion

        #region 接口封装

        #region public string SavePath
        /// <summary>
        /// 获取或指定文件保存路径   
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
        /// 获取或指定文件大小上限
        /// </summary>
        public int MaxSize
        {
            get { return _maxsize; }
            set { _maxsize = value; }
        }
        #endregion

        #region public string AllowExtFile
        /// <summary>
        /// 获取或指定允许的文件后缀列表，用“；”分割，包含“.”   
        /// </summary>
        public string AllowExtFile
        {
            get { return _extfile; }
            set { _extfile = value; }
        }
        #endregion

        #region public string NewFileName
        /// <summary>
        /// 获取或指定新的文件名，不包含后缀   
        /// </summary>
        public string NewFileName
        {
            get { return _newfilename; }
            set { _newfilename = value; }
        }
        #endregion

        #region public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        /// <summary>
        /// 获取或指定HtmlInputFile控件   
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

        #region 构造/析构函数

        #region public WebUploader()
        /// <summary>
        /// 构造函数，不指定任何数据   
        /// </summary>
        public WebUploader()
        {

        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        {
            this.FileSource = scrFile;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        /// <summary>
        /// 构造函数，上传后文件名不作修改   
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件</param>
        /// <param name="SavePath">保存路径</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        {
            this.FileSource = scrFile;
            _savepath = SavePath;
            _newfilename = scrFile.PostedFile.FileName;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath, string NewFileName)
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件</param>
        /// <param name="SavePath">保存路径</param>
        /// <param name="NewFileName">新的文件名（不包含后缀）</param>
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
        /// 检测后缀是否符合要求
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
        /// 准备就绪后，开始上传
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
        /// 调用start()后，若返回值不为0，调用可获取错误信息   
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        public string GetErr(int errCode)
        {
            switch (errCode)
            {
                case 500:
                    return "未知内部或外部的错误";
                case 501:
                    return "文件大小超出限制";
                case 502:
                    return "文件类型不符合规定，只允许：" + _extfile + "类型的文件";
                case 504:
                    return "没有指定需要上传的文件";
                default:
                    return "未知内部或外部的错误";
            }
        }
        #endregion
    }  

}

/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：WebServiceInvoker.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时02分00秒
 * * 文件标识：85A3FD5B-02D7-420A-98CE-89D7FBF10430
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Services.Description;
using System.IO;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace codest.Net.WebService
{
    /// <summary>
    /// 实现动态调用Web Service
    /// </summary>
    public class WebServiceInvoker
    {
        #region public object InvokeWebService(string url, string methodname, object[] args)
        /// <summary>
        /// 动态调用Web Service
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="methodname">要调用Web Service的方法名称</param>
        /// <param name="args">要调用Web Service的参数列表</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string methodname, object[] args)
        {
            return this.InvokeWebService(url, null, methodname, args);
        }
        #endregion

        #region public object InvokeWebService(string url, string methodname, object arg)
        /// <summary>
        /// 动态调用Web Service，传入单个参数
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="methodname">调用Web Service的方法名称</param>
        /// <param name="arg">要调用Web Service的参数</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string methodname, object arg)
        {
            return this.InvokeWebService(url, null, methodname, new object[] { arg });
        }
        #endregion

        #region public object InvokeWebService(string url, string classname, string methodname, object[] args)
        /// <summary>
        /// 动态调用Web Service
        /// </summary>
        /// <param name="url">Web Service的URL地址</param>
        /// <param name="classname">要调用Web Service的类的名称</param>
        /// <param name="methodname">要调用Web Service的方法名称</param>
        /// <param name="args">要调用Web Service的参数列表</param>
        /// <returns>方法处理结果</returns>
        public object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = this.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
        #endregion

        #region private string GetWsClassName(string wsUrl)
        /// <summary>
        /// 通过Web Service的URL获取类名
        /// </summary>
        /// <param name="wsUrl">Web Service的URL地址</param>
        /// <returns>Web Service的类名</returns>
        private string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }

}

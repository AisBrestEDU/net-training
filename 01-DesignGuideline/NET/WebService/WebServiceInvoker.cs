/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�WebServiceInvoker.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��00��
 * * �ļ���ʶ��85A3FD5B-02D7-420A-98CE-89D7FBF10430
 * * ����ժҪ��
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
    /// ʵ�ֶ�̬����Web Service
    /// </summary>
    public class WebServiceInvoker
    {
        #region public object InvokeWebService(string url, string methodname, object[] args)
        /// <summary>
        /// ��̬����Web Service
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="methodname">Ҫ����Web Service�ķ�������</param>
        /// <param name="args">Ҫ����Web Service�Ĳ����б�</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string methodname, object[] args)
        {
            return this.InvokeWebService(url, null, methodname, args);
        }
        #endregion

        #region public object InvokeWebService(string url, string methodname, object arg)
        /// <summary>
        /// ��̬����Web Service�����뵥������
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="methodname">����Web Service�ķ�������</param>
        /// <param name="arg">Ҫ����Web Service�Ĳ���</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string methodname, object arg)
        {
            return this.InvokeWebService(url, null, methodname, new object[] { arg });
        }
        #endregion

        #region public object InvokeWebService(string url, string classname, string methodname, object[] args)
        /// <summary>
        /// ��̬����Web Service
        /// </summary>
        /// <param name="url">Web Service��URL��ַ</param>
        /// <param name="classname">Ҫ����Web Service���������</param>
        /// <param name="methodname">Ҫ����Web Service�ķ�������</param>
        /// <param name="args">Ҫ����Web Service�Ĳ����б�</param>
        /// <returns>����������</returns>
        public object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = this.GetWsClassName(url);
            }

            try
            {
                //��ȡWSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //���ɿͻ��˴��������
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //�趨�������
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //���������
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

                //���ɴ���ʵ���������÷���
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
        /// ͨ��Web Service��URL��ȡ����
        /// </summary>
        /// <param name="wsUrl">Web Service��URL��ַ</param>
        /// <returns>Web Service������</returns>
        private string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }

}

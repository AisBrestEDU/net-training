/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DSAEncrypt.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��16��
 * * �ļ���ʶ��056C74BA-5776-4E24-9BBB-DE22E25481B7
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using codest.Encode;
using codest.Net;

namespace codest.Encode
{
    /// <summary>
    /// ʵ�ֶ����ݽ���DSAǩ������֤
    /// </summary>
    public class DSAEncrypt : SerializableBaseClass<DSAEncrypt>
    {
        #region ��Ա����
        /// <summary>
        /// �ṩDSA�㷨
        /// </summary>
        protected DSACryptoServiceProvider dsac;
        #endregion

        #region �ӿڷ�װ

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public DSAEncrypt()
        {
            dsac = new DSACryptoServiceProvider();
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~DSAEncrypt()
        {
            dsac.Clear();
        }
        #endregion

        #region public byte[] GetSignature(string srcData)
        /// <summary>
        /// ���ַ������ݽ���ǩ��
        /// </summary>
        /// <param name="srcData">�ַ�������</param>
        /// <returns>DSAǩ��</returns>
        public byte[] GetSignature(string srcData)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            return GetSignature(binaryData);
        }
        #endregion

        #region public byte[] GetSignature(byte[] srcData)
        /// <summary>
        /// �Զ��������ݽ���ǩ��
        /// </summary>
        /// <param name="srcData">����������</param>
        /// <returns>DSAǩ��</returns>
        public byte[] GetSignature(byte[] srcData)
        {
            byte[] sign = dsac.SignData(srcData);
            return sign;
        }
        #endregion

        #region public bool VerifySignature(byte[] srcData, byte[] signature)
        /// <summary>
        /// ��֤ǩ��
        /// </summary>
        /// <param name="srcData">��Ҫ��֤������</param>
        /// <param name="signature">DSAǩ��</param>
        /// <returns>ǩ���Ƿ���ȷ</returns>
        public bool VerifySignature(byte[] srcData, byte[] signature)
        {
            bool ver = dsac.VerifyData(srcData, signature);
            return ver;
        }
        #endregion

        #region public bool VerifySignature(string srcData, byte[] signature)
        /// <summary>
        /// ��֤ǩ��
        /// </summary>
        /// <param name="srcData">��Ҫ��֤������</param>
        /// <param name="signature">DSAǩ��</param>
        /// <returns>ǩ���Ƿ���ȷ</returns>
        public bool VerifySignature(string srcData, byte[] signature)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            bool ver = dsac.VerifyData(binaryData, signature);
            return ver;
        }
        #endregion
    }

}

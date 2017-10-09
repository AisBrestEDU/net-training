/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DESEncrypt.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��15��
 * * �ļ���ʶ��29E9467D-FDF9-46F7-A47C-531AEA3718EF
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace codest.Encode
{
    /// <summary>
    /// ʵ�����ݵ�DES���ܽ���
    /// </summary>
    public class DESEncrypt : SerializableBaseClass<DESEncrypt>
    {
        #region ��Ա����
        /// <summary>
        /// �̻��ļ��ܽ�������
        /// </summary>
        private byte[] keys;
        /// <summary>
        /// ��Կ
        /// </summary>
        private string encryptKey;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// �ڲ��̻�����Key
        /// </summary>
        public byte[] Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        /// <summary>
        /// ��Կ
        /// </summary>
        public string EncryptKey
        {
            get { return encryptKey; }
            set { encryptKey = value; }
        }
        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public DESEncrypt()
        {
            keys = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="encryptKey">��Կ</param>
        public DESEncrypt(string encryptKey)
            :this()
        {
            this.encryptKey = encryptKey;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~DESEncrypt()
        { }
        #endregion

        #region public string Encrypt(string srcData)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="srcData">Ҫ���ܵ��ַ���</param>
        /// <returns>���ܹ�������</returns>
        public string Encrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = Encrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }
        #endregion

        #region public byte[] Encrypt(byte[] srcData)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="srcData">Ҫ���ܵ�������</param>
        /// <returns>���ܹ�������</returns>
        public byte[] Encrypt(byte[] srcData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = keys;
                byte[] inputByteArray = srcData;
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region public string Decrypt(string srcData)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="srcData">��Ҫ���ܵ�����</param>
        /// <returns>���ܺ������</returns>
        public string Decrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = Decrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }
        #endregion

        #region public byte[] Decrypt(byte[] srcData)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="srcData">��Ҫ���ܵ�����</param>
        /// <returns>���ܺ������</returns>
        public byte[] Decrypt(byte[] srcData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(encryptKey);
                byte[] rgbIV = keys;
                byte[] inputByteArray = srcData;
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion


    }
}

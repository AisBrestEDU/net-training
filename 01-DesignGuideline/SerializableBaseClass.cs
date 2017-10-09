/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：SerializableBaseClass.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时02分20秒
 * * 文件标识：736FBD08-7EDA-47C1-B294-9E3B49305977
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace codest
{
    /// <summary>
    /// 为对象提供XML序列化、二进制序列化及反序列化功能
    /// 实现了ICloneable接口
    /// </summary>
    /// <typeparam name="T">继承类的类型</typeparam>
    [Serializable()]
    public abstract class SerializableBaseClass<T> : ICloneable
    {
        #region public virtual byte[] BinarySerialize()
        /// <summary>
        /// 对类进行二进制序列化
        /// </summary>
        /// <returns>序列化代码</returns>
        public virtual byte[] BinarySerialize()
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            ser.Serialize(mStream, this);
            byte[] buf = mStream.ToArray();
            mStream.Close();
            return buf;
        }
        #endregion

        #region public virtual string XMLSerialize()
        /// <summary>
        /// 对类进行XML序列化
        /// </summary>
        /// <returns>XML序列化代码</returns>
        public virtual string XMLSerialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(GetType());
            MemoryStream stream = new MemoryStream();
            xmlSerializer.Serialize(stream, this);
            byte[] buf = stream.ToArray();
            string xml = Encoding.ASCII.GetString(buf);
            stream.Close();
            return xml;
        }
        #endregion

        #region  public static T DeSerialize(byte[] binary)
        /// <summary>
        /// 对类进行二进制反序列化
        /// </summary>
        /// <param name="binary">二进制序列化代码</param>
        /// <returns>反序列化后的对象，若失败则返回null</returns>
        public static T DeSerialize(byte[] binary)
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream(binary);
            T o = (T)ser.Deserialize(mStream);
            mStream.Close();
            return o;
        }
        #endregion 

        #region public static T DeSerialize(string xmlString)
        /// <summary>
        /// 对类进行XML反序列化
        /// </summary>
        /// <param name="xmlString">XML序列化代码</param>
        /// <returns>反序列化后的对象，若失败则返回null</returns>
        public static T DeSerialize(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            byte[] buf = Encoding.ASCII.GetBytes(xmlString);
            MemoryStream stream = new MemoryStream(buf);
            T o = (T)xmlSerializer.Deserialize(stream);
            return o;
        }
        #endregion

        #region public static bool TryDeSerialize(byte[] binary, ref T obj)
        /// <summary>
        /// 尝试使用二进制数据对类进行反序列化
        /// </summary>
        /// <param name="binary">序列化二进制数据</param>
        /// <param name="obj">返回对象引用</param>
        /// <returns>反序列化是否成功</returns>
        public static bool TryDeSerialize(byte[] binary, ref T obj)
        {
            try
            {
                obj = DeSerialize(binary);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region public static bool TryDeSerialize(string  xmlString, ref T obj)
        /// <summary>
        /// 尝试使用XML数据对类进行反序列化
        /// </summary>
        /// <param name="xmlString">序列化XML数据</param>
        /// <param name="obj">返回对象引用</param>
        /// <returns>反序列化是否成功</returns>
        public static bool TryDeSerialize(string  xmlString, ref T obj)
        {
            try
            {
                obj = DeSerialize(xmlString);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion 

        #region public virtual T Copy()
        /// <summary>
        /// 完成对象的浅复制
        /// </summary>
        /// <returns>对象的副本</returns>
        public virtual T Copy()
        {
            return (T)Clone();
        }
        #endregion

        #region ICloneable 成员
        /// <summary>
        /// 完成对象的浅复制
        /// </summary>
        /// <returns>对象的副本</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion


    }

}

/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：BaseClass.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时02分17秒
 * * 文件标识：F78FB93F-3459-4CF8-82CB-4BEF90B3CEAA
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Text;
using System.IO;
using System.ComponentModel;
namespace codest
{
    #region public delegate void NullParamEvent();
    /// <summary>
    /// 无参数且无返回值的托管类型
    /// </summary>
    public delegate void NullParamEvent();
    #endregion

    /// <summary>
    /// Icyplayer所有类的基类
    /// 实现IDisposable, ICloneable接口
    /// </summary>
    public abstract class BaseClass : IDisposable, ICloneable
    {
        #region 成员变量
        /// <summary>
        /// 托管资源容器
        /// </summary>
        private Container components = null; 
        /// <summary>
        /// 指示对象是否已经析构
        /// </summary>
        protected bool disposed = false;
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// BaseClass构造函数
        /// </summary>
        public BaseClass()
        { 
        }
        /// <summary>
        /// BaseClass析构函数
        /// </summary>
        ~BaseClass()
        {
            Dispose(false);
        }
        #endregion

        #region protected virtual void Dispose(bool disposing)
        /// <summary>
        /// 撤销对象
        /// </summary>
        /// <param name="disposing">false：系统调用，true：手动调用</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //释放托管资源
                if (components != null)
                {
                    components.Dispose();
                }
                disposed = true;
            }
            //释放非托管资源
            //base.Dispose(disposing);
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

        #region IDisposable 成员
        /// <summary>
        /// 显式撤销对象
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }


}

/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DataUpdater.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分41秒
 * * 文件标识：15ED7E83-DEB1-44EE-933A-FFB9A126E20D
 * * 内容摘要：
 * *******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace codest.Data
{
    /// <summary>
    /// 所有数据库更新器的基类
    /// </summary>
    public abstract class DataUpdater : BaseClass
    {
        #region 成员变量
        private bool autoRelease;
        internal int updaterID;
        #endregion

        #region 接口封装
        /// <summary>
        /// 指示调用Update()后是否自动释放当前更新器
        /// 默认为：true
        /// </summary>
        public bool AutoRelease
        {
            get { return autoRelease; }
            set { autoRelease = value; }
        }
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">分配容器中唯一的ID</param>
        public DataUpdater(int id)
        {
            autoRelease = true;
            updaterID = id;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DataUpdater()
        {
        }
        #endregion

        #region  protected override void Dispose(bool disposing)
        /// <summary>
        /// 释放由当前对象控制的所有资源
        /// </summary>
        /// <param name="disposing">显式调用</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //释放托管资源
            }
            //释放非托管资源
            base.Dispose(disposing);
        }
        #endregion

        #region  protected virtual void DecideRelease()
        /// <summary>
        /// 决定是否自动释放更新器
        /// </summary>
        protected virtual void DecideRelease()
        {
            if (autoRelease)
                Release();
        }
        #endregion

        #region public abstract DataTable SelectWithUpdate(string SQLCmd)

        /// <summary>
        /// 使对象将进入修改模式状态
        /// 用户可以修改返回结果，包括删除、修改和增加行
        /// 再调用Update(DataTable)进行更新操作
        /// 此后，对象将退出修改模式状态
        /// </summary>
        public abstract DataTable SelectWithUpdate(string SQLCmd);
        #endregion

        #region public abstract DataTable InsertMode(string TableName)

        /// <summary>
        /// 使对象进入修改模式
        /// 用户可以在返回表结构的DataTable中添加数据
        /// 再调用Update(DataTable)进行更新操作
        /// 此后，对象将退出修改模式状态
        /// </summary>
        public abstract DataTable InsertMode(string TableName);
        #endregion

        #region public abstract void Update(System.Data.DataTable DataTableSource)

        /// <summary>
        /// 关闭修改模式,并根据DataTable进行更新操作
        /// </summary>
        public abstract void Update(System.Data.DataTable DataTableSource);
        #endregion

        #region  public abstract void Release()
        /// <summary>
        /// 释放当前更新器
        /// </summary>
        public abstract void Release();
        #endregion

    }

}

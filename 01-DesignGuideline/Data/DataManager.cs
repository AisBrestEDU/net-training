/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DataManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分43秒
 * * 文件标识：124F76B9-05EE-4B2A-9400-A233BC1D4BF3
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace codest.Data
{
    /// <summary>
    /// 所有数据库管理器的基类
    /// </summary>
    public abstract class DataManager : BaseClass
    {
        #region 成员变量
        internal int execNum = 0;
        /// <summary>
        /// 保存当前管理器中所有已分配的更新器
        /// </summary>
        protected Hashtable dataUpdaterColl;
        private string connString = string.Empty;
        private string _dbscr = string.Empty;
        #endregion

        #region 接口封装
        /// <summary>
        /// 获取或设置数据库连接字符串
        /// </summary>
        public string ConnString
        {
          get { return connString; }
          set { connString = value; }
        }
        /// <summary>
        /// 获取当前数据库连接查询的次数
        /// </summary>
        public int ExecNum
        {
            get { return execNum; }
        }

        /// <summary>
        /// 获取或指定数据库源
        /// </summary>
        public string DataSource
        {
            get { return _dbscr; }
            set { _dbscr = value; }
        }
        #endregion
         
        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataManager()
        {
            dataUpdaterColl = new Hashtable();
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DataManager()
        {
            Dispose(false);
        }
        #endregion

        #region protected override void Dispose(bool disposing)
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

        //--begin--连接数据库操作--

        #region public abstract void Open()
        /// <summary>
        /// 打开数据库
        /// </summary>
        public abstract void Open();
        #endregion

        #region public abstract void OpenByConnString()
        /// <summary>
        /// 使用数据库连接字符串打开数据库
        /// </summary>
        public abstract void OpenByConnString();
        #endregion

        #region public abstract void Close()
        /// <summary>
        /// 关闭SQL连接
        /// </summary>
        public abstract void Close();
        #endregion

        //--end----连接数据库操作--

        //--begin--访问数据库操作--

        #region public abstract int Exec(string SQLCmd)
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQLCmd">SQL语句</param>
        /// <returns>受响应的行数</returns>
        public abstract int Exec(string SQLCmd);
        #endregion

        #region public abstract DataTable Select(string SQLCmd)

        /// <summary>
        /// 执行SQL语句，将响应的数据填充到DataTable中，不能进行更新操作
        /// </summary>
        public abstract DataTable Select(string SQLCmd);
        #endregion

        #region public abstract DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)

        /// <summary>
        /// 选择一定范围记录的Select语句
        /// </summary>
        public abstract DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord);
        #endregion

        #region public abstract DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        /// <summary>
        /// 实现分页的Select
        /// </summary>
        public abstract DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID);
        #endregion

        #region public abstract bool Delete(string SQLCmd)
        /// <summary>
        /// 执行删除操作的SQL语句
        /// </summary>
        public abstract bool Delete(string SQLCmd);
        #endregion

        //--end----访问数据库操作--

        //--begin--更新器操作--

        #region public abstract DataUpdater AllocateDataUpdater()
        /// <summary>
        /// 分配一个更新器
        /// </summary>
        /// <returns></returns>
        public abstract DataUpdater AllocateDataUpdater();
        #endregion

        #region  public virtual void ReleaseAllDataUpdater()
        /// <summary>
        /// 释放当前实例中所有的更新器
        /// </summary>
        public virtual void ReleaseAllDataUpdater()
        {
            foreach (DictionaryEntry de in dataUpdaterColl)
            {
                DataUpdater updater = (DataUpdater)de.Value;
                updater.Dispose();
            }
            dataUpdaterColl.Clear();
        }
        #endregion

        #region  public virtual void ReleaseDataUpdater(DataUpdater updater)
        /// <summary>
        /// 释放一个更新器
        /// </summary>
        /// <param name="updater">更新器</param>
        public virtual void ReleaseDataUpdater(DataUpdater updater)
        {
            this.dataUpdaterColl.Remove(updater.updaterID);
            updater.Dispose();
        }
        #endregion

        //--end----更新器操作--
    }
}

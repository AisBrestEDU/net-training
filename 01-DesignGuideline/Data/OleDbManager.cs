/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：OleDbManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分50秒
 * * 文件标识：CF457278-457E-44F7-9E95-E873F4FFDF2F
 * * 内容摘要：
 * *******************************************************************************/


using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace codest.Data
{
    /// <summary>
    /// OleDb(默认Access)数据库数据库管理器
    /// </summary>
    public class OleDbManager : DataManager
    {
        #region 成员变量
        internal System.Data.OleDb.OleDbConnection _conn; //access数据库连接
        private int lastUpdaterId;
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public OleDbManager()
        {
            lastUpdaterId = 0;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">设置数据源</param>
        public OleDbManager(string dataSource)
            : this()
        {
            base.DataSource = dataSource;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~OleDbManager()
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
                base.DataSource = null;
            }
            //释放非托管资源
            this.Close();
            base.Dispose(disposing);
        }
        #endregion
        
        //--begin--连接数据库操作--

        #region public void Open(string DataSource)
        /// <summary>
        /// 打开指定Access数据库文件
        /// </summary>
        /// <param name="DataSource"></param>
        public void Open(string DataSource)
        {
            base.DataSource = DataSource;
            Open();
        }

        #endregion

        #region public override void Open()
        /// <summary>
        /// 打开已经设定好的Access数据库文件
        /// </summary>
        public override void Open()
        {
            base.ConnString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + base.DataSource;
            this.OpenByConnString();
        }

        #endregion

        #region public override void OpenByConnString()
        /// <summary>
        /// 使用数据库连接字符串打开数据库
        /// </summary>
        public override void OpenByConnString()
        {
            _conn = new OleDbConnection();
            _conn.ConnectionString = base.ConnString;
            _conn.Open();
        }

        #endregion

        #region  public override void Close()
        /// <summary>
        /// 关闭SQL连接
        /// </summary>
        public override void Close()
        {
            try
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
            catch
            {

            }
        }
        #endregion
        
        //--end----连接数据库操作--

        //--begin--访问数据库操作--

        #region  public override int Exec(string SQLCmd)
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQLCmd">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public override int Exec(string SQLCmd)
        {
            OleDbCommand cmd = new OleDbCommand(SQLCmd, _conn);
            return cmd.ExecuteNonQuery();
        }

        #endregion

        #region public override DataTable Select(string SQLCmd)
        /// <summary>
        /// 执行SQL语句，将响应的数据填充到DataTable中，不能进行更新操作
        /// </summary>
        /// <param name="SQLCmd"></param>
        /// <returns></returns>
        public override DataTable Select(string SQLCmd)
        {
            execNum++;
            OleDbDataAdapter da;
            DataTable dt = new DataTable();
            da = new OleDbDataAdapter(SQLCmd, _conn);
            da.Fill(dt);
            da.Dispose();
            //DecideRelease();
            return dt;
        }

        #endregion

        #region public override DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)
        /// <summary>
        /// 选择一定范围记录的Select语句
        /// </summary>
        /// <param name="SQLCmd"></param>
        /// <param name="srcTalbe"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecord"></param>
        /// <returns></returns>
        public override DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)
        {
            execNum++;
            OleDbDataAdapter da;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            da = new OleDbDataAdapter(SQLCmd, _conn);
            da.Fill(ds, startRecord, maxRecord, srcTalbe);
            dt = ds.Tables[0];
            da.Dispose();
            //DecideRelease();
            return dt;
        }

        #endregion

        #region public override DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        /// <summary>
        /// 实现分页的Select
        /// </summary>
        /// <param name="SQLCmd"></param>
        /// <param name="srcTalbe"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public override DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        {
            if (pageID == 0) pageID = 1;
            return Select(SQLCmd, srcTalbe, pageSize * (pageID - 1), pageSize);
        }

        #endregion

        #region public override bool Delete(string SQLCmd)
        /// <summary>
        /// 执行删除操作的SQL语句
        /// </summary>
        /// <param name="SQLCmd"></param>
        /// <returns></returns>
        public override bool Delete(string SQLCmd)
        {
            if (SQLCmd.Substring(0, 6).ToLower() != "delete") return false;
            execNum++;
            OleDbCommand cmd;
            cmd = new OleDbCommand(SQLCmd, _conn);
            cmd.ExecuteNonQuery();
            //DecideRelease();
            return true;
        }
        #endregion
        
        //--end----访问数据库操作--

        //--begin--更新器操作--

        #region  public override DataManager.DataUpdater AllocateDataUpdater()
        /// <summary>
        /// 分配一个新的更新器
        /// </summary>
        /// <returns>OleDb更新器</returns>
        public override DataUpdater AllocateDataUpdater()
        {
            int id = lastUpdaterId++;
            OleDbUpdater updater = new OleDbUpdater(id, this);
            base.dataUpdaterColl.Add(id, updater);
            return updater;
        }
        #endregion
        
        //--end----更新器操作--

    }
}
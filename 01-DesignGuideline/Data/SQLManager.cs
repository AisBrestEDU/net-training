/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：SQLManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分56秒
 * * 文件标识：A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * 内容摘要：
 * *******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace codest.Data
{
    /// <summary>
    /// SQL Server数据库管理器
    /// </summary>
    public class SQLManager : DataManager
    {
        #region 成员变量
        private string database;
        private string username;
        private string password;
        internal SqlConnection _conn; //数据库连接
        private int lastUpdaterId = 0;
        #endregion

        #region 接口封装
        /// <summary>
        /// 库名称
        /// </summary>
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        /// <summary>
        /// 连接SQL Server数据库的用户名
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// 连接SQL Server数据库的密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLManager()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）</param>
        /// <param name="database">库名称</param>
        /// <param name="usr">用户名</param>
        /// <param name="pwd">密码</param>
        public SQLManager(string dataSource, string database, string usr, string pwd)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~SQLManager()
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
            this.Close();
            base.Dispose(disposing);
        }

        #endregion
        
        //--begin--连接数据库操作--

        #region  public void Open(string dataSource, string database, string usr, string pwd)
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）</param>
        /// <param name="database">库名称</param>
        /// <param name="usr">用户名</param>
        /// <param name="pwd">密码</param>
        public void Open(string dataSource, string database, string usr, string pwd)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
            this.Open();
        }
        #endregion

        #region public override void Open()
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public override void Open()
        {
            string connstr = string.Empty;
            connstr += "server=" + base.DataSource + ";";
            connstr += "database=" + this.database + ";";
            connstr += "uid="+this.username+";";
            connstr += "pwd=" + this.password;
            base.ConnString = connstr;
            this.OpenByConnString();
        }
        #endregion

        #region public override void OpenByConnString()
        /// <summary>
        /// 使用数据库连接字符串打开数据库
        /// </summary>
        public override void OpenByConnString()
        {
            _conn = new SqlConnection();
            _conn.ConnectionString = base.ConnString;
            _conn.Open();
        }
        #endregion

        #region public override void Close()
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
            SqlCommand cmd = new SqlCommand(SQLCmd, _conn);
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
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(SQLCmd, _conn);
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
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(SQLCmd, _conn);
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
            SqlCommand cmd;
            cmd = new SqlCommand(SQLCmd, _conn);
            cmd.ExecuteNonQuery();
            //DecideRelease();
            return true;
        }
        #endregion

        //--end----访问数据库操作--

        //--begin--更新器操作--

        #region  public override DataManager.DataUpdater AllocateDataUpdater()
        /// <summary>
        /// 分配一个新的SQL更新器
        /// </summary>
        /// <returns></returns>
        public override DataUpdater AllocateDataUpdater()
        {
            int id = lastUpdaterId++;
            SQLUpdater updater = new SQLUpdater(id, this);
            base.dataUpdaterColl.Add(id, updater);
            return updater;
        }
        #endregion

        //--end----更新器操作--


    }
}

/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SQLManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��56��
 * * �ļ���ʶ��A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * ����ժҪ��
 * *******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace codest.Data
{
    /// <summary>
    /// SQL Server���ݿ������
    /// </summary>
    public class SQLManager : DataManager
    {
        #region ��Ա����
        private string database;
        private string username;
        private string password;
        internal SqlConnection _conn; //���ݿ�����
        private int lastUpdaterId = 0;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ������
        /// </summary>
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        /// <summary>
        /// ����SQL Server���ݿ���û���
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// ����SQL Server���ݿ������
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public SQLManager()
        {

        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��</param>
        /// <param name="database">������</param>
        /// <param name="usr">�û���</param>
        /// <param name="pwd">����</param>
        public SQLManager(string dataSource, string database, string usr, string pwd)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~SQLManager()
        {
            Dispose(false);
        }
        #endregion

        #region protected override void Dispose(bool disposing)
        /// <summary>
        /// �ͷ��ɵ�ǰ������Ƶ�������Դ
        /// </summary>
        /// <param name="disposing">��ʽ����</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //�ͷ��й���Դ
            }
            //�ͷŷ��й���Դ
            this.Close();
            base.Dispose(disposing);
        }

        #endregion
        
        //--begin--�������ݿ����--

        #region  public void Open(string dataSource, string database, string usr, string pwd)
        /// <summary>
        /// �����ݿ�����
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��</param>
        /// <param name="database">������</param>
        /// <param name="usr">�û���</param>
        /// <param name="pwd">����</param>
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
        /// �����ݿ�����
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
        /// ʹ�����ݿ������ַ��������ݿ�
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
        /// �ر�SQL����
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

        //--end----�������ݿ����--

        //--begin--�������ݿ����--

        #region  public override int Exec(string SQLCmd)
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="SQLCmd">SQL���</param>
        /// <returns>��Ӱ�������</returns>
        public override int Exec(string SQLCmd)
        {
            SqlCommand cmd = new SqlCommand(SQLCmd, _conn);
            return cmd.ExecuteNonQuery();
        }

        #endregion

        #region public override DataTable Select(string SQLCmd)
        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���
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
        /// ѡ��һ����Χ��¼��Select���
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
        /// ʵ�ַ�ҳ��Select
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
        /// ִ��ɾ��������SQL���
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

        //--end----�������ݿ����--

        //--begin--����������--

        #region  public override DataManager.DataUpdater AllocateDataUpdater()
        /// <summary>
        /// ����һ���µ�SQL������
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

        //--end----����������--


    }
}

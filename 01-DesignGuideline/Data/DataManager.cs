/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DataManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��43��
 * * �ļ���ʶ��124F76B9-05EE-4B2A-9400-A233BC1D4BF3
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace codest.Data
{
    /// <summary>
    /// �������ݿ�������Ļ���
    /// </summary>
    public abstract class DataManager : BaseClass
    {
        #region ��Ա����
        internal int execNum = 0;
        /// <summary>
        /// ���浱ǰ�������������ѷ���ĸ�����
        /// </summary>
        protected Hashtable dataUpdaterColl;
        private string connString = string.Empty;
        private string _dbscr = string.Empty;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ��ȡ���������ݿ������ַ���
        /// </summary>
        public string ConnString
        {
          get { return connString; }
          set { connString = value; }
        }
        /// <summary>
        /// ��ȡ��ǰ���ݿ����Ӳ�ѯ�Ĵ���
        /// </summary>
        public int ExecNum
        {
            get { return execNum; }
        }

        /// <summary>
        /// ��ȡ��ָ�����ݿ�Դ
        /// </summary>
        public string DataSource
        {
            get { return _dbscr; }
            set { _dbscr = value; }
        }
        #endregion
         
        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public DataManager()
        {
            dataUpdaterColl = new Hashtable();
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~DataManager()
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
            base.Dispose(disposing);
        }
        #endregion

        //--begin--�������ݿ����--

        #region public abstract void Open()
        /// <summary>
        /// �����ݿ�
        /// </summary>
        public abstract void Open();
        #endregion

        #region public abstract void OpenByConnString()
        /// <summary>
        /// ʹ�����ݿ������ַ��������ݿ�
        /// </summary>
        public abstract void OpenByConnString();
        #endregion

        #region public abstract void Close()
        /// <summary>
        /// �ر�SQL����
        /// </summary>
        public abstract void Close();
        #endregion

        //--end----�������ݿ����--

        //--begin--�������ݿ����--

        #region public abstract int Exec(string SQLCmd)
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="SQLCmd">SQL���</param>
        /// <returns>����Ӧ������</returns>
        public abstract int Exec(string SQLCmd);
        #endregion

        #region public abstract DataTable Select(string SQLCmd)

        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���
        /// </summary>
        public abstract DataTable Select(string SQLCmd);
        #endregion

        #region public abstract DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)

        /// <summary>
        /// ѡ��һ����Χ��¼��Select���
        /// </summary>
        public abstract DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord);
        #endregion

        #region public abstract DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        /// <summary>
        /// ʵ�ַ�ҳ��Select
        /// </summary>
        public abstract DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID);
        #endregion

        #region public abstract bool Delete(string SQLCmd)
        /// <summary>
        /// ִ��ɾ��������SQL���
        /// </summary>
        public abstract bool Delete(string SQLCmd);
        #endregion

        //--end----�������ݿ����--

        //--begin--����������--

        #region public abstract DataUpdater AllocateDataUpdater()
        /// <summary>
        /// ����һ��������
        /// </summary>
        /// <returns></returns>
        public abstract DataUpdater AllocateDataUpdater();
        #endregion

        #region  public virtual void ReleaseAllDataUpdater()
        /// <summary>
        /// �ͷŵ�ǰʵ�������еĸ�����
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
        /// �ͷ�һ��������
        /// </summary>
        /// <param name="updater">������</param>
        public virtual void ReleaseDataUpdater(DataUpdater updater)
        {
            this.dataUpdaterColl.Remove(updater.updaterID);
            updater.Dispose();
        }
        #endregion

        //--end----����������--
    }
}

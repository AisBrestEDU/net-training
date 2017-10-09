/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�OleDbUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��52��
 * * �ļ���ʶ��61502EEA-229B-4503-9A25-0DD25ED0B4B3
 * * ����ժҪ��
 * *******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace codest.Data
{
    /// <summary>
    /// OleDb���ݿ������
    /// </summary>
    public class OleDbUpdater : DataUpdater
    {
        #region ��Ա����
        /// <summary>
        /// ��ǰ��������ʹ�õ����ݿ������
        /// </summary>
        protected OleDbManager dataManager;
        /// <summary>
        /// ���и��²���ʱ����������������Ϣ
        /// </summary>
        protected System.Data.OleDb.OleDbDataAdapter _dap;
        /// <summary>
        /// ���и��²���ʱ�����OleDbDataAdapterʹ��
        /// </summary>
        protected System.Data.OleDb.OleDbCommandBuilder _cmdb;
        #endregion

        #region �ӿڷ�װ

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">������Ψһ�ĸ�����ID</param>
        /// <param name="manager">���ݿ������</param>
        public OleDbUpdater(int id, OleDbManager manager)
            : base(id)
        {
            dataManager = manager;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~OleDbUpdater()
        {
            this.Release();
        }
        #endregion

        #region  protected override void Dispose(bool disposing)
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
            dataManager = null;
            _cmdb = null;
            _dap = null;
            base.Dispose(disposing);
        }
        #endregion

        #region public override DataTable SelectWithUpdate(string SQLCmd)
        /// <summary>
        /// ʹ���󽫽����޸�ģʽ״̬
        /// �û������޸ķ��ؽ��������ɾ�����޸ĺ�������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        /// <param name="SQLCmd">SQL���</param>
        /// <returns>��ѯ��Ӧ���</returns>
        public override DataTable SelectWithUpdate(string SQLCmd)
        {
            dataManager.execNum++;
            System.Data.DataTable dt = new DataTable();
            _dap = new OleDbDataAdapter(SQLCmd, dataManager._conn);
            _cmdb = new OleDbCommandBuilder(_dap);
            _dap.Fill(dt);
            return dt;
        }

        #endregion

        #region public override DataTable InsertMode(string TableName)
        /// <summary>
        /// ʹ��������޸�ģʽ
        /// �û������ڷ��ر�ṹ��DataTable���������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        /// <param name="TableName">��Ҫ����ı�����</param>
        /// <returns>Ҫ����Ŀ���Ľṹ</returns>
        public override DataTable InsertMode(string TableName)
        {
            dataManager.execNum++;
            System.Data.DataTable dt = new DataTable();
            _dap = new OleDbDataAdapter("select * from [" + TableName + "] where false", dataManager._conn);
            _cmdb = new OleDbCommandBuilder(_dap);
            _dap.Fill(dt);
            return dt;
        }

        #endregion

        #region public override void Update(System.Data.DataTable DataTableSource)
        /// <summary>
        /// �ر��޸�ģʽ,������DataTable���и��²���
        /// </summary>
        /// <param name="DataTableSource">Ҫ�ύ�����ݱ�</param>
        public override void Update(System.Data.DataTable DataTableSource)
        {
            dataManager.execNum++;
            _dap.Update(DataTableSource);
            DecideRelease();
        }

        #endregion

        #region  public override void Release()
        /// <summary>
        /// �ͷŵ�ǰ������
        /// </summary>
        public override void Release()
        {
            dataManager.ReleaseDataUpdater(this);
        }
        #endregion
    }


}

/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��43��
 * * �ļ���ʶ��3A5149DB-F43C-4651-B708-FF9022AFF8FA
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace codest.Net.Sockets
{
    #region public delegate void DataArriveEvent(TCPThread tcpThread, byte[] buffer);
    /// <summary>
    /// ���ݵ�����д���ĺ�����ί��
    /// </summary>
    /// <param name="tcpThread">��ǰtcp�Ự����</param>
    /// <param name="buffer">����</param>
    public delegate void DataArriveEvent(TCPThread tcpThread, byte[] buffer);
    #endregion 
    
    /// <summary>
    /// �ṩTCP���ӷ���˵���
    /// </summary>
    public class TCPManager : BaseClass
    {
        #region ��Ա����
        private Socket socket;
        #endregion

        #region �����¼�
        /// <summary>
        /// �ͻ�������֪ͨ�¼�
        /// </summary>
        public DataArriveEvent OnClientDataArrive;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ���ص�ǰSocket
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }
        /// <summary>
        /// ��ȡ��ǰ���ӵı���TCP�˿�
        /// </summary>
        public int LocalPort
        {
            get { return ((IPEndPoint)socket.LocalEndPoint).Port; }
        }
        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public TCPManager()
        {
            socket = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp
                );
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~TCPManager()
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
            socket.Close();
            socket = null;
            base.Dispose(disposing);
        }
        #endregion

        #region protected void OnClientDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        /// <summary>
        /// �����յ��ͻ�������ʱ�������첽�ص�����
        /// </summary>
        /// <param name="tcpThread"></param>
        /// <param name="buffer"></param>
        protected void OnClientDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (OnClientDataArrive != null)
                OnClientDataArrive(tcpThread, buffer);
        }
        #endregion

        #region protected void OnAccept(IAsyncResult ar)
        /// <summary>
        /// ���������������ʱ�������첽�ص�����
        /// </summary>
        /// <param name="ar"></param>
        protected void OnAccept(IAsyncResult ar)
        {
            TCPThread tcpthread;
            Socket sock;
            try
            {
                sock = socket.EndAccept(ar);
                tcpthread = new TCPThread(sock);
                tcpthread.OnDataArrive += new DataArriveEvent(OnClientDataArriveEvent);
                tcpthread.BeginReceive();
            }
            catch
            {

            }
            finally
            {
                BeginAccept();
            }
        }
        #endregion

        #region public bool Create(int LocalPort)
        /// <summary>
        /// ����Socket
        /// </summary>
        /// <param name="LocalPort">����TCP�˿�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public bool Create(int LocalPort)
        {
            IPEndPoint _EP;
            try
            {
                _EP = new IPEndPoint(IPAddress.Any, LocalPort);
                socket.Bind(_EP);
                socket.Listen(0);
                return true;
            }
            catch
            {
                socket.Close();
                return false;
            }
        }
        #endregion

        #region public void Close()
        /// <summary>
        /// �ر�Socket����
        /// </summary>
        public void Close()
        {
            socket.Close();
        }
        #endregion

        #region public bool BeginAccept()
        /// <summary>
        /// ָʾSocket���Կ�ʼ��������������
        /// </summary>
        /// <returns>�����Ƿ�ɹ�</returns>
        public bool BeginAccept()
        {
            try
            {
                socket.BeginAccept(new AsyncCallback(OnAccept), this);
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region public bool EndAccept()
        /// <summary>
        /// ָʾSocketֹͣ������������
        /// </summary>
        /// <returns>�����Ƿ�ɹ�</returns>
        public bool EndAccept()
        {
            try
            {
                socket.EndAccept(null);
                return true;
            }
            catch { return false; }
        }
        #endregion

    }
}

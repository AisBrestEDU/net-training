/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPThread.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��36��
 * * �ļ���ʶ��795AF74A-746C-4C84-89BE-B98CB50837E2
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace codest.Net.Sockets
{
    /// <summary>
    /// TCP���ӿ�����
    /// </summary>
    public class TCPThread : BaseClass
    {
        #region ��Ա����
        /// <summary>
        /// Socket
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// �������ݵĻ�������С��Ϊ100K
        /// </summary>
        protected int BUFFER_SIZE = 1024 * 0xFF;
        /// <summary>
        /// �������ݵĻ�����
        /// </summary>
        protected byte[] _buffer;
        /// <summary>
        /// ָʾSocket�Ƿ�����
        /// </summary>
        private bool connected;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ָʾSocket�Ƿ�����
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }
        /// <summary>
        /// Socket
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }
        #endregion

        #region �����¼�
        /// <summary>
        /// ���ݵ�����¼�����
        /// </summary>
        public event DataArriveEvent OnDataArrive;
        /// <summary>
        /// �����ʳ���
        /// </summary>
        public event Action<int> OnError;
        /// <summary>
        /// �����ӱ��ر�
        /// </summary>
        public event NullParamEvent OnClose;
        #endregion

        #region ����/��������
        /// <summary>
        /// TCPThread���캯��������Ĭ��socket����
        /// </summary>
        /// <param name="sock">socket����</param>
        public TCPThread(Socket sock)
            :base()
        {
            socket = sock;
            connected = true;
        }
        /// <summary>
        /// TCPThread���캯��
        /// </summary>
        public TCPThread()
        {
            connected = false;
            _buffer = new byte[BUFFER_SIZE]; 
        }
        /// <summary>
        /// TCPThread��������
        /// </summary>
        ~TCPThread()
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
                _buffer = null;
            }
            //�ͷŷ��й���Դ
            if (connected) OnCloseEvent();
            socket = null;
            base.Dispose(disposing);
        }
        #endregion

        //--begin--���������¼�--

        #region protected void OnErrorEvent(int errNum)
        /// <summary>
        /// ��socket����
        /// </summary>
        /// <param name="errNum">������</param>
        protected void OnErrorEvent(int errNum)
        {
            connected = false;
            socket.Close();
            if (OnError != null) OnError(errNum);
        }
        #endregion

        #region protected void OnCloseEvent()
        /// <summary>
        /// socket�Ѿ��ر�
        /// </summary>
        protected void OnCloseEvent()
        {
            connected = false;
            socket.Close();
            if (OnClose != null) OnClose();
        }
        #endregion

        #region protected void OnDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        /// <summary>
        /// ���ݵ����¼�
        /// </summary>
        /// <param name="tcpThread">��ǰtcp�Ựsocket</param>
        /// <param name="buffer">����</param>
        protected void OnDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (OnDataArrive != null) OnDataArrive(tcpThread, buffer);
        }
        #endregion
        
        //--end----���������¼�--

        //--begin--�첽socket״̬����--

        #region protected void OnReceive(IAsyncResult ar)
        /// <summary>
        /// ���ݵ�����첽������
        /// </summary>
        /// <param name="ar"></param>
        protected void OnReceive(IAsyncResult ar)
        {
            int len;
            try
            {
                len = socket.EndReceive(ar);
            }
            catch (SocketException ex)
            {
                OnErrorEvent(ex.ErrorCode);
                return;
            }
            if (len == 0)
            {
                OnCloseEvent();
            }
            byte[] data = new byte[len];
            Array.Copy(_buffer, 0, data, 0, len);
            OnDataArriveEvent(this, data);
            BeginReceive();
        }
        #endregion

        #region protected void OnEndSend(IAsyncResult ar)
        /// <summary>
        /// �첽�����������
        /// </summary>
        /// <param name="ar"></param>
        protected void OnEndSend(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch (SocketException ex)
            {
                OnErrorEvent(ex.ErrorCode);
            }
        }
        #endregion

        //--end----�첽socket״̬����--

        #region public void BeginReceive()
        /// <summary>
        /// ָʾSocket���Կ�ʼ��������
        /// </summary>
        public void BeginReceive()
        {
            socket.BeginReceive(_buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(OnReceive), socket);
        }
        #endregion

        #region public virtual void Send(byte[] data)
        /// <summary>
        /// ���Ͷ���������
        /// </summary>
        /// <param name="data">��Ҫ���͵�����</param>
        public virtual void Send(byte[] data)
        {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnEndSend), socket);
        }
        #endregion

        #region public virtual void Send(string StringData)
        /// <summary>
        /// �����ַ�������
        /// </summary>
        /// <param name="StringData">��Ҫ���͵�����</param>
        public virtual void Send(string StringData)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(StringData);
            Send(data);
        }
        #endregion
    }
}

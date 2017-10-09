/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPClient.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��41��
 * * �ļ���ʶ��1126C195-03C7-4B46-9EC7-804837049ACE
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
    /// TCP�ͻ���
    /// </summary>
    public class TCPClient : TCPThread
    {
        #region �ӿڷ�װ
        /// <summary>
        /// ���ذ󶨵Ķ˿�
        /// </summary>
        private int LocalPort
        {
            get { return ((IPEndPoint)(base.socket.LocalEndPoint)).Port; }
        }

        /// <summary>
        /// ���ӵ�Զ�̶˿�
        /// </summary>
        private int RemotePort
        {
            get
            {
                if (base.Connected)
                    return ((IPEndPoint)base.socket.RemoteEndPoint).Port;
                else
                    return -1;
            }
        }

        /// <summary>
        /// ���ӵ�Զ��IP��ַ
        /// </summary>
        private string RemoteIP
        {
            get
            {
                if (base.Connected)
                    return ((IPEndPoint)base.socket.RemoteEndPoint).Address.ToString();
                else
                    return string.Empty;
            }
        }
        #endregion

        #region �����¼�
        /// <summary>
        /// ����������ӳɹ�
        /// </summary>
        public event Action<bool> OnConnect;

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public TCPClient()
            : base()
        {
            base.socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
                );
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~TCPClient()
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

        #region protected void OnConnectEvent(bool flag)
        /// <summary>
        /// Connect�ص�
        /// </summary>
        /// <param name="flag">�����Ƿ�ɹ�</param>
        protected void OnConnectEvent(bool flag)
        {
            if (OnConnect != null) OnConnect(flag);
        }
        #endregion

        #region protected void EndConnect(IAsyncResult ar)
        /// <summary>
        /// �첽Connect����
        /// </summary>
        /// <param name="ar"></param>
        protected void EndConnect(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                OnConnectEvent(true);
            }
            catch (SocketException ex)
            {
                OnConnectEvent(false);
                base.OnErrorEvent(ex.ErrorCode);
            }
        }
        #endregion

        #region public void Connect(string remoteAddress, int remotePort)
        /// <summary>
        /// ����Զ�̷�����
        /// </summary>
        /// <param name="remoteAddress">��������ַ</param>
        /// <param name="remotePort">�������˿�</param>
        public void Connect(string remoteAddress, int remotePort)
        {
            IPEndPoint remoteEP = new IPEndPoint(GetIPByHostName(remoteAddress), remotePort);
            socket.BeginConnect(remoteEP, new AsyncCallback(EndConnect), socket);
        }
        #endregion

        #region public bool Create(int LocalPort)
        /// <summary>
        /// ����socket
        /// </summary>
        /// <param name="LocalPort">���ض˿�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool Create(int LocalPort)
        {
            IPEndPoint _EP;
            try
            {
                _EP = new IPEndPoint(IPAddress.Any, LocalPort);
                socket.Bind(_EP);
                return true;
            }
            catch
            {
                socket.Close();
                return false;
            }
        }
        #endregion

        #region public static IPHostEntry GetHostNameByIP(string ipAddress)
        /// <summary>
        /// ͨ��IP����������
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static IPHostEntry GetHostNameByIP(string ipAddress)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);
            return hostInfo;
        }
        #endregion

        #region public static IPAddress GetIPByHostName(string hostName)
        /// <summary>
        /// ͨ������������IP��ַ
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static IPAddress GetIPByHostName(string hostName)
        {
            IPAddress[] addrList = Dns.GetHostAddresses(hostName);
            return addrList[0];
        }
        #endregion


    }
}

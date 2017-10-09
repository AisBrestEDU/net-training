/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPClient.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分41秒
 * * 文件标识：1126C195-03C7-4B46-9EC7-804837049ACE
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace codest.Net.Sockets
{
    /// <summary>
    /// TCP客户端
    /// </summary>
    public class TCPClient : TCPThread
    {
        #region 接口封装
        /// <summary>
        /// 本地绑定的端口
        /// </summary>
        private int LocalPort
        {
            get { return ((IPEndPoint)(base.socket.LocalEndPoint)).Port; }
        }

        /// <summary>
        /// 连接的远程端口
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
        /// 连接的远程IP地址
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

        #region 公共事件
        /// <summary>
        /// 当请求的连接成功
        /// </summary>
        public event Action<bool> OnConnect;

        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
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
        /// 析构函数
        /// </summary>
        ~TCPClient()
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

        #region protected void OnConnectEvent(bool flag)
        /// <summary>
        /// Connect回调
        /// </summary>
        /// <param name="flag">连接是否成功</param>
        protected void OnConnectEvent(bool flag)
        {
            if (OnConnect != null) OnConnect(flag);
        }
        #endregion

        #region protected void EndConnect(IAsyncResult ar)
        /// <summary>
        /// 异步Connect结束
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
        /// 连接远程服务器
        /// </summary>
        /// <param name="remoteAddress">服务器地址</param>
        /// <param name="remotePort">服务器端口</param>
        public void Connect(string remoteAddress, int remotePort)
        {
            IPEndPoint remoteEP = new IPEndPoint(GetIPByHostName(remoteAddress), remotePort);
            socket.BeginConnect(remoteEP, new AsyncCallback(EndConnect), socket);
        }
        #endregion

        #region public bool Create(int LocalPort)
        /// <summary>
        /// 创建socket
        /// </summary>
        /// <param name="LocalPort">本地端口</param>
        /// <returns>是否成功</returns>
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
        /// 通过IP查找主机名
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
        /// 通过主机名查找IP地址
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

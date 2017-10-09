/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分43秒
 * * 文件标识：3A5149DB-F43C-4651-B708-FF9022AFF8FA
 * * 内容摘要：
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
    /// 数据到达进行处理的函数的委托
    /// </summary>
    /// <param name="tcpThread">当前tcp会话对象</param>
    /// <param name="buffer">数据</param>
    public delegate void DataArriveEvent(TCPThread tcpThread, byte[] buffer);
    #endregion 
    
    /// <summary>
    /// 提供TCP连接服务端的类
    /// </summary>
    public class TCPManager : BaseClass
    {
        #region 成员变量
        private Socket socket;
        #endregion

        #region 公共事件
        /// <summary>
        /// 客户端数据通知事件
        /// </summary>
        public DataArriveEvent OnClientDataArrive;
        #endregion

        #region 接口封装
        /// <summary>
        /// 返回当前Socket
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }
        /// <summary>
        /// 获取当前连接的本地TCP端口
        /// </summary>
        public int LocalPort
        {
            get { return ((IPEndPoint)socket.LocalEndPoint).Port; }
        }
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
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
        /// 析构函数
        /// </summary>
        ~TCPManager()
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
            socket.Close();
            socket = null;
            base.Dispose(disposing);
        }
        #endregion

        #region protected void OnClientDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        /// <summary>
        /// 当有收到客户端数据时，触发异步回调函数
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
        /// 当有连接请求接入时，触发异步回调函数
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
        /// 创建Socket
        /// </summary>
        /// <param name="LocalPort">本地TCP端口</param>
        /// <returns>创建是否成功</returns>
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
        /// 关闭Socket连接
        /// </summary>
        public void Close()
        {
            socket.Close();
        }
        #endregion

        #region public bool BeginAccept()
        /// <summary>
        /// 指示Socket可以开始处理连接请求了
        /// </summary>
        /// <returns>调用是否成功</returns>
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
        /// 指示Socket停止处理连接请求
        /// </summary>
        /// <returns>调用是否成功</returns>
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

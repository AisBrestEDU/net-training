/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPThread.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分36秒
 * * 文件标识：795AF74A-746C-4C84-89BE-B98CB50837E2
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
    /// TCP连接控制类
    /// </summary>
    public class TCPThread : BaseClass
    {
        #region 成员变量
        /// <summary>
        /// Socket
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// 接收数据的缓冲区大小，为100K
        /// </summary>
        protected int BUFFER_SIZE = 1024 * 0xFF;
        /// <summary>
        /// 接收数据的缓冲区
        /// </summary>
        protected byte[] _buffer;
        /// <summary>
        /// 指示Socket是否连接
        /// </summary>
        private bool connected;
        #endregion

        #region 接口封装
        /// <summary>
        /// 指示Socket是否连接
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

        #region 公共事件
        /// <summary>
        /// 数据到达的事件处理
        /// </summary>
        public event DataArriveEvent OnDataArrive;
        /// <summary>
        /// 当访问出错
        /// </summary>
        public event Action<int> OnError;
        /// <summary>
        /// 当连接被关闭
        /// </summary>
        public event NullParamEvent OnClose;
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// TCPThread构造函数，传入默认socket连接
        /// </summary>
        /// <param name="sock">socket连接</param>
        public TCPThread(Socket sock)
            :base()
        {
            socket = sock;
            connected = true;
        }
        /// <summary>
        /// TCPThread构造函数
        /// </summary>
        public TCPThread()
        {
            connected = false;
            _buffer = new byte[BUFFER_SIZE]; 
        }
        /// <summary>
        /// TCPThread析构函数
        /// </summary>
        ~TCPThread()
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
                _buffer = null;
            }
            //释放非托管资源
            if (connected) OnCloseEvent();
            socket = null;
            base.Dispose(disposing);
        }
        #endregion

        //--begin--触发公共事件--

        #region protected void OnErrorEvent(int errNum)
        /// <summary>
        /// 当socket出错
        /// </summary>
        /// <param name="errNum">错误编号</param>
        protected void OnErrorEvent(int errNum)
        {
            connected = false;
            socket.Close();
            if (OnError != null) OnError(errNum);
        }
        #endregion

        #region protected void OnCloseEvent()
        /// <summary>
        /// socket已经关闭
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
        /// 数据到达事件
        /// </summary>
        /// <param name="tcpThread">当前tcp会话socket</param>
        /// <param name="buffer">数据</param>
        protected void OnDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (OnDataArrive != null) OnDataArrive(tcpThread, buffer);
        }
        #endregion
        
        //--end----触发公共事件--

        //--begin--异步socket状态处理--

        #region protected void OnReceive(IAsyncResult ar)
        /// <summary>
        /// 数据到达的异步处理函数
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
        /// 异步发送数据完成
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

        //--end----异步socket状态处理--

        #region public void BeginReceive()
        /// <summary>
        /// 指示Socket可以开始接收数据
        /// </summary>
        public void BeginReceive()
        {
            socket.BeginReceive(_buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(OnReceive), socket);
        }
        #endregion

        #region public virtual void Send(byte[] data)
        /// <summary>
        /// 发送二进制数据
        /// </summary>
        /// <param name="data">需要发送的数据</param>
        public virtual void Send(byte[] data)
        {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnEndSend), socket);
        }
        #endregion

        #region public virtual void Send(string StringData)
        /// <summary>
        /// 发送字符串数据
        /// </summary>
        /// <param name="StringData">需要发送的数据</param>
        public virtual void Send(string StringData)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(StringData);
            Send(data);
        }
        #endregion
    }
}

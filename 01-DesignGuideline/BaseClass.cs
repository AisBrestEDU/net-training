/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�BaseClass.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��17��
 * * �ļ���ʶ��F78FB93F-3459-4CF8-82CB-4BEF90B3CEAA
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Text;
using System.IO;
using System.ComponentModel;
namespace codest
{
    #region public delegate void NullParamEvent();
    /// <summary>
    /// �޲������޷���ֵ���й�����
    /// </summary>
    public delegate void NullParamEvent();
    #endregion

    /// <summary>
    /// Icyplayer������Ļ���
    /// ʵ��IDisposable, ICloneable�ӿ�
    /// </summary>
    public abstract class BaseClass : IDisposable, ICloneable
    {
        #region ��Ա����
        /// <summary>
        /// �й���Դ����
        /// </summary>
        private Container components = null; 
        /// <summary>
        /// ָʾ�����Ƿ��Ѿ�����
        /// </summary>
        protected bool disposed = false;
        #endregion

        #region ����/��������
        /// <summary>
        /// BaseClass���캯��
        /// </summary>
        public BaseClass()
        { 
        }
        /// <summary>
        /// BaseClass��������
        /// </summary>
        ~BaseClass()
        {
            Dispose(false);
        }
        #endregion

        #region protected virtual void Dispose(bool disposing)
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="disposing">false��ϵͳ���ã�true���ֶ�����</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //�ͷ��й���Դ
                if (components != null)
                {
                    components.Dispose();
                }
                disposed = true;
            }
            //�ͷŷ��й���Դ
            //base.Dispose(disposing);
        }
        #endregion

        #region ICloneable ��Ա
        /// <summary>
        /// ��ɶ����ǳ����
        /// </summary>
        /// <returns>����ĸ���</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region IDisposable ��Ա
        /// <summary>
        /// ��ʽ��������
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }


}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;

namespace RTClient
{
    public class CObjectFactory
    {
        /// <summary>
        /// 存储Assembly对象
        /// </summary>
        private Hashtable m_AssemblyCache;
        public Hashtable AssemblyCache
        {
            get
            {
                if (this.m_AssemblyCache == null)
                    this.m_AssemblyCache = new Hashtable();
                return m_AssemblyCache;
            }
            set { m_AssemblyCache = value; }
        }

        private CObjectFactory() { }

        /// <summary>
        /// 唯一静态实例
        /// </summary>
        private static CObjectFactory m_Instance;
        public static CObjectFactory Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new CObjectFactory();
                return CObjectFactory.m_Instance;
            }
        }

        /// <summary>
        /// 按给定的类名创建实例
        /// </summary>
        /// <param name="name">完整的类名（包括名字空间）</param>
        /// <returns></returns>
        public object CreateObject(string name, string fileKind)
        {
            Assembly asm = null;
            if (fileKind == "DLL")
            {
                asm = Assembly.LoadFrom(name.Substring(0, name.LastIndexOf('.') + 1) + "dll");
            }
            else
            {
                asm = Assembly.LoadFrom(name.Substring(0, name.LastIndexOf('.') + 1) + "exe");
            }
            object obj = null;
            Type objType = asm.GetType(name);
            if (objType != null)
                obj = Activator.CreateInstance(objType);
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkStation.FunClass
{
    public class SecurityQueue<T>
    {
        private readonly List<T> _operations;
        private object objLock = new object();
        public SecurityQueue()
        {
            _operations = new List<T>();
        }

        /// <summary>
        /// 将对象添加到 CustomQueue 的结尾处。
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            lock (objLock)
            {
                _operations.Add(item);
            }
        }

        /// <summary>
        /// 移除并返回位于 CustomQueue 开始处的对象。
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            lock (objLock)
            {
                if (_operations.Count > 0)
                {
                    T item = _operations[0];
                    _operations.RemoveAt(0);
                    return item;
                }
                return default(T);
            }
        }

        /// <summary>
        /// 返回位于 CustomQueue 开始处的对象但不将其移除。
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            lock (objLock)
            {
                if (_operations.Count > 0)
                {
                    return _operations[0];
                }
                return default(T);
            }
        }

        /// <summary>
        /// 包含从 Queue 复制的元素的新数组。
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            lock (objLock)
            {
                return _operations.ToArray();
            }
        }

        /// <summary>
        ///  从 Queue 中移除所有对象。
        /// </summary>
        public void Clear()
        {
            lock (objLock)
            {
                _operations.Clear();
            }
        }
        /// <summary>
        /// 确定某元素是否在 Queue 中。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            lock (objLock)
            {
                return _operations.Contains(item);
            }
        }

        /// <summary>
        /// 确定某元素是否在 Queue 中。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Exists(Predicate<T> match)
        {
            lock (objLock)
            {
                return _operations.Exists(match);
            }
        }

        /// <summary>
        ///  获取 CustomQueue 中包含的元素数。
        /// </summary>
        public int Count
        {
            get
            {
                return _operations.Count;
            }
        }

        /// <summary>
        /// 插入到首位置
        /// </summary>
        /// <param name="buffer"></param>
        public void Push(T item)
        {
            lock (objLock)
            {
                _operations.Insert(0, item);
            }
        }

    }
}

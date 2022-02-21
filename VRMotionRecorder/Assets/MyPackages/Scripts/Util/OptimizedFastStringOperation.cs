/**
The MIT License (MIT)

Copyright (c) 2016 Yusuke Kurokawa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
using System.Threading;
using System.Text;

namespace StringOperationUtil
{
    /// <summary>
    /// Using this,you can optimize string concat operation easily.
    /// To use this , you should put this on the top of code.
    /// ------
    /// using StrOpe = StringOperationUtil.OptimizedFastStringOperation;
    /// ------
    /// 
    /// - before code
    /// string str = "aaa" + 20 + "bbbb";
    /// 
    /// - after code
    /// string str = StrOpe.i + "aaa" + 20 + "bbbb";
    /// 
    /// "StrOpe.i" is for MainThread , do not call from other theads.
    /// If "StrOpe.i" is called from Mainthread , reuse same object.
    /// 
    /// You can also use "StrOpe.small" / "StrOpe.medium" / "StrOpe.large" instead of "StrOpe.i". 
    /// These are creating instance.
    /// </summary>
    public class OptimizedFastStringOperation
    {
        private static OptimizedFastStringOperation instance = null;
        #if !UNITY_WEBGL
        private static Thread singletonThread = null;
        #endif
        private FastString fs = null;

        static OptimizedFastStringOperation()
        {
            instance = new OptimizedFastStringOperation(1024);
        }
        private OptimizedFastStringOperation(int capacity)
        {
            fs = new FastString(capacity);
        }

        public static OptimizedFastStringOperation Create(int capacity)
        {
            return new OptimizedFastStringOperation(capacity);
        }

        public static OptimizedFastStringOperation small
        {
            get
            {
                return Create(64);
            }
        }

        public static OptimizedFastStringOperation medium
        {
            get
            {
                return Create(256);
            }
        }
        public static OptimizedFastStringOperation large
        {
            get
            {
                return Create(1024);
            }
        }

        public static OptimizedFastStringOperation i
        {
            get
            {
                #if !UNITY_WEBGL
                // Bind instance to thread.
                if (singletonThread == null )
                {
                    singletonThread = Thread.CurrentThread;
                }
                // check thread...
                if (singletonThread != Thread.CurrentThread)
                {
                    #if DEBUG || UNITY_EDITOR
                    UnityEngine.Debug.LogError("Execute from another thread.");
                    #endif
                    return small;
                }
                #endif
                instance.fs.Clear();
                return instance;
            }
        }

        public int Length
        {
            get { return this.fs.Length; }
        }

        public OptimizedFastStringOperation Replace(string oldValue, string newValue)
        {
            fs.Replace(oldValue, newValue);
            return this;
        }

        public override string ToString()
        {
            return fs.ToString();
        }

        public void Clear()
        {
            // StringBuilder.Clear() doesn't support .Net 3.5...
            // "Capasity = 0" doesn't work....
            fs = new FastString(0);
        }

        public static implicit operator string(OptimizedFastStringOperation t)
        {
            return t.ToString();
        }

        #region ADD_OPERATOR
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, bool v)
        {
            t.fs.Append(v);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, int v)
        {
            t.fs.Append(v);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, short v)
        {
            t.fs.Append(v);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, byte v)
        {
            t.fs.Append(v);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, float v)
        {
            t.fs.Append(v);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, char c)
        {
            t.fs.Append(c);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, char[] c)
        {
            t.fs.Append(c);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, string str)
        {
            t.fs.Append(str);
            return t;
        }
        public static OptimizedFastStringOperation operator +(OptimizedFastStringOperation t, FastString fs)
        {
            t.fs.Append(fs);
            return t;
        }
        #endregion ADD_OPERATOR
    }
}
using System;
using System.IO;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        static readonly string BASEDIRECTORY = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "日志");

        #region 同步锁
        static object errorLocker = new object();
        static object normalInfoLocker = new object();
        #endregion

        /// <summary>
        /// 记录日志
        /// GBK编码
        /// </summary>
        static void DoLog(string log, string dirName, object locker)
        {
            try
            {
                lock (locker)
                {
                    string dirPath = Path.Combine(BASEDIRECTORY, dirName);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string filename = Path.Combine(dirPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
                    using (FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GBK")))
                        {
                            sw.Write(string.Format("{0}：{1}{2}", DateTime.Now.ToString("HH:mm:ss"), log, Environment.NewLine));
                            sw.Write(Environment.NewLine);
                            sw.Flush();
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        public static void DoErrorLog(string errorMessage)
        {
            DoLog(errorMessage, "异常信息", errorLocker);
        }

        /// <summary>
        /// 记录普通信息日志
        /// </summary>
        public static void DoNormalLog(string message)
        {
            DoLog(message, "普通信息", normalInfoLocker);
        }
    }
}

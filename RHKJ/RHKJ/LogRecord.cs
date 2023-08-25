using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace RHKJ
{
    public class LogRecord
    {
        static ReaderWriterLock logWriteLock1 = new ReaderWriterLock();
        
        public static void WriteOperLog(string strlog)
        {
            try
            {
                logWriteLock1.AcquireWriterLock(200);
                string sFilePath = "D:\\运行日志\\操作日志\\" + DateTime.Now.ToString("yyyMM") + "操作日志";
                string sFileName = sFilePath + "\\" + "操作日志" + DateTime.Now.ToString("dd") + ".log";
                
                if(!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                FileStream fs;
                StreamWriter sw;
                if(File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(string.Format("{0}\t{1,16}\t", DateTime.Now.ToString() + "-----***-----", strlog));
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                logWriteLock1.ReleaseWriterLock();
            }
        }
    }
}

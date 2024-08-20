using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using BaseModel;
using System.Windows.Forms;
using System.Collections;
using BaseModel.Logs;
using System.Net;
using System.Threading;

namespace WorkStation
{
    public class CWorkFlowControlHelper
    {

        #region 唯一静态实例
        private CWorkFlowControlHelper()
        {
        }

        private static CWorkFlowControlHelper m_Instance;
        public static CWorkFlowControlHelper Instance
        {
            get
            {
                lock (typeof(CWorkFlowControlHelper))
                {
                    if (m_Instance == null)
                        m_Instance = new CWorkFlowControlHelper();
                }
                return m_Instance;
            }
        }
        #endregion

        #region 遍历目录下所有文件
        /// <summary>
        /// 遍历目录下所有文件  *.txt
        /// </summary>
        /// <param name="dir">路径</param>
        /// <param name="files">文件集合</param>
        /// <param name="FileType">文件类型(*.csv)|*.csv|(*.XLS)|*.XLS|(*.XLSX)|*.XLSX/*.txt</param>
        public static List<string> GetDirFiles(string dir, List<string> files, string FileType)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = di.GetFiles(FileType);//文件类型
            foreach (FileInfo fi in fis)
            {
                files.Add(fi.FullName);
            }
            return files;
        }

        /// <summary>
        /// 遍历目录下所有文件 炉温文件上传  文件最后修改时间间隔x小时
        /// </summary>
        /// <param name="dir">路径</param>
        /// <param name="files">文件集合</param>
        /// <param name="FileType">文件类型(*.csv)|*.csv|(*.XLS)|*.XLS|(*.XLSX)|*.XLSX/*.txt</param>
        /// <param name="IntervalHour">时间</param>
        public static List<string> GetDirFiles(string dir, List<string> files, string FileType, int IntervalHour)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] fis = di.GetFiles(FileType);//文件类型
            foreach (FileInfo fi in fis)
            {
                DateTime lastdt = fi.LastWriteTime;
                DateTime now = DateTime.Now;
                if (lastdt.AddHours(IntervalHour) < now)
                {
                    files.Add(fi.FullName);
                }
            }
            return files;
        }

        /// <summary>
        /// 递归获取文件
        /// </summary>
        /// <param name="dir">路径</param>
        /// <param name="files">存放文件路径集合</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="IntervalHour">时间</param>
        /// <returns></returns>
        public static List<string> GetDirFilesRecursion(string dir, List<string> files, string fileType)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] _files = di.GetFiles();//文件
            FileInfo[] fis = di.GetFiles(fileType);//文件类型
            foreach (FileInfo fi in _files)
            {
                files.Add(fi.FullName);//添加全路径文件名到列表中 
            }
            //获取子文件夹内的文件列表，递归遍历  
            DirectoryInfo[] directs = di.GetDirectories();//文件夹
            foreach (DirectoryInfo dd in directs)
            {
                GetDirFilesRecursion(dd.FullName, files, fileType);
            }
            return files;
        }

        /// <summary>
        /// 递归获取文件
        /// </summary>
        /// <param name="dir">路径</param>
        /// <param name="files">存放文件路径集合</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="IntervalHour">时间</param>
        /// <returns></returns>
        public static List<string> GetDirFilesRecursion(string dir, List<string> files, string fileType, int IntervalHour)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] _files = di.GetFiles();//文件
            FileInfo[] fis = di.GetFiles(fileType);//文件类型
            foreach (FileInfo fi in _files)
            {
                DateTime lastdt = fi.LastWriteTime;
                DateTime now = DateTime.Now;
                if (lastdt.AddHours(IntervalHour) < now)
                {
                    files.Add(fi.FullName);//添加全路径文件名到列表中 
                }
            }
            //获取子文件夹内的文件列表，递归遍历  
            DirectoryInfo[] directs = di.GetDirectories();//文件夹
            foreach (DirectoryInfo dd in directs)
            {
                GetDirFilesRecursion(dd.FullName, files, fileType, IntervalHour);
            }
            return files;
        }
        #endregion

        #region 访问接口
        /// <summary>
        /// 调用上传接口
        /// </summary>
        /// <param name="url">接口</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="uploadPath">上传路径</param>
        /// <returns></returns>
        public static string UploadRequest(string url,string fileName, string filePath, string uploadPath)
        {
            string returnValue = "";
            // 时间戳，用做boundary
            string timeStamp = DateTime.Now.Ticks.ToString("x");

            //SSL/TLS 安全通道设置
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ;  //4.5版本
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;                //4.5以下版本

            //根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpReq.Method = "POST";
            httpReq.AllowWriteStreamBuffering = false; //对发送的数据不使用缓存
            httpReq.Timeout = 300000;  //设置获得响应的超时时间（300秒）
            httpReq.ContentType = "multipart/form-data; boundary=" + timeStamp;

            //文件
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            //头信息文件名称
            string boundary = "--" + timeStamp;
            string dataFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n";
            string header = string.Format(dataFormat, "file", fileName);//Path.GetFileName(filePath)
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(header);

            //写入参数
            string para = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            string spt = string.Format(para, "servicePath", uploadPath);
            byte[] postParaBytes = Encoding.UTF8.GetBytes(spt);
            //====================

            //结束边界
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + timeStamp + "--\r\n");

            long length = fileStream.Length + postHeaderBytes.Length + boundaryBytes.Length + postParaBytes.Length;
            httpReq.ContentLength = length;//请求内容长度

            try
            {
                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数
                long offset = 0;
                int size = binaryReader.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();

                //发送参数
                postStream.Write(postParaBytes, 0, postParaBytes.Length);
                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    size = binaryReader.Read(buffer, 0, bufferLength);
                }

                //添加尾部边界
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应
                using (HttpWebResponse response = (HttpWebResponse)httpReq.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    returnValue = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                return "文件传输异常：" + ex.Message;
            }
            finally
            {
                fileStream.Close();
                binaryReader.Close();
            }
            return returnValue;
        }
        #endregion

        #region 弹出框
        #region MessageBox
        /// <summary>
        /// 给消息框加上标题。
        /// </summary>
        /// <param name="msg"></param>
        public static void MsgMessageBox(string msg,string title)
        {
            Thread th = new Thread(new ThreadStart(() =>
            {
                MessageBox.Show(msg, title);
            }));
            th.Start();
        }
        #endregion

        #endregion

        /// <summary>
        /// 文件转移
        /// </summary>
        /// <param name="path">备份路径</param>
        /// <param name="file">文件全路径</param>
        public static void BackUpFile(string path, string file)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string newPath = Path.Combine(path, DateTime.Now.ToString("yyMMddHHmmss") + "_" + Path.GetFileName(file));
            File.Move(file, newPath);
        }
    }
}

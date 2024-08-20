using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace WorkStation.FunClass
{
    /// <summary>
    /// 文件传输服务，用于从服务器的共享文件夹上上传下载文件
    /// </summary>
    public class FileTransferService
    {
        //private readonly ILog4NetService _log4NetService;

        //public FileTransferService(ILog4NetService log4NetService)
        //{
        //    _log4NetService = log4NetService;
        //}

        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        private OperateResult ConnectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return new OperateResult() { IsSuccess = Flag };
        }


        /// <summary>
        /// 上传下载文件
        /// </summary>
        /// <param name="destPath">源文件路径</param>
        /// <param name="sourcePath">目标文件路径</param>
        /// <param name="serverPath">服务器路径</param>
        /// <param name="userName">访问服务器用户名</param>
        /// <param name="password">访问服务器密码</param>
        /// <returns></returns>
        public OperateResult TransferFileFromServer(string sourcePath, string destPath, string serverPath, string userName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(destPath) || string.IsNullOrEmpty(sourcePath))
                {
                    return OperateResult.CreateFailResult("文件路径不能为空！！");
                }

                var shareDir = Path.GetDirectoryName(serverPath);
                if (string.IsNullOrEmpty(shareDir))
                {
                    return OperateResult.CreateFailResult("未找到目标路径目录！！");
                }
                //连接共享文件夹
                var statusResult = ConnectState(shareDir, userName, password);
                if (statusResult.IsSuccess)
                {
                    var inFileStream = new FileStream(sourcePath, FileMode.Open , FileAccess.Read);
                    var outFileStream = new FileStream(destPath + Path.GetFileName(sourcePath), FileMode.OpenOrCreate);
                    byte[] buf = new byte[inFileStream.Length];
                    int byteCount;
                    while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        outFileStream.Write(buf, 0, byteCount);
                    }
                    inFileStream.Flush();
                    inFileStream.Close();
                    outFileStream.Flush();
                    outFileStream.Close();
                    return OperateResult.CreateSuccessResult();
                }
                return OperateResult.CreateFailResult("连接到服务器失败: " + statusResult.Message);
            }
            catch (Exception ex)
            {
                //_log4NetService.WriteLog(LogNameCustom.SystemError, ex);
                return OperateResult.CreateFailResult(ex.ToString());
            }
        }
    }
}

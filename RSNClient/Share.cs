using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseModel;
using System.IO;
using System.Diagnostics;
using System.Threading;
using WorkStation;

namespace RTClient
{
    public partial class Share : Form
    {
        /// <summary>
        /// WorkStation.ini 配置文件路径
        /// </summary>
        string wsIniPath = Application.StartupPath + @"\Config\WorkStation.ini";
        /// <summary>
        /// 连接状态
        /// </summary>
        private bool linkStatus = false;

        public Share()
        {
            InitializeComponent();

            btnLink.Click += new EventHandler(btnLink_Click);
            tbReadPath.Click += new EventHandler(tbReadPath_Click);
            tbBackPath.Click += new EventHandler(tbBackPath_Click);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);

            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);

            InitData();
        }

        #region InitData
        public void InitData()
        {
            tbSharePath.Text = IniFileHelper.ReadIniData("ShareNet", "SharePath", "", wsIniPath);
            tbShareUserName.Text = IniFileHelper.ReadIniData("ShareNet", "ShareUserName", "", wsIniPath);
            tbSharePassWord.Text = IniFileHelper.ReadIniData("ShareNet", "SharePassWord", "", wsIniPath);
            tbReadRate.Text = IniFileHelper.ReadIniData("ShareNet", "Satt", "", wsIniPath);
            tbReadPath.Text = IniFileHelper.ReadIniData("ShareNet", "ReadPath", "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("ShareNet", "BackPath", "", wsIniPath);
        }
        #endregion

        #region 读备路径
        private void tbReadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbReadPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("ShareNet", "ReadPath", this.tbReadPath.Text, wsIniPath);
            }
        }

        private void tbBackPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个备份文件夹";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbBackPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("ShareNet", "BackPath", this.tbBackPath.Text, wsIniPath);
            }
        }
        #endregion

        #region 测试连接
        private void btnLink_Click(object sender, EventArgs e)
        {
            bool status = false;
            string path = tbSharePath.Text;
            string userName = tbShareUserName.Text;
            string passWord = tbSharePassWord.Text;
            status = connectState(path, userName, passWord);
            if (status)
                MessageBox.Show(path + "  连接成功！");
            else
                MessageBox.Show(path + "  连接失败！");
        }
        #endregion

        #region btnStart_Click
        private void btnStart_Click(object sender, EventArgs e)
        {
            if ("".Equals(tbReadPath.Text))
            {
                MessageBox.Show("未设置读取目录！");
                return;
            }
            if ("".Equals(tbBackPath.Text))
            {
                MessageBox.Show("未设置备份目录！");
                return;
            }

            int readpl = 5;
            int.TryParse(tbReadRate.Text, out readpl);
            if (readpl == 0)
                readpl = 5;
            timer1.Enabled = true;
            timer2.Interval = readpl * 1000;
            timer2.Enabled = true;
            SetEnable();
        }
        #endregion

        #region btnEnd_Click
        private void btnEnd_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            lblState.Text = "系统监听已停止...";
            lblStateTitle.ForeColor = Color.Red;
            lblState.ForeColor = Color.Red;
            SetEnable();
        }
        #endregion

        #region SetEnable()
        private void SetEnable()
        {
            tbReadPath.Enabled = !tbReadPath.Enabled;
            tbBackPath.Enabled = !tbBackPath.Enabled;
            btnEnd.Enabled = !btnEnd.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
            tbReadRate.ReadOnly = !tbReadRate.ReadOnly;
        }
        #endregion

        #region timer1_Tick
        int iTimerFlag = 0;
        string iTimerTitle = "系统正在监听,任务执行中...";
        private void timer1_Tick(object sender, EventArgs e)
        {
            iTimerFlag++;
            if (iTimerFlag > 15)
            {
                iTimerFlag = 1;
            }
            lblState.Text = iTimerTitle.Substring(0, iTimerFlag);
        }
        #endregion

        #region timer2_Tick
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            bool isTrue = false;
            isTrue = MachineDataRead();
            if (!isTrue)
                btnEnd_Click(null, null);
            timer2.Enabled = isTrue;
        }
        #endregion

        #region  业务处理 MachineDataRead()
        private bool MachineDataRead()
        {
            if (!linkStatus)
            {
                string path = tbSharePath.Text;
                string userName = tbShareUserName.Text;
                string passWord = tbSharePassWord.Text;
                linkStatus = connectState(path, userName, passWord);
                if (!linkStatus)
                    return false;
            }

            if (listBoxMsg.Items.Count > 50)
                listBoxMsg.Items.Clear();

            int tt = 5;
            int.TryParse(tbReadRate.Text, out tt);
            Thread.Sleep(tt * 1000);
            //获取路径下所以文件
            List<string> files = new List<string>();
            files = CWorkFlowControlHelper.GetDirFilesRecursion(tbReadPath.Text, files, "*.*");
            if (files.Count <= 0)
            {
                listBoxMsg.Items.Insert(0, "没有可上传的文件！！！");
                return true;
            }

            //共享文件夹的目录
            DirectoryInfo theFolder = new DirectoryInfo(tbSharePath.Text);

            foreach (string file in files)
            {
                string filePath = file;
                string fileName = Path.GetFileName(filePath);

                //获取指定文件夹下到文本之间的路径
                // C:\Users\Administrator\Desktop\test\12A\QQQ.txt  取\12A\
                string midPath = filePath.Replace(tbReadPath.Text, "");
                midPath = midPath.Replace(fileName, "");
                //相对共享文件夹的路径
                string shareFileName = "";
                if (midPath.Length > 1)
                    shareFileName = @"\" + midPath;
                else
                    shareFileName = @"\";

                //获取保存文件的路径
                string filename = theFolder.ToString() + shareFileName;
                //执行方法
                Transport(filePath, filename, fileName);
                
                //上传成功后开始备份转移
                #region 文件名称变更
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string newBackPath = tbBackPath.Text + @"\" + year + @"\" + month;
                if (midPath.Length > 1)
                {
                    newBackPath = newBackPath + midPath;
                }
                if (!Directory.Exists(newBackPath))
                    Directory.CreateDirectory(newBackPath);

                string newPath = Path.Combine(newBackPath, Path.GetFileName(file));
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                File.Move(file, newPath);
                #endregion

                //删除文件
                //File.Delete(filePath);

                KillEmptyFolder(tbReadPath.Text);
                listBoxMsg.Items.Insert(0, "上传成功  " + "  Path:" + fileName);

            }

            return true;
        }
        #endregion

        #region 连接远程共享文件夹
        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
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
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
        #endregion

        #region 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// <summary>
        /// 向远程文件夹保存本地内容，或者从远程文件夹下载文件到本地
        /// </summary>
        /// <param name="src">要保存的文件的路径，如果保存文件到共享文件夹，这个路径就是本地文件路径如：@"D:\1.avi"</param>
        /// <param name="dst">保存文件的路径，不含名称及扩展名</param>
        /// <param name="fileName">保存文件的名称以及扩展名</param>
        public static void Transport(string src, string dst, string fileName)
        {
            try
            {
                FileStream inFileStream = new FileStream(src, FileMode.Open);
                if (!Directory.Exists(dst))
                {
                    Directory.CreateDirectory(dst);
                }
                dst = dst + fileName;
                FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + "\r\n" + dst  + "\r\n" + ex.ToString());
                throw;
            }

        }
        #endregion

        #region 删除掉空文件夹
        /// <summary>
        /// 删除掉空文件夹,所有没有子“文件系统”的都将被删除
        /// </summary>
        /// <param name="sPath">路径</param>
        private void KillEmptyFolder(string sPath)
        {
            if (Directory.Exists(sPath))
            {
                DirectoryInfo info = new DirectoryInfo(sPath);

                foreach (string item in Directory.GetFileSystemEntries(sPath, "*", SearchOption.AllDirectories))
                {
                    //如果是文件夹
                    if (!File.Exists(item))
                    {
                        //如果文件夹存在
                        if (Directory.Exists(item))
                        {
                            //取得文件夹下所有文件
                            string[] nFiles = Directory.GetFiles(item, "*.*", SearchOption.AllDirectories);
                            //如果文件个数为0则删除目录及子目录
                            if (nFiles.Count() == 0)
                            {
                                Directory.Delete(item, true);
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }
}

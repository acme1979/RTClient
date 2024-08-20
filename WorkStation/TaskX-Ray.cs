using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseModel;
using System.Collections;
using WorkStation.FunClass;
using System.IO;
using System.Management;
using System.Diagnostics;
using System.Threading;
using BaseModel.Logs;

namespace WorkStation
{
    public partial class TaskX_Ray : CUserControl, ICommonWorkStationCtl
    {
        #region 构造函数与成员  get、set
        private string m_PO;
        /// <summary>
        /// 当前生产单据号
        /// </summary>
        public string PO
        {
            get { return m_PO; }
            set { m_PO = value; }
        }

        private string m_Process;
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        public string Process
        {
            get { return m_Process; }
            set { m_Process = value; }
        }

        private string m_ProcessName;
        /// <summary>
        /// 当前生产工序代码
        /// </summary>
        public string ProcessName
        {
            get { return m_ProcessName; }
            set { m_ProcessName = value; }
        }

        private string m_ProductLine;
        /// <summary>
        /// 线别
        /// </summary>
        public string ProductLine
        {
            get { return m_ProductLine; }
            set { m_ProductLine = value; }
        }

        private string m_ProductLineName;
        /// <summary>
        /// 线别
        /// </summary>
        public string ProductLineName
        {
            get { return m_ProductLineName; }
            set { m_ProductLineName = value; }
        }
        private string m_WorkStation;
        /// <summary>
        /// 当前生产工站代码
        /// </summary>
        public string WorkStation
        {
            get { return m_WorkStation; }
            set { m_WorkStation = value; }
        }

        private string m_WorkStationName;
        /// <summary>
        /// 当前生产工站名称
        /// </summary>
        public string WorkStationName
        {
            get { return m_WorkStationName; }
            set { m_WorkStationName = value; }
        }

        private string m_UsrCode;
        /// <summary>
        /// 当前用户工号
        /// </summary>
        public string UsrCode
        {
            get { return m_UsrCode; }
            set { m_UsrCode = value; }
        }

        private string m_UsrName;
        /// <summary>
        /// 当前用户姓名
        /// </summary>
        public string UsrName
        {
            get { return m_UsrName; }
            set { m_UsrName = value; }
        }

        /// <summary>
        /// 界面显示模块列表
        /// </summary>
        List<string> m_GroupBoxShowList = new List<string>();
        public List<string> GroupBoxShowList
        {
            get { return m_GroupBoxShowList; }
            set { }
        }
        /// <summary>
        /// 系统控件对接的通用参数集合
        /// </summary>
        private Hashtable htCommonParam = null;
        public Hashtable HtCommonParam
        {
            get
            {
                if (htCommonParam == null)
                { htCommonParam = new Hashtable(); }
                return htCommonParam;
            }
            set { htCommonParam = value; }
        }

        private string dbCode;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBCode
        {
            get { return dbCode; }
            set { dbCode = value; }
        }

        private IDBHelper dbHelper;
        /// <summary>
        /// 数据库链接
        /// </summary>
        public IDBHelper iDBHelper
        {
            get { return dbHelper; }
            set { dbHelper = value; }
        }

        private string data_auth;
        /// <summary>
        /// 组织机构
        /// </summary>
        public string DATA_AUTH
        {
            get { return data_auth; }
            set { data_auth = value; }
        }

        #endregion

        /// <summary>
        /// WorkStation.ini 配置文件路径
        /// </summary>
        string wsIniPath = Application.StartupPath + @"\Config\WorkStation.ini";
        /// <summary>
        /// 连接状态
        /// </summary>
        private bool linkStatus = false;

        public TaskX_Ray()
        {
            InitializeComponent();

            btnLink.Click += new EventHandler(btnLink_Click);
            tbSN.KeyDown += new KeyEventHandler(tbSN_KeyDown);
            tbReadPath.Click +=new EventHandler(tbReadPath_Click);
            //button1.Click += new EventHandler(button1_Click);
        }

        #region InitData
        public void InitData()
        {
            tbSharePath.Text = IniFileHelper.ReadIniData("ShareNet", "SharePath", "", wsIniPath);
            tbShareUserName.Text = IniFileHelper.ReadIniData("ShareNet", "ShareUserName", "", wsIniPath);
            tbSharePassWord.Text = IniFileHelper.ReadIniData("ShareNet", "SharePassWord", "", wsIniPath);
            tbReadRate.Text = IniFileHelper.ReadIniData("ShareNet", "Satt", "", wsIniPath);
            tbReadPath.Text = IniFileHelper.ReadIniData("ShareNet",  "ReadPath", "", wsIniPath);
        }
        #endregion

        #region 读取路径
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

        #region 扫描条码
        private void tbSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                snDataOperate(tbSN.Text.Trim());
                tbSN.Text = "";
                tbSN.Focus();
            }
        }
        #endregion


        #region 扫描SN处理方法
        private void snDataOperate(string sn)
        {
            if (!linkStatus)
            {
                string path = tbSharePath.Text;
                string userName = tbShareUserName.Text;
                string passWord = tbSharePassWord.Text;
                linkStatus = connectState(path, userName, passWord);
                if (!linkStatus)
                    return;
            }
            int tt = 5;
            int.TryParse(tbReadRate.Text, out tt);
            Thread.Sleep(tt * 1000);
            //获取路径下所以文件
            List<string> files = new List<string>();
            files = CWorkFlowControlHelper.GetDirFilesRecursion(tbReadPath.Text, files, "*.*");
            if (files.Count <= 0)
            {
                listBoxMsg.Items.Insert(0, "没有可上传的文件！！！");
                return;
            }

            //共享文件夹的目录
            DirectoryInfo theFolder = new DirectoryInfo(tbSharePath.Text);

            foreach (string file in files)
            {
                string filePath = file;
                string fileName = Path.GetFileName(filePath);

                //获取指定文件夹下到文本之间的路径
                // C:\Users\Administrator\Desktop\test\12A\QQQ.txt  取\12A\
                string midPath = filePath.Replace(tbReadPath.Text,"");
                midPath = midPath.Replace(fileName, "");
                //相对共享文件夹的路径
                string shareFileName = "";
                if (midPath.Length > 1)
                    shareFileName = @"\" + sn + midPath;
                else
                    shareFileName = @"\" + sn + @"\";

                //获取保存文件的路径
                string filename = theFolder.ToString() + shareFileName;
                //执行方法
                Transport(filePath, filename, fileName);

                //删除文件
                File.Delete(filePath);

                listBoxMsg.Items.Insert(0, "上传成功  SN:" + sn + "  Path:" + filename);

                //记录保存
                string guid = Guid.NewGuid().ToString("N");
                string sql = "INSERT INTO T_WIP_X_RAY (ID, CREATE_USER, CREATE_TIME, DATA_AUTH, SN, PATH) VALUES (";
                sql += "'" + guid + "','" + m_UsrCode + "',SYSDATE,'" + data_auth + "','" + sn + "','" + filename + fileName + "')";
                try
                {
                    dbHelper.ExecuteSql(sql);
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error("\r\n " + "数据保存失败:" + err.ToString() + "\r\n " + sql);
                    listBoxMsg.Items.Insert(0, "数据保存失败  SN:" + sn + "  Path:" + filePath);
                }
            }
            KillEmptyFolder(tbReadPath.Text);
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

        #region MyRegion
        void button1_Click(object sender, EventArgs e)
        {
            string myPath = @"C:\Users\Administrator\Desktop\1234.txt";
            myPath = @"C:\Users\Administrator\Desktop\test\12A\QQQ.txt";
            string mbPath = @"\\172.168.103.248\ftp";


            bool status = false;
            //连接共享文件夹
            status = connectState(@"\\172.168.103.248\ftp", "YWP", "yyf1989918");
            if (status)
            {
                //共享文件夹的目录
                DirectoryInfo theFolder = new DirectoryInfo(@"\\172.168.103.248\ftp");
                //相对共享文件夹的路径
                string fielpath = @"\test\12A\";
                //fielpath = @"\";
                //获取保存文件的路径
                string filename = theFolder.ToString() + fielpath;
                //执行方法
                Transport(myPath, filename, "QQQ.txt");

            }
            else
            {
                //ListBox1.Items.Add("未能连接！");
            }

            MessageBox.Show(myPath);
        }
        #endregion

        #region ExecScanBarOperate
        public CScanResult ExecScanBarOperate(string BarString)
        {
            CScanResult csr = new CScanResult();
            csr.BarString = BarString;
            csr.Result = "NG";
            csr.Remark = "NG：不符合流程";
            return csr;
        }
        #endregion
    }
}

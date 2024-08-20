using System;
using System.Configuration;
using System.Windows.Forms;
//using AutoMachineDataRead;
using ConfigHelper;
using ExcelReport;

namespace RTClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //if (RunningInstance() != null)
            //{
            //    MessageBox.Show("警告：程序已经被打开,该程序只能打开一个！！！");
            //    return;
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ExcelReportConfigHandler config1 = new ExcelReportConfigHandler();
            try
            {
                ParamLogsConfigHandler config = new ParamLogsConfigHandler();
            }
            catch (Exception ex)
            { }
            string systemType = ConfigurationManager.AppSettings["SystemType"];
            if (AutoUpdate())
            {
                if (ConfigurationManager.AppSettings["ClientType"] != null && ConfigurationManager.AppSettings["ClientType"].Equals("AutoRead"))
                {
                    Application.Run(new MainFrm(systemType));
                }
                else if (ConfigurationManager.AppSettings["ClientType"] != null && ConfigurationManager.AppSettings["ClientType"].Equals("WorkStation"))
                {
                    Application.Run(new frmMainClient(systemType));
                }
                else if (ConfigurationManager.AppSettings["ClientType"] != null && ConfigurationManager.AppSettings["ClientType"].Equals("UploadShare"))
                {
                    Application.Run(new Share());
                }
                else
                {
                    MessageBox.Show("提示：ClientType配置类型错误！");
                }
            }
        }

        #region AutoUpdate
        /// <summary>
        /// <pre>
        ///功能描述：自动更新
        ///关联函数：
        ///特别提示：
        ///</pre>
        /// </summary>
        private static bool AutoUpdate()
        {
            //如果存在配置项NeedAutoUpdate，且值为false，则不需要自动更新。否则自动更新！
            if (ConfigurationManager.AppSettings["NeedAutoUpdate"] != null && ConfigurationManager.AppSettings["NeedAutoUpdate"].ToLower().Equals("false"))
                return true;
            try
            {
                FwCore.AutoUpdate.AutoUpdateLib.Common updateHelper = new FwCore.AutoUpdate.AutoUpdateLib.Common();//取消了原Common中的大量的static方法，使用的时候再去new Common对象
                if (updateHelper.HasNewVersion(updateHelper.BuildCmdLineParam())) //原CheckVersion方法名修改为HasNewVersion
                {
                    //if (CMessageBox.ShowQuestion("AUTOUPDATE_ISUPDATE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.FileName = "FwCore.AutoUpdate.AutoUpdateLive.exe";
                        //startInfo.Arguments = System.Diagnostics.Process.GetCurrentProcess().Id + " \"ApplicationCenter.exe\" \"" + updateHelper.BuildCmdLineParam() + "\"";
                        startInfo.Arguments = System.Diagnostics.Process.GetCurrentProcess().Id + " \"RTClient.exe\" \"" + updateHelper.BuildCmdLineParam() + "\"";
                        System.Diagnostics.Process pro = System.Diagnostics.Process.Start(startInfo);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("自动更新失败:" + ex.Message, "更新错误提示");
                return false;
            }
        }
        #endregion

        #region RunningInstance 获取正在运行的实例，没有运行的实例返回null
        /// <summary>
        /// 获取正在运行的实例，没有运行的实例返回null;
        /// </summary>
        public static System.Diagnostics.Process RunningInstance()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}

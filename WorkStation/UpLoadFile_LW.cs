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
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using BaseModel.Logs;


namespace WorkStation
{
    public partial class UpLoadFile_LW : CUserControl, ICommonWorkStationCtl
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

        public UpLoadFile_LW()
        {
            InitializeComponent();
            ListBox.CheckForIllegalCrossThreadCalls = false;
            button1.Click += new EventHandler(button1_Click);
            tbReadPath.Click += new EventHandler(tbReadPath_Click);
            tbBackPath.Click += new EventHandler(tbBackPath_Click);

            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);

            btnStart.Click += new EventHandler(btnStart_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
         
            ckMsg.CheckedChanged += new EventHandler(ckMsg_CheckedChanged);
            ckRecursion.CheckedChanged += new EventHandler(ckRecursion_CheckedChanged);

            cbmFileType.SelectedIndexChanged += new EventHandler(cbmFileType_SelectedIndexChanged);
        }


        #region InitData
        public void InitData()
        {
            bool cm = false;
            tbReadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ReadPath", "", wsIniPath);
            tbBackPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "BackPath", "", wsIniPath);

            tbUploadPath.Text = "up_load\\comm\\lw";
            string upt = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "UploadPath", "", wsIniPath);
           // tbUploadPath.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "UploadPath", "", wsIniPath);

            tbReadRate.Text = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "Fre", "", wsIniPath);
            ckMsg.Checked = bool.TryParse(IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ckMsg", "", wsIniPath), out cm);
            cm = false;
            bool.TryParse(IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "ckRecursion", "", wsIniPath), out cm);
            ckRecursion.Checked = cm;

            string ih = IniFileHelper.ReadIniData("workStationNum", m_WorkStation + "IntervalHour", "", wsIniPath);
            try
            {
                tbIntervalHour.Text = int.Parse(ih).ToString();
            }
            catch (Exception)
            {
                tbIntervalHour.Text = "10";
            }
            string sql = "SELECT CODE,VALUE FROM SY_DICT_VAL WHERE DICT_CODE = 'REFLOW_FILE_TYPE' ORDER BY DISP_INDX";
            DataTable dt = dbHelper.GetDataTable(sql, "SY_DICT_VAL");
            this.cbmFileType.DataSource = dt;
            cbmFileType.DisplayMember = "VALUE";
            cbmFileType.ValueMember = "CODE";

            int index = 0;
            Int32.TryParse(upt, out index);
            cbmFileType.SelectedIndex = index;
            tbUploadPath.Text = "up_load\\comm\\lw\\" + index;
        }
        #endregion

        #region cbmFileType_SelectedIndexChanged
        private void cbmFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ve = cbmFileType.SelectedValue.ToString();
            if ("System.Data.DataRowView".Equals(ve))
                return;
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "UploadPath", ve, wsIniPath);
            tbUploadPath.Text = "up_load\\comm\\lw\\" + ve;
        }
        #endregion

        #region ckMsg_CheckedChanged
        private void ckMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMsg.Checked)
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ckMsg", ckMsg.Checked.ToString(), wsIniPath);
            else
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ckMsg", ckMsg.Checked.ToString(), wsIniPath);
        }  
        #endregion

        #region ckRecursion_CheckedChanged
        private void ckRecursion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckRecursion.Checked)
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ckRecursion", ckRecursion.Checked.ToString(), wsIniPath);
            else
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ckRecursion", ckRecursion.Checked.ToString(), wsIniPath);
        }
        #endregion

        #region 设置读备路径
        private void tbReadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbReadPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "ReadPath", this.tbReadPath.Text, wsIniPath);
            }
        }

        private void tbBackPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择一个备份文件";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.tbBackPath.Text = fbd.SelectedPath;
                IniFileHelper.WriteIniData("workStationNum", WorkStation + "BackPath", this.tbBackPath.Text, wsIniPath);
            }
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
            int intervalHour = 2;
            int.TryParse(tbIntervalHour.Text, out intervalHour);
            tbIntervalHour.Text = intervalHour.ToString();

            IniFileHelper.WriteIniData("workStationNum", WorkStation + "Fre", this.tbReadRate.Text, wsIniPath);
            IniFileHelper.WriteIniData("workStationNum", WorkStation + "IntervalHour", this.tbIntervalHour.Text, wsIniPath);

            int readpl = 5;
            int.TryParse(tbReadRate.Text, out readpl);
            if (readpl == 0)
                readpl = 5;
            timer1.Enabled = true;
            timer2.Interval = readpl * 1000;
            timer2.Enabled = true;
            SetEnable();
            listBoxMsg.Items.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    程序启动！");
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
            listBoxMsg.Items.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    程序停止！");

            try
            {
                DBHelper.Instance.FlashDBObject(DBCode);
            }
            catch (Exception)
            { }
            dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
        }
        #endregion

        #region SetEnable()
        private void SetEnable()
        {
            tbReadPath.Enabled = !tbReadPath.Enabled;
            tbBackPath.Enabled = !tbBackPath.Enabled;
            btnEnd.Enabled = !btnEnd.Enabled;
            ckMsg.Enabled = !ckMsg.Enabled;
            cbmFileType.Enabled = !cbmFileType.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
            tbReadRate.ReadOnly = !tbReadRate.ReadOnly;
            tbIntervalHour.Enabled = !tbIntervalHour.Enabled;
        }
        #endregion

        #region timer1_Tick
        int iTimerFlag = 0;
        string iTimerTitle = "系统正在监听,数据获取中...";
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
            List<string> files = new List<string>();
            if (ckRecursion.Checked)
                files = CWorkFlowControlHelper.GetDirFilesRecursion(tbReadPath.Text, files, "*.*", int.Parse(tbIntervalHour.Text));
            else
                files = CWorkFlowControlHelper.GetDirFiles(tbReadPath.Text, files, "*.*", int.Parse(tbIntervalHour.Text));
            if (files.Count <= 0)
                return true;

            dbHelper = DBHelper.Instance.GetDBInstace(DBCode);
            string url = "http://192.168.20.7:8066/mc/http/interface.ms?model=Upload&method=UploadLW";
            StringBuilder sb = new StringBuilder();
            string uploadPath = tbUploadPath.Text.Replace('\\', '*');//up_load*comm

            foreach (string file in files)
            {
                sb.Clear();
                Thread.Sleep(500);
                string filePath = file;
                string fileName = Path.GetFileName(filePath);
                string guid = Guid.NewGuid().ToString("N");
                string newFileName = guid + "_" + fileName;
                string res = CWorkFlowControlHelper.UploadRequest(url,newFileName, filePath, uploadPath);
                //string res = "{\"result\":\"NG\",\"msg\":\"文件上传成功\",\"path\":\"D:\\\\mesProject\\\\LinctlProject\\\\tomcat-7.0.67\\\\webapps\\\\mc\\\\up_load\\\\comm\\\\asdf.pdf\"}";
               
                //解析返回信息并数据处理
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    " + fileName + "    ");
                JObject jo = (JObject)JsonConvert.DeserializeObject(res);
                string result = jo["result"].ToString();
                if (!"OK".Equals(result))
                {
                    sb.Append("上传失败");
                    listBoxMsg.Items.Insert(0, sb.ToString());
                    if (ckMsg.Checked)
                    {
                        CWorkFlowControlHelper.MsgMessageBox("上传失败！","Error");
                        continue;
                    }
                }
                sb.Append("上传成功");
                string msg = jo["msg"].ToString();
                //string path = jo["path"].ToString();
                string path = tbUploadPath.Text;
                //炉温数据保存 
                string sql = "INSERT INTO T_WIP_TEMPERATURE_CURVE (ID,CREATE_USER,CREATE_TIME,DATA_AUTH,WORK_STATION,FILE_NAME,BEGIN_TIME,END_TIME,FILE_PATH,FILE_REAL_NAME,FILE_TYPE)";
                sql += "VALUES('{0}','{1}',SYSDATE,'{2}','{3}','{4}',to_date('{5}','yyyy-mm-dd,hh24:mi:ss'),to_date('{6}','yyyy-mm-dd,hh24:mi:ss'),'{7}','{8}','{9}')";

                sql = string.Format(sql, guid, m_UsrCode, data_auth, m_WorkStation, fileName, 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    path, newFileName, cbmFileType.SelectedValue.ToString());
                try
                {
                    dbHelper.ExecuteSql(sql);
                }
                catch (Exception err)
                {
                    SimpleLoger.Instance.Error(err.ToString() + "\r\n\r\n" + sql);
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    SetEnable();
                    lblState.Text = "数据保存失败，程序停止...";
                    DBHelper.Instance.FlashDBObject(DBCode);
                    sb.Append(",数据保存失败");
                    listBoxMsg.Items.Insert(0, sb.ToString());
                    return false;
                }


                //文件转移
                //string tempBackPath = tbBackPath.Text + "\\" + Path.GetFileName(file);
                string tempBackPath = tbBackPath.Text;
                if (ckRecursion.Checked)
                {
                    tempBackPath = tempBackPath + filePath.Replace(tbReadPath.Text, "");
                    string gfp = Path.GetDirectoryName(tempBackPath);  //判断备份文件夹下是否有子文件夹，没有创建
                    if (!Directory.Exists(gfp))
                    {
                        Directory.CreateDirectory(gfp);
                    }
                }
                else
                { 
                    tempBackPath = tempBackPath  + "\\" +  Path.GetFileName(file);
                }

                if (File.Exists(tempBackPath))
                {
                    tempBackPath = tbBackPath.Text + "\\" + guid + "_" + Path.GetFileName(file);
                }

                try
                {
                    File.Move(file, tempBackPath);
                }
                catch (Exception err)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    SetEnable();
                    lblState.Text = "文件转移失败停止...文件已存在";
                    DBHelper.Instance.FlashDBObject(DBCode);
                    sb.Append(",文件转移失败");
                    listBoxMsg.Items.Insert(0, sb.ToString());
                    return false;
                }

                listBoxMsg.Items.Insert(0, sb.ToString());
                //ShowListBoxMsg(sb.ToString());
            }
            DBHelper.Instance.FlashDBObject(DBCode);
            return true;
        }
        #endregion


        public void ShowListBoxMsg(string msg)
        {
            Thread th = new Thread(new ThreadStart(() =>
            {
                listBoxMsg.Items.Insert(0, msg);
            }));
            th.Start();
        }


        #region 测试
        void button1_Click(object sender, EventArgs e)
        {



           /* string url = "https://www.morewiscloud.com:10312/mc/http/interface.ms?model=Upload&method=UploadLW";
            url = "http://127.0.0.1:8080/mc/http/interface.ms?model=Upload&method=UploadLW";

            List<string> files = new List<string>();
            files = CWorkFlowControlHelper.GetDirFiles(tbReadPath.Text, files, "*.*");
            if (files.Count <= 0)
                return;

            foreach (string file in files)
            {
                string filePath = file;
                UploadRequest(url, filePath);
            }*/
        }


        private string UploadRequest(string url, string filePath)
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
            string header = string.Format(dataFormat, "file", Path.GetFileName(filePath));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(header);

            //写入参数
            string para = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            string spt = string.Format(para, "servicePath", "up_load*comm");
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

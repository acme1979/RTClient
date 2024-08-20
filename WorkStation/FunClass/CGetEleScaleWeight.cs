using System;
using System.Configuration;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading;

namespace WorkStation.FunClass
{
    public class CGetEleScaleWeight
    {
        #region 属性
        /// <summary>
        /// 端口名称
        /// </summary>
        string _PortName = "";
        public string PortName
        {
            get { return _PortName; }
            set { _PortName = value; }
        }
        /// <summary>
        /// 串行波特率
        /// </summary>
        int _BaudRate = 9600;
        public int BaudRate
        {
            get { return _BaudRate; }
            set { _BaudRate = value; }
        }
        ///// <summary>
        ///// 奇偶校验位
        ///// </summary>
        //public Parity Parity
        //{
        //    get ;
        //    set;
        //}
        ///// <summary>
        ///// 设置标准停止位
        ///// </summary>
        //public StopBits StopBits
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 缓冲区字节数16字节
        /// </summary>
        int _ReceivedBytesThreshold = 16;
        public int ReceivedBytesThreshold
        {
            get { return _ReceivedBytesThreshold; }
            set { _ReceivedBytesThreshold = value; }
        }
        /// <summary>
        /// 称重获取的数据
        /// </summary>
        double _Weight = 0;
        public double Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        /// <summary>
        /// 称重获取的单位
        /// </summary>
        string _Unit = "";
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        /// <summary>
        /// 称重数据显示标签
        /// </summary>
        private Label _lblWeightData = new Label();
        public Label lblWeightData
        {
            get { return _lblWeightData; }
            set { _lblWeightData = value; }
        }
        /// <summary>
        /// 单位数据显示标签
        /// </summary>
        private Label _lblUnit = new Label();
        public Label lblUnit
        {
            get { return _lblUnit; }
            set { _lblUnit = value; }
        }
        /// <summary>
        /// 端口变量
        /// </summary>
        public SerialPort Sp = new SerialPort();
        /// <summary>
        /// 数据读取标识，TRUE为读取，FALSE为不读
        /// </summary>
        private Boolean flagOpenReadCOM = true;
        /// <summary>
        /// 刷码标识，TRUE为可刷码，FALSE为不可刷码
        /// </summary>
        private Boolean _flagReadCorde = true;
        public Boolean flagReadCorde
        {
            get { return _flagReadCorde; }
            set { _flagReadCorde = value; }
        }
        /// <summary>
        /// 委托标识，此为重点
        /// </summary>
        public delegate void HandleInterfaceUpdataDelegate(string text);
        #endregion

        #region 方法
        #region GetpPrinterName()获取打印机名称
        /// <summary>
        /// 获取打印机名称
        /// </summary>
        /// <returns>返回datatable值</returns>
        public void GetpPrinterName(ComboBox cbb)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cbb.Items.Add(printer);
            }
            cbb.Items.Add("file");
            PrintDocument fPrintDocument = new PrintDocument();
            cbb.SelectedIndex = cbb.Items.IndexOf(fPrintDocument.PrinterSettings.PrinterName);
        }
        #endregion

        #region GetCOMname()获取端口名称
        /// <summary>
        /// 获取端口名称
        /// </summary>
        /// <returns>返回datatable值</returns>
        public void GetCOMname(ComboBox cbb)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey(@"Hardware\DeviceMap\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    cbb.Items.Add(sValue);
                }
            }
            //cbb.Text = System.Configuration.ConfigurationSettings.AppSettings["ComPortId"].ToString();
            //cbb.Text = 
        }
        #endregion

        #region SetTransCoding(byte[] data)把十进制的ASCII码转换为数字和字母，小数点的字符串
        /// <summary>
        /// ASCII码转换为字符串
        /// </summary>
        /// <param name="data">串口传回ASCII数据</param>
        /// <returns>字符串</returns>
        private string setTransCoding(byte[] data)
        {
            byte[] byteRead = new byte[18];
            string strRcv = null;
            System.Text.Encoding encoding = System.Text.Encoding.ASCII;
            for (int i = 0; i < data.Length; i++)
            {
                int recData = 0;
                if (int.TryParse(data[i].ToString(), out recData))
                {
                    if (recData < 46 || (recData > 57 && recData < 65) || (recData > 90 && recData < 94) || recData > 122)
                    {
                        continue;
                    }
                    strRcv += encoding.GetString(data, i, 1);
                    if (recData == 13 || recData == 10)
                    {
                        break;
                    }
                }
            }
            return strRcv;
        }
        #endregion

        #region GetParsedString(string data)把称重返回信息字符串解析为可用数据
        /// <summary>
        /// 获取称重数据
        /// </summary>
        /// <param name="data">称重字符串</param>
        /// <returns>双精度浮点数</returns>
        private double GetParsedStringDouble(string data)
        {
            string sTemp = data;
            double fTemp = 0;
            if (sTemp.Length > 0)
            {
                sTemp = sTemp.Replace("G.W.", " ").Trim();
                sTemp = sTemp.Replace("STNT", " ").Trim();
                sTemp = sTemp.Replace("STTR", " ").Trim();
                sTemp = sTemp.Replace("USNT", " ").Trim();
                sTemp = sTemp.Replace("KG", " ").Trim();
                sTemp = sTemp.Replace("kg", " ").Trim();
                sTemp = sTemp.Replace('G', ' ').Trim();
                sTemp = sTemp.Replace('g', ' ').Trim();
                sTemp = sTemp.ToUpper().Replace(":", "");
                sTemp = sTemp.ToUpper().Replace("+", "");
                sTemp = sTemp.Replace(" ", "");
                double.TryParse(sTemp, out fTemp);
            }
            return fTemp;
        }
        #endregion

        #region GetParsedStringUnit(string data)把获取称重单位
        /// <summary>
        /// 获取称重单位
        /// </summary>
        /// <param name="data">称重字符串</param>
        /// <returns>称重单位</returns>
        private string GetParsedStringUnit(string dataString)
        {
            string unit = "";
            if (dataString.Length > 0)
            {
                if (dataString.Contains("G") || dataString.Contains("g"))
                {
                    unit = "g";
                }
                if (dataString.Contains("KG") || dataString.Contains("kg"))
                {
                    unit = "kg";
                }
            }
            return unit;
        }
        #endregion

        #region OpenPortAndStartWeighting() 端口打开，并开启一个线程读取RS232端口数据
        /// <summary>
        /// 端口打开，并开启一个线程读取RS232端口数据
        /// </summary>
        /// <returns>返回提示字符串</returns>
        public string OpenPortAndStartWeighting()
        {
            string str = "";
            if (this.PortName != "")
            {
                Sp.PortName = this._PortName;
                Sp.BaudRate = this._BaudRate;
                Sp.Parity = Parity.None;
                Sp.StopBits = StopBits.One;
                Sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                Sp.ReceivedBytesThreshold = this.ReceivedBytesThreshold;
                if (this.Sp.IsOpen == false)
                {
                    Sp.Open();
                }
                //else
                //{
                //    str = "端口打开失败，请重新检查";
                //}
            }
            else
            {
                str = "端口名称不能为空，请重新检查";
            }
            return str;
        }
        #endregion

        #region ClosePortAndStopWeighting() 关闭打开的端口
        /// <summary>
        /// 端口打开，并开启一个线程读取RS232端口数据
        /// </summary>
        /// <returns>返回提示字符串</returns>
        public string ClosePortAndStopWeighting()
        {
            string str = "";
            if (this.Sp.IsOpen)
            {
                Sp.Close();
            }
            //else
            //{
            //    str = "端口关闭失败，请重新检查";
            //}
            return str;
        }
        #endregion

        #region SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)RS232端口监测事件
        //sp是串口控件
        /// <summary>
        /// RS232端口监测事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int len = Sp.BytesToRead;
                if (len < 18)
                {
                    System.Threading.Thread.Sleep(200);
                    return;
                }
                //读取RS232端口内容bit字节
                byte[] receivedData = new byte[18];
                byte[] getData = new byte[Sp.BytesToRead];
                Sp.Read(getData, 0, Sp.BytesToRead);
                bool flag = false;
                for (int k = 0; k < getData.Length; k++)
                {
                    if (getData[k].ToString() == "71" && getData[k + 1].ToString() == "46" && k + 18 <= getData.Length)
                    {
                        for (int i = 0; i < 18; i++)
                        {
                            if (getData.Length > i + k)
                            {
                                receivedData[i] = getData[i + k];
                            }
                            else
                            {
                                receivedData[i] = (byte)'0';
                            }
                        }
                        flag = true;
                        break;
                    }
                    if (k > 30)
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    return;
                }
                //限制过度读取，读取完一个且解析完后再读取
                if (Sp.IsOpen && flagOpenReadCOM && receivedData.Length == 18)
                {
                    //设置关闭关闭节点
                    flagOpenReadCOM = !flagOpenReadCOM;
                    string strRcv = null;
                    //把ASCII码bit字节转换为正常字符串
                    strRcv = setTransCoding(receivedData);
                    //更新lable标签显示数据
                    if (!strRcv.ToUpper().StartsWith("G.W."))
                    {
                        strRcv = "0";
                    }
                    UpdateData(strRcv.ToUpper());
                    //设置打开读取节点
                    flagOpenReadCOM = !flagOpenReadCOM;
                }
            }
            catch (Exception ex)
            {
                //如果解析出错，重新打开读取节点标志
                flagOpenReadCOM = true;
                //MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region SerialPort_DataReceived1
        //sp是串口控件
        /// <summary>
        /// RS232端口监测事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int count = Sp.BytesToRead;
                if (count > 0)
                {
                    byte[] readBuffer = new byte[count];
                    Application.DoEvents();
                    Sp.Read(readBuffer, 0, count);
                    Thread.Sleep(500);
                    Sp.DiscardOutBuffer();
                    //限制过度读取，读取完一个且解析完后再读取
                    if (Sp.IsOpen && flagOpenReadCOM)
                    {
                        //设置关闭关闭节点
                        flagOpenReadCOM = !flagOpenReadCOM;
                        //把ASCII码bit字节转换为正常字符串
                        System.Text.ASCIIEncoding AsciiEncoding = new System.Text.ASCIIEncoding();
                        string valueString = AsciiEncoding.GetString(readBuffer);
                        string[] arrs = valueString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        valueString = "0";
                        foreach (string arr in arrs)
                        {
                            //更新lable标签显示数据
                            if (arr.ToUpper().Contains("G.W."))
                            {
                                valueString = arr;
                                break;
                            }
                        }
                        UpdateData(valueString);
                        //设置打开读取节点
                        flagOpenReadCOM = !flagOpenReadCOM;
                    }
                }
            }
            catch (Exception ex)
            {
                //如果解析出错，重新打开读取节点标志
                flagOpenReadCOM = true;
                //MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region UpdateData(string dataString)把副线程的数据提交到主线程中的控件显示
        /// <summary>
        /// 把副线程的数据提交到主线程中的控件显示
        /// </summary>
        /// <param name="dataString">称重字符串数据</param>
        private void UpdateData(string dataString)
        {
            string sTemp = dataString;
            double w = GetParsedStringDouble(sTemp);
            string u = GetParsedStringUnit(sTemp);
            if (sTemp != "")
            {
                //HandleInterfaceUpdataDelegate del = new HandleInterfaceUpdataDelegate(SetUpdateLabelw);
                //lblWeightData.Invoke(del, sTemp);
                if (lblWeightData.InvokeRequired == true && w != Weight)
                {
                    Weight = w;
                    //访问主线程，并设置主线程里的控件显示数据
                    HandleInterfaceUpdataDelegate del = new HandleInterfaceUpdataDelegate(SetUpdateLabelw);

                    if (_Unit == "kg")
                    {
                        lblWeightData.BeginInvoke(del, Weight.ToString("F3"));
                    }
                    else
                    {
                        lblWeightData.BeginInvoke(del, Weight.ToString("F1"));
                    }
                }
                if (lblUnit.InvokeRequired == true && u != Unit && u != "")
                {
                    Unit = u;
                    //访问主线程，并设置主线程里的控件显示数据
                    HandleInterfaceUpdataDelegate del = new HandleInterfaceUpdataDelegate(SetUpdateLabelu);
                    lblUnit.BeginInvoke(del, Unit);
                }
            }
            if (Weight == 0)
            {
                flagReadCorde = true;
            }
        }
        #endregion

        #region SetUpdateLabel(string weight)设置标签显示读取的数据值，数值和单位
        /// <summary>
        /// 设置标签显示读取的数据值，数值和单位
        /// </summary>
        /// <param name="weight">传入从RS232端口读取，且转换为数值和字母的字符串</param>
        private void SetUpdateLabelw(string weight)
        {
            lblWeightData.Text = weight;

        }
        #endregion

        #region SetUpdateLabelu(string Unit)设置标签显示读取的数据值，数值和单位
        /// <summary>
        /// 设置标签显示读取的数据值，数值和单位
        /// </summary>
        /// <param name="weight">传入从RS232端口读取，且转换为数值和字母的字符串</param>
        private void SetUpdateLabelu(string Unit)
        {
            lblUnit.Text = Unit;
        }
        #endregion

        #endregion
    }
}

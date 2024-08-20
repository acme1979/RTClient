using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WorkStation.FunClass
{
    public class CSerialPortComm
    {
        #region 属性
        /// <summary>
        /// 定义EventHandle
        /// </summary>
        /// <param name="readBuffer"></param>
        public delegate void EventHandle(byte[] readBuffer);
        /// <summary>
        /// 接收EventHandle
        /// </summary>
        public event EventHandle DataReceived;
        /// <summary>
        /// 定义端口类
        /// </summary>
        public SerialPort serialPort;
        /// <summary>
        /// 线程控制
        /// </summary>
        Thread thread;
        /// <summary>
        /// 是否读取数据
        /// </summary>
        volatile bool _keepReading;
        #endregion

        #region CSerialPortComm
        /// <summary>
        /// 初始化类
        /// </summary>
        public CSerialPortComm()
        {
            serialPort = new SerialPort();
            thread = null;
            _keepReading = false;
        }
        #endregion

        #region IsOpen
        /// <summary>
        /// 获取串口打开状态
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }
        #endregion

        #region StartReading
        /// <summary>
        /// 开启线程读取串口数据
        /// </summary>
        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                thread = new Thread(new ThreadStart(ReadPort));
                thread.Start();
            }
        }
        #endregion

        #region StopReading
        /// <summary>
        /// 停止读取数据
        /// </summary>
        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                thread.Join();
                thread = null;
            }
        }
        #endregion

        #region ReadPort
        /// <summary>
        /// 读取串口数据
        /// </summary>
        private void ReadPort()
        {
            while (_keepReading)
            {
                if (serialPort.IsOpen)
                {
                    Thread.Sleep(200);
                    int count = serialPort.BytesToRead;
                    if (count > 0)
                    {
                        byte[] readBuffer = new byte[count];
                        try
                        {
                            Application.DoEvents();
                            serialPort.Read(readBuffer, 0, count);
                            if (DataReceived != null)
                                DataReceived(readBuffer);
                            Thread.Sleep(500);
                            serialPort.DiscardOutBuffer();
                        }
                        catch (TimeoutException)
                        {
                        }
                    }
                }
            }
        }
        #endregion

        #region Open
        /// <summary>
        /// 打开端口开启线程
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            Close();
            serialPort.Open();
            if (serialPort.IsOpen)
            {
                StartReading();
                return true;
            }
            else
            {
                return false ;
            }
        }
        #endregion

        #region OpenPort
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="PortName">端口号</param>
        /// <param name="StopBits">停止位1，1.5，2</param>
        /// <param name="Parity">奇偶校验位</param>
        /// <param name="BaudRate">波特率</param>
        /// <param name="DataBits">标准数据长度，5，6，7，8</param>
        /// <param name="ReadTimeout">读取超时</param>
        /// <param name="WriteTimeout">写入超时</param>
        /// <returns></returns>
        public bool OpenPort(string PortName, System.IO.Ports.StopBits StopBits, System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None, int BaudRate = 9600, int DataBits = 8, int ReadTimeout = 500, int WriteTimeout = -1)
        {
            //读取端口名称
            serialPort.PortName = PortName;
            //波特率
            serialPort.BaudRate = BaudRate;
            //数据位
            serialPort.DataBits = DataBits;
            //两个停止位
            serialPort.StopBits = StopBits;
            //无奇偶校验位
            serialPort.Parity = Parity;
            //读取超时
            serialPort.ReadTimeout = ReadTimeout;
            //写入超时
            serialPort.WriteTimeout = WriteTimeout;
            //打开端口
            return Open();
        }
        #endregion

        #region Close
        /// <summary>
        /// 释放串口
        /// </summary>
        public void Close()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                StopReading();
                serialPort.Close();
            }
        }
        #endregion

        #region WritePort
        /// <summary>
        /// 往串口写入数据
        /// </summary>
        /// <param name="send">byte数据</param>
        /// <param name="offSet"></param>
        /// <param name="count"></param>
        public void WritePort(byte[] send, int offSet, int count)
        {
            if (IsOpen)
            {
                serialPort.Write(send, offSet, count);
            }
        }
        #endregion

        #region Loadingcmb 加载com端口
        /// <summary>
        /// 获取COM口数据
        /// </summary>
        /// <param name="cbb"></param>
        public void Loadingcmb(ComboBox cbb)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey(@"Hardware\DeviceMap\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                cbb.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    cbb.Items.Add(sValue);
                }
            }
        }
        #endregion

        #region ByteArrayToHexString  将一个byte数组转换成16进制字符串
        /// <summary>
        /// 将一个byte数组转换成16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }
        #endregion

        #region ByteArrayToHexString 将16进制字符串转换成byte数组
        /// <summary>
        /// 将16进制字符串转换成byte数组
        /// </summary>
        /// <param name="hexString">16进制字符串</param>
        /// <returns>byte数组</returns>
        public static byte[] ByteArrayToHexString(string hexString)
        {
            //将16进制秘钥转成字节数组
            var byteArray = new byte[hexString.Length / 2];
            for (var x = 0; x < byteArray.Length; x++)
            {
                var i = Convert.ToInt32(hexString.Substring(x * 2, 2), 16);
                byteArray[x] = (byte)i;
            }
            return byteArray;
        }
        #endregion

        #region HexAnyTo10 非10进制转10进制
        /// <summary>
        /// 非10进制转10进制
        /// </summary>
        /// <param name="strAny">非10进制字符</param>
        /// <param name="strHex">非10进制数母字符串</param>
        /// <returns></returns>
        public static int HexAnyTo10(string strAny, string strHex = "0123456789ABCDEF")
        {
            double a = 0;
            string b = "";
            for (int i = 0; i < strAny.Length; i++)
            {
                b = strAny.Substring(i, 1);
                if (strHex.IndexOf(b) > 0)//判断进制是否存在字符
                {
                    a = a + (strHex.IndexOf(b)) * Math.Pow(strHex.Length, strAny.Length - i - 1);
                }
            }
            return (int)a;
        }
        #endregion

        #region Hex10ToAny10进制转非10进制
        /// <summary>
        /// 10进制转非10进制
        /// </summary>
        /// <param name="int10">10进制数</param>
        /// <param name="strHex">非10进制数母字符串</param>
        /// <returns></returns>
        public static string Hex10ToAny(int int10, string strHex = "0123456789ABCDEF")
        {
            int a = int10;
            string b = "";
            if (a == 0)
            {
                b = strHex.Substring(0, 1);
            }
            while (a > 0)
            {
                b = strHex.Substring(a % strHex.Length, 1) + b;
                a = (a - a % strHex.Length) / strHex.Length;
            }
            return b;
        }
        #endregion

        #region IEEE754 单精度浮点格式
        /// <summary>
        /// IEEE754 单精度浮点格式（共四字节32 位，从高到低）
        /// 转为为十进制浮点数据公式为: FData=(-1)^S * (1 + F) * 2^(E - 127) 
        /// 格式说明：
        ///    A、第32 bit 为符号位，为0 则表示正数，反之为负数，其读数值用S 表示；
        ///    B、第31～24 bit 共8 位为幂数(2 的幂数)，其读数值用E 表示；
        ///    C、第23～1 bit 共23 位作为系数，视为二进制纯小数，假定该小数的十进制值为F
        /// </summary>
        /// <param name="binString">二进制</param>
        /// <returns></returns>
        public static float FData(string binString)
        {
            //00000110100000110001111001110011
            int S = 1;
            if (binString.Substring(0, 1) == "1") S = -1;
            int powe = HexAnyTo10(binString.Substring(1, 8), "01");
            double sum = 0;
            Char[] sg = binString.Substring(9, 23).ToCharArray();
            for (int i = 1; i <= 23; i++)
            {
                if (sg[i - 1] == '1')
                {
                    sum += 1 / (Math.Pow(2, i));
                }

            }
            return (float)(S * (1 + sum) * Math.Pow(2, powe - 127));
        }
        #endregion

        #region CRC16_Modbus效验
        /// <summary>
        /// CRC16_Modbus效验
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public byte[] ToModbus(byte[] byteData)
        {
            byte[] CRC = new byte[2];
            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }
            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;
        }


        /// <summary>
        /// CRC16_Modbus效验
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <param name="byteLength">长度</param>
        /// <returns>计算后的数组</returns>
        public byte[] ToModbus(byte[] byteData, int byteLength)
        {
            byte[] CRC = new byte[2];
            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteLength; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;
        }
        #endregion

        #region byteToHexStr
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion
    }
}
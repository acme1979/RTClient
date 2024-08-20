using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkStation.FunClass
{
    /// <summary>
    /// json实体类
    /// </summary>
    public class CJsonRoot
    {
        /// <summary>
        /// 蓝牙MAC地址
        /// </summary>
        public string BtMacAddr { get; set; }
        /// <summary>
        /// 顺丰资产编号
        /// </summary>
        public string DSN2 { get; set; }
        /// <summary>
        /// 机型功能
        /// </summary>
        public string DevFunc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ICCID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IMEI { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IMEI2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MacAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Serial { get; set; }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string time { get; set; }
        }

    }
}

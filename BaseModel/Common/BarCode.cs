using System;
using System.Collections.Generic;
using System.Text;
using IEC16022Sharp;
using ThoughtWorks.QRCode;
using System.IO;

namespace BaseModel
{
    public class BarCode
    {
        #region 唯一静态实例
        private BarCode() { }

        private static BarCode m_Instance;
        public static BarCode Instance
        {
            get
            {
                lock (typeof(BarCode))
                {
                    if (m_Instance == null)
                        m_Instance = new BarCode();
                }
                return m_Instance;
            }
        }
        #endregion

        public string CreateQRCodeImage(string sBarcode)
        {
            try
            {
                ThoughtWorks.QRCode.Codec.QRCodeEncoder qrcode = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
                qrcode.QRCodeBackgroundColor = System.Drawing.Color.White;
                qrcode.QRCodeForegroundColor = System.Drawing.Color.Black;
                qrcode.QRCodeScale = 4;
                qrcode.QRCodeVersion = 5;
                qrcode.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
                qrcode.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.L;
                string tempPath = Path.GetTempPath() + CreateFileName("JPG");
                byte[] imageBytes;
                System.Drawing.Image image = qrcode.Encode(sBarcode);
                MemoryStream imgMem = new MemoryStream();
                image.Save(imgMem, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageBytes = imgMem.GetBuffer();
                FileStream fs = new FileStream(tempPath, FileMode.Create);
                fs.Write(imageBytes, 0, imageBytes.Length);
                fs.Close();
                fs.Dispose();
                return tempPath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CreateFileName(string extensionname)
        {
            return GetGUID() + "." + extensionname;
        }

        #region 生成GUID
        /// <summary>
        /// 添加日期 2014/11/23
        /// </summary>
        /// <returns></returns>
        public string GetGUID()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        #endregion
    }
}

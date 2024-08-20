using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LabelManager2;
using System.Data;
using System.Collections;

namespace BaseModel
{
    public class PrintLabelByCS6
    {
        #region 成员变量
        #region TempletFileName
        private string sTempletFileName;
        public string TempletFileName
        {
            set
            {
                if (File.Exists(value))
                {
                    sTempletFileName = value;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
        }
        #endregion

        #region CS6State
        private bool cs6State;
        public bool CS6State
        {
            get { return cs6State; }
            set { cs6State = value; }
        }
        #endregion

        #region PrintNum
        private int iPrintNum;
        public int PrintNum
        {
            get
            {
                if (iPrintNum <= 0)
                {
                    return 1;
                }
                else
                {
                    return iPrintNum;
                }
            }
            set
            {
                if (value > 0)
                {
                    iPrintNum = value;
                }
                else
                {
                    throw new Exception("打印张数不能小于或等于0！");
                }
            }
        }
        #endregion

        #region DtPrintContent
        private DataTable dtPrintContent;
        public DataTable DtPrintContent
        {
            set
            {
                if (value.Rows.Count > 0)
                {
                    dtPrintContent = value;
                }
                else
                {
                    throw new Exception("不能传空数据，否则无法打印！");
                }
            }
            get
            {
                return dtPrintContent;
            }
        }
        #endregion

        #region HtPrintContent
        private Hashtable htPrintContent;
        public Hashtable HtPrintContent
        {
            set
            {
                htPrintContent = value;
            }
            get
            {
                return htPrintContent;
            }
        }
        #endregion

        #region DsPrintContent
        private DataSet dsPrintContent;
        public DataSet DsPrintContent
        {
            set
            {
                dsPrintContent = value;
            }
            get
            {
                return dsPrintContent;
            }
        }
        #endregion

        #region PrinterName
        private string printerName;
        public string PrinterName
        {
            set
            {
                printerName = value;
            }
        }
        #endregion

        private LabelManager2.ApplicationClass csApp;
        private LabelManager2.Document csDoc;
        #endregion

        #region Open()
        private void Open()
        {
            csApp = new ApplicationClass();
            csApp.Documents.Open(sTempletFileName);
            csDoc = csApp.ActiveDocument;
            if (printerName != "")
            {
                csDoc.Printer.Name = printerName;
                csDoc.Printer.SwitchTo(printerName, "", true);
            }
            CS6State = true;
        }
        #endregion

        #region Destroy
        /// <summary>
        /// 对象的释放
        /// </summary>
        public void Destroy()
        {
            csApp.Documents.CloseAll(false);
            csApp.EnableEvents = false;
            csApp.Quit();
        }
        #endregion

        #region PrintLabelByCS6
        /// <summary>
        /// 传入模板文件，构造函数会自动开启CodeSoft6软件
        /// </summary>
        /// <param name="templetFileName">模板文件名</param>
        public PrintLabelByCS6(string templetFileName)
        {
            sTempletFileName = templetFileName;
            Open();
        }
        #endregion

        #region PrintLabelByCS6
        /// <summary>
        /// 传入模板文件,同时指定所使用的打印机名称，构造函数会自动开启CodeSoft6软件
        /// </summary>
        /// <param name="templetFileName">模板文件名</param>
        public PrintLabelByCS6(string templetFileName, string printerName)
        {
            sTempletFileName = templetFileName;
            PrinterName = printerName;
            Open();
        }
        #endregion

        #region PrintLabelByDataTable()
        /// <summary>
        /// 用户传入DataTable来打印数据
        /// </summary>
        /// <returns></returns>
        public bool PrintLabelByDataTable()
        {
            try
            {
                if (DtPrintContent.Rows.Count <= 0)
                {
                    throw new Exception("需要打印的数据集为空，无法正常打印！");
                }
                if (!File.Exists(sTempletFileName))
                {
                    throw new Exception("模板文件不存在，请检查！");
                }
                foreach (DataRow row in dtPrintContent.Rows)
                {
                    for (int i = 0; i < row.Table.Columns.Count; i++)
                    {
                        if (csDoc.Variables.FormVariables.Item(row.Table.Columns[i].ColumnName) != null)
                        {
                            csDoc.Variables.FormVariables.Item(row.Table.Columns[i].ColumnName).Value = row[i].ToString();
                        }
                    }
                    csDoc.PrintDocument(PrintNum);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region PrintLabelByHashTable()
        /// <summary>
        /// 用户传入HashTable来打印数据
        /// </summary>
        /// <returns></returns>
        public bool PrintLabelByHashTable()
        {
            try
            {
                if (HtPrintContent.Count <= 0)
                {
                    throw new Exception("需要打印的HashTable为空，无法正常打印！");
                }
                if (!File.Exists(sTempletFileName))
                {
                    throw new Exception("模板文件不存在，请检查！");
                }
                foreach (DictionaryEntry entry in HtPrintContent)
                {
                    if (csDoc.Variables.FormVariables.Item(entry.Key) != null)
                    {
                        csDoc.Variables.FormVariables.Item(entry.Key).Value = entry.Value.ToString();
                    }
                }
                csDoc.PrintDocument(PrintNum);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region PrintLabelByDataSet()
        /// <summary>
        /// 用户传入DateSet来打印数据
        /// </summary>
        /// <returns></returns>
        public bool PrintLabelByDataSet()
        {
            try
            {
                if (HtPrintContent.Count <= 0)
                {
                    throw new Exception("需要打印的HashTable为空，无法正常打印！");
                }
                if (!File.Exists(sTempletFileName))
                {
                    throw new Exception("模板文件不存在，请检查！");
                }
                foreach (DictionaryEntry entry in HtPrintContent)
                {
                    if (csDoc.Variables.FormVariables.Item(entry.Key) != null)
                    {
                        csDoc.Variables.FormVariables.Item(entry.Key).Value = entry.Value.ToString();
                    }
                }
                csDoc.PrintDocument(PrintNum);
                foreach (DataRow row in dtPrintContent.Rows)
                {
                    for (int i = 0; i < row.Table.Columns.Count; i++)
                    {
                        if (csDoc.Variables.FormVariables.Item(row.Table.Columns[i].ColumnName) != null)
                        {
                            csDoc.Variables.FormVariables.Item(row.Table.Columns[i].ColumnName).Value = row[i].ToString();
                        }
                    }
                    csDoc.PrintDocument(PrintNum);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}

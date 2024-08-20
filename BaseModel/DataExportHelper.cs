using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Windows.Forms;

namespace BaseModel
{
    public class DataExportHelper
    {
        /// <summary>
        /// Xslt文件存放路径
        /// </summary>
        private static string xsltPath = "";

        #region 唯一静态实例
        private DataExportHelper() { }

        /// <summary>
        /// 唯一静态实例
        /// </summary>
        private static DataExportHelper m_Instance;
        public static DataExportHelper Instance
        {
            get
            {
                lock (typeof(DataExportHelper))
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new DataExportHelper();
                        //获取Xslt文件存放路径
                        xsltPath = Directory.GetCurrentDirectory() + "\\Xslt\\";
                    }
                }
                return m_Instance;
            }
        }
        #endregion

        #region Property PathDialog
        /// <summary>
        /// 路径选择窗口
        /// </summary>
        private SaveFileDialog pathDialog = null;
        /// <summary>
        /// 路径选择窗口
        /// </summary>
        public SaveFileDialog PathDialog
        {
            get
            {
                if (this.pathDialog == null)
                {
                    this.pathDialog = new SaveFileDialog();
                    this.pathDialog.RestoreDirectory = true;
                    this.pathDialog.Filter = "Excel|*.xls";
                    this.pathDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                return pathDialog;
            }
        }
        #endregion

        #region Method ExportGridToExcel
        /// <summary>
        /// 将数据集导出到Excel文件,适用于数据列动态变化的情况
        /// </summary>
        /// <param name="ID">窗口的ID，对应功能ID及Xslt文件名</param>
        /// <param name="fileName">窗口的中文描述，CUserControl可通过ControlDesc属性获取、CForm可通过Text获得</param>
        /// <param name="dsSource">要导出到Excel的数据源</param>
        /// <param name=" dgv">展现数据的DataGridView </param>
        public void ExportGridToExcel(string ID, string fileName, DataSet dsSource, DataGridView dgv)
        {
            this.CreateXsltByGrid(ID, fileName, dgv);
            this.ExportToExcel(ID, fileName, dsSource, null);
        }
        #endregion

        #region Method CreateXsltByGrid
        /// <summary>
        /// 根据给定的DataGridView生成相应的样式表
        /// </summary>
        /// <param name="dgv">要用作生成样式表的Grid</param>
        /// <param name="title">要传递到Grid中的标题</param>
        /// <returns>返回样式表文件名</returns>
        public void CreateXsltByGrid(string Id, string title, DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;

            int width = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                    width += col.Width;
            }
            if (width == 0) return;

            try
            {
                if (!Directory.Exists(xsltPath))
                {
                    Directory.CreateDirectory(xsltPath);
                }
                string xsltFile = xsltPath + Id.Trim() + ".xslt";
                StreamWriter sw = new StreamWriter(xsltFile, false, Encoding.UTF8);
                //添加文件头
                sw.Write(global::BaseModel.Properties.Resources.XsltTemplate);
                sw.Write("\r\n");
                //添加Table行
                sw.WriteLine("<table align=\"center\" width=\"" + width + "px\">");

                #region 添加标题行
                if (!string.IsNullOrEmpty(title))
                {
                    sw.WriteLine("  <tr>");
                    sw.WriteLine("      <td align=\"center\" height=\"50px\" style=\"font-size:22pt\">");
                    sw.WriteLine("      <b>" + title + "</b>");
                    sw.WriteLine("      </td>");
                    sw.WriteLine("  </tr>");
                }
                #endregion

                #region 添加列标题
                sw.WriteLine("  <tr>");
                sw.WriteLine("     <td>");
                sw.WriteLine("         <table align=\"center\">");
                sw.WriteLine("         <thead>");
                sw.WriteLine("             <tr align=\"center\">");
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        sw.WriteLine("                 <th width=\"" + col.Width + "px\" class=\"thStyle\">" + col.HeaderText + "</th>");
                    }
                }
                sw.WriteLine("             </tr>");
                sw.WriteLine("         </thead>");
                BindingSource bsSource = dgv.DataSource as BindingSource;
                if (bsSource != null && !string.IsNullOrEmpty(bsSource.DataMember))
                    sw.WriteLine("<xsl:apply-templates select=\"//" + bsSource.DataMember + "\" />");
                #endregion

                #region 添加尾部
                sw.WriteLine("         </table>");
                sw.WriteLine("     </td>");
                sw.WriteLine(" </tr>");
                sw.WriteLine("</table>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
                sw.WriteLine("</xsl:template>");
                #endregion

                #region 添加取数模板
                if (bsSource != null && !string.IsNullOrEmpty(bsSource.DataMember))
                {
                    sw.WriteLine("<xsl:template match=\"" + bsSource.DataMember + "\">");
                    sw.WriteLine(" <tr>");

                    DataTable dt = ((DataSet)bsSource.DataSource).Tables[bsSource.DataMember];
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible)
                        {
                            string dataTypeName = dt.Columns[col.DataPropertyName].DataType.Name;
                            if (dataTypeName.ToLower() == "datetime")
                            {
                                //sw.WriteLine("     <td align=\"center\" class=\"tdStyle\"><xsl:value-of select=\"" + col.DataPropertyName + "\"/></td>");
                                sw.WriteLine("     <td align=\"center\" class=\"tdStyle\"><xsl:value-of select=\"substring(" + col.DataPropertyName + ",1,10)\"/> <xsl:value-of select=\"substring(" + col.DataPropertyName + ",12,12)\"/></td>");
                            }
                            else if (dataTypeName == "String")
                                sw.WriteLine("     <td align=\"center\" class=\"tdStyle\"  style=\"mso-number-format:'\\@';\"><xsl:value-of select=\"" + col.DataPropertyName + "\"/></td>");
                            else if (dataTypeName.IndexOf("Int") > 0)
                                sw.WriteLine("     <td align=\"right\" class=\"tdStyle\"><xsl:value-of select=\"" + col.DataPropertyName + "\"/></td>");
                            else if (dataTypeName == "Decimal" || dataTypeName == "Double")
                                sw.WriteLine("     <td align=\"right\" class=\"tdStyle\"><xsl:value-of select=\"format-number(" + col.DataPropertyName + ",'#0.00')\"/></td>");
                            else
                                sw.WriteLine("     <td class=\"tdStyle\"><xsl:value-of select=\"" + col.DataPropertyName + "\"/></td>");
                        }
                    }
                    sw.WriteLine(" </tr>");
                    sw.WriteLine("</xsl:template>");
                }
                #endregion

                //添加文件尾
                sw.WriteLine("</xsl:stylesheet>");
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion

        #region ExportToExcel
        /// <summary>
        /// 将数据集导出到Excel文件,带引用表
        /// </summary>
        /// <param name="ID">窗口的ID，对应功能ID及Xslt文件名</param>
        /// <param name="fileName">窗口的中文描述，CUserControl可通过ControlDesc属性获取、CForm可通过Text获得</param>
        /// <param name="dsSource">要导出到Excel的数据源</param>
        /// <param name="combobxTypes">数据源引用到的常用代码Type配置名（注意，务必给正确值）</param>
        public void ExportToExcel(string ID, string fileName, DataSet dsSource, string[] combobxTypes)
        {
            if (string.IsNullOrEmpty(ID) || dsSource == null || dsSource.Tables.Count == 0) return;

            //取Xslt文件名
            string xsltFile = xsltPath + ID.Trim() + ".xslt";
            //如果样式表文件不存在，不允许导出并提示用户
            if (!File.Exists(xsltFile))
            {
                MessageBox.Show("模板文件不存在！");
                return;
            }

            /*===================处理数据源==================*/
            //1、首先将数据源中的表拷贝到非强类型集ds中，主要是避免后面写XML文件的时候产生强类型集的声明（强类型集影响XSLT转换）
            DataSet ds = new DataSet();
            foreach (DataTable tb in dsSource.Tables)
            {
                ds.Merge(tb.Copy());
            }
            //2、如果数据源引用了其它基础代码的数据，则应根据给定的联想配置将被引用表数据从内存中获取并Merge到数据集ds中
            //if (combobxTypes != null && combobxTypes.Length > 0)
            //{
            //    foreach (string s in combobxTypes)
            //    {
            //        DataTable tb1 = CComboBoxDataSet.Instance.GetTable(s.Trim());
            //        if (tb1 != null)
            //            ds.Merge(tb1);
            //    }
            //}
            //3、对数据集中所有数据类型为数字的字段中，值为Null的默认为0
            foreach (DataTable tb in ds.Tables)
            {
                if (tb.Rows.Count == 0) continue;
                foreach (DataColumn col in tb.Columns)
                {
                    string dataType = col.DataType.Name;
                    if (dataType.Contains("Int") || dataType.Contains("Decimal") || dataType.Contains("Double"))
                    {
                        foreach (DataRow row in tb.Rows)
                        {
                            if (row[col] == DBNull.Value && String.IsNullOrEmpty(col.Expression))
                            {
                                col.ReadOnly = false;
                                row[col] = 0;
                            }
                        }
                        tb.AcceptChanges();
                    }
                }
            }
            /*==================数据源处理完毕==================*/

            try
            {
                //如果给定文件名中包含不符合文件命名规范的字符，要清除(目前主要是正、反斜杠)
                fileName = fileName.Replace("\\", "");
                fileName = fileName.Replace("/", "");
                //取Excel默认导出文件名
                string resultFile = (string.IsNullOrEmpty(fileName) ? ID.Trim() : fileName.Trim()) + ".xls";
                //选择路径并获取用户选择的文件名
                this.PathDialog.FileName = resultFile;
                if (this.PathDialog.ShowDialog() == DialogResult.OK)
                {
                    resultFile = this.PathDialog.FileName;
                }
                else
                {
                    return;
                }

                //加载xsltFile
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltFile);

                //先删除临时文件
                string tempXmlFile = xsltPath + ID.Trim() + ".xml";
                if (File.Exists(tempXmlFile))
                    File.Delete(tempXmlFile);
                //将数据源写到临时文件
                ds.WriteXml(tempXmlFile, XmlWriteMode.IgnoreSchema);

                //加载临时文件
                XPathDocument xpathdocument = new XPathDocument(tempXmlFile);
                XmlTextWriter writer = new XmlTextWriter(resultFile, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;

                //转化成Excel文件
                xslt.Transform(xpathdocument, null, writer);
                writer.Close();

                //CMessageBox.ShowInformation("CS_EXPORT_SUCCESS");
                GC.Collect();
            }
            catch (Exception e)
            {
                MessageBox.Show("导出失败，提示信息：", e.Message);
            }
        }
        #endregion

    }
}

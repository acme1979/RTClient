using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BaseModel
{
    public class StyleHelper
    {
        #region 唯一静态实例
        private StyleHelper() { }

        private static StyleHelper m_Instance;
        public static StyleHelper Instance
        {
            get
            {
                lock (typeof(StyleHelper))
                {
                    if (m_Instance == null)
                        m_Instance = new StyleHelper();
                }
                return m_Instance;
            }
        }
        #endregion

        #region SetStyle(ToolStrip ts)
        /// <summary>
        /// 设置ToolStrip的显示风格
        /// </summary>
        /// <param name="ts">需要设置显示风格的ToolStrip</param>
        public void SetStyle(ToolStrip ts)
        {
            if (ts == null)
                return;

            ts.RenderMode = ToolStripRenderMode.System;
            ts.BackColor = System.Drawing.Color.Transparent;
            ts.GripStyle = ToolStripGripStyle.Hidden;

            foreach (ToolStripItem item in ts.Items)
            {
                if (item is ToolStripButton || item is ToolStripDropDownButton)
                {
                    item.BackgroundImage = global::BaseModel.Properties.Resources.Button;
                    item.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (item is ToolStripLabel)
                {
                    item.ForeColor = System.Drawing.Color.Blue;
                }
            }
        }
        #endregion

        #region SetStyle(ToolStrip ts,RightToLeft rightToLeft)
        /// <summary>
        /// 设置ToolStrip的显示风格
        /// </summary>
        /// <param name="ts">需要设置显示风格的ToolStrip</param>
        public void SetStyle(ToolStrip ts, RightToLeft rightToLeft)
        {
            SetStyle(ts);
            if (ts != null)
                ts.RightToLeft = rightToLeft;
        }
        #endregion

        #region SetStyle(Button btn)
        /// <summary>
        /// 设置Button的显示风格
        /// </summary>
        /// <param name="ts">需要设置显示风格的Button</param>
        public void SetStyle(Button btn)
        {
            if (btn == null)
                return;

            btn.BackgroundImage = global::BaseModel.Properties.Resources.Button;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
        }
        #endregion

        #region SetStyle(ToolStripButton btn)
        /// <summary>
        /// 设置ToolStripButton的显示风格
        /// </summary>
        /// <param name="ts">需要设置显示风格的Button</param>
        public void SetStyle(ToolStripButton btn)
        {
            if (btn == null)
                return;

            btn.BackgroundImage = global::BaseModel.Properties.Resources.Button;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
        }
        #endregion

        #region SetStyle(DataGridView dgv)
        /// <summary>
        /// 设置DataGridView的显示风格
        /// </summary>
        /// <param name="dgv">需要设置显示风格的DataGridView</param>
        public void SetStyle(DataGridView dgv)
        {
            if (dgv == null) return;

            DataGridViewCellStyle columnHeadersStyle = new DataGridViewCellStyle();
            columnHeadersStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            //columnHeadersStyle.BackColor = System.Drawing.Color.Yellow;
            columnHeadersStyle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            columnHeadersStyle.ForeColor = System.Drawing.Color.Navy;
            columnHeadersStyle.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            columnHeadersStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            columnHeadersStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.ColumnHeadersDefaultCellStyle = columnHeadersStyle;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv.ColumnHeadersHeight = 22;
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgv.RowHeadersWidth = 18;
            dgv.MultiSelect = false;
            if (dgv.ReadOnly)
            {
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            else
            {
                dgv.ImeMode = ImeMode.Off;
            }

            dgv.DataError -= new DataGridViewDataErrorEventHandler(DataGridView_DataError);
            dgv.DataError += new DataGridViewDataErrorEventHandler(DataGridView_DataError);
        }

        /// <summary>
        /// 添加该事件，主要是防止出现数据不完整时抛出异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        #endregion

        #region SetGridSortMode(DataGridViewColumnCollection dgcc, DataGridViewColumnSortMode sortMode)
        /// <summary>
        /// 设置DataGridViewColumn排序模式
        /// </summary>
        /// <param name="dgcc">要设置排序模式的DataGridColumn集合</param>
        /// <param name="sortMode">排序模式</param>
        /// <remarks></remarks>
        public void SetGridSortMode(DataGridViewColumnCollection dgcc, DataGridViewColumnSortMode sortMode)
        {
            if (dgcc == null || dgcc.Count == 0) return;

            foreach (DataGridViewColumn col in dgcc)
            {
                col.SortMode = sortMode;
            }
        }
        #endregion

        #region SetStyleForDecimalCell(DataGridView dgv, DataTable tb)
        /// <summary>
        /// 将Grid中的多于两个小数点的列设置为只显示两位小数,且靠右显示
        /// </summary>
        /// <param name="dgv">要进行设置的DataGridView</param>
        /// <param name="tb">Grid绑定的数据源表，以便获取列对应的数据类型</param>
        public void SetStyleForDecimalCell(DataGridView dgv, DataTable tb)
        {
            if (dgv == null || dgv.Columns.Count == 0 || tb == null || tb.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (tb.Columns[col.DataPropertyName].DataType.Name == "Decimal")
                {
                    System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
                    cellStyle.Format = "N2";
                    cellStyle.NullValue = null;
                    cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle = cellStyle;
                }
            }
        }
        #endregion

        #region SetInputLength
        /// <summary>
        /// 将给定Panel中的文本输入框按其绑定的数据长度设置其输入长度，以防输入超长
        /// </summary>
        /// <param name="pnl">Panel</param>
        public void SetInputLength(Panel pnl)
        {
            if (pnl == null || pnl.Controls.Count == 0) return;

            foreach (Control ctl in pnl.Controls)
            {
                if (!(ctl is TextBox)) continue;
                if (ctl.DataBindings.Count == 0) continue;
                TextBox tb = (TextBox)ctl;
                if (tb.Multiline) continue;

                Binding bd = ctl.DataBindings[0];
                BindingSource bs = bd.DataSource as BindingSource;
                DataSet ds = bs.DataSource as DataSet;
                DataColumn column = ds.Tables[bs.DataMember].Columns[bd.BindingMemberInfo.BindingField];
                if (column.DataType.FullName == "System.String")
                    tb.MaxLength = column.MaxLength;
            }
        }
        #endregion

        #region SetInputLength
        /// <summary>
        /// 将给定GroupBox中的文本输入框按其绑定的数据长度设置其输入长度，以防输入超长
        /// </summary>
        /// <param name="gb">GroupBox</param>
        public void SetInputLength(GroupBox gb)
        {
            if (gb == null || gb.Controls.Count == 0) return;

            foreach (Control ctl in gb.Controls)
            {
                if (!(ctl is TextBox)) continue;
                if (ctl.DataBindings.Count == 0) continue;
                TextBox tb = (TextBox)ctl;
                if (tb.Multiline) continue;

                Binding bd = ctl.DataBindings[0];
                BindingSource bs = bd.DataSource as BindingSource;
                DataSet ds = bs.DataSource as DataSet;
                DataColumn column = ds.Tables[bs.DataMember].Columns[bd.BindingMemberInfo.BindingField];
                if (column == null) continue;

                if (column.DataType.FullName == "System.String")
                    tb.MaxLength = column.MaxLength;
            }
        }
        #endregion

        #region SetInputLength
        /// <summary>
        /// 将给定DataGridView中的文本输入框按其绑定的数据长度设置其输入长度，以防输入超长
        /// </summary>
        /// <param name="gb">GroupBox</param>
        public void SetInputLength(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;
            BindingSource bs = dgv.DataSource as BindingSource;
            if (bs == null || string.IsNullOrEmpty(bs.DataMember)) return;
            DataSet ds = bs.DataSource as DataSet;
            if (ds == null) return;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (!(column is DataGridViewTextBoxColumn)) continue;
                if (column.Visible == false || column.ReadOnly || !ds.Tables[bs.DataMember].Columns.Contains(column.DataPropertyName) ||
                    ds.Tables[bs.DataMember].Columns[column.DataPropertyName].DataType.FullName != "System.String") continue;

                DataGridViewTextBoxColumn dgvColumn = column as DataGridViewTextBoxColumn;
                dgvColumn.MaxInputLength = ds.Tables[bs.DataMember].Columns[column.DataPropertyName].MaxLength;
            }
        }
        #endregion

        #region GetRGBFromStr(string value)
        /// <summary>
        /// 将指定的字符串转化成RGB颜色值
        /// </summary>
        /// <param name="value">格式串，要求符合格式“数字1,数字2,数字3”,如“211,222,255”</param>
        /// <returns></returns>
        public System.Drawing.Color GetRGBFromStr(string value)
        {
            System.Drawing.Color color = System.Drawing.Color.White;
            if (string.IsNullOrEmpty(value))
                return color;
            string[] val = value.Split(',');
            if (val.Length != 3)
                return color;
            try
            {
                int R = Convert.ToInt32(val[0].Trim());
                int G = Convert.ToInt32(val[1].Trim());
                int B = Convert.ToInt32(val[2].Trim());
                return System.Drawing.Color.FromArgb(R, G, B);
            }
            catch
            {
                return color;
            }
        }
        #endregion

        #region OpenControlIME
        /// <summary>
        /// 打开编辑控件的IME输入法开关
        /// </summary>
        /// <param name="cc"></param>
        public void OpenControlIME(System.Windows.Forms.Control.ControlCollection cc)
        {
            if (cc == null || cc.Count == 0) return;
            foreach (Control c in cc)
            {
                if (c is TextBox || c is MaskedTextBox || c is ComboBox || c is DataGridView)
                {
                    c.ImeMode = ImeMode.On;
                    c.GotFocus += new EventHandler(Control_GotFocus);
                }
                else if (c is GroupBox || c is Panel || c is SplitContainer)
                {
                    this.OpenControlIME(c.Controls);
                }
                else
                {
                    continue;
                }
            }
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            Control c = sender as Control;
            this.FullShapeUnabled(c.Handle);
        }
        #endregion

        #region FullShapeUnabled
        /// <summary>
        /// 如果当前界面的输入法打开了，则判断是否全角，如果是则转换成半角
        /// </summary>
        /// <param name="handle"></param>
        public void FullShapeUnabled(IntPtr handle)
        {
            IntPtr HIme = WinAPI.ImmGetContext(handle);
            if (WinAPI.ImmGetOpenStatus(HIme))         //如果输入法处于打开状态 
            {
                int iMode = 0;
                int iSentence = 0;
                bool bSuccess = WinAPI.ImmGetConversionStatus(HIme, ref iMode, ref iSentence);  //检索输入法信息 
                if (bSuccess)
                {
                    if ((iMode & WinAPI.IME_CMODE_FULLSHAPE) > 0)                          //如果是全角 
                        WinAPI.ImmSimulateHotKey(handle, WinAPI.IME_CHOTKEY_SHAPE_TOGGLE);   //转换成半角 
                }
            }
        }
        #endregion

        #region Handler for dgv_ColumnHeaderMouseClick
        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            BindingSource bs = dgv.DataSource as BindingSource;
            dgv.EndEdit();
            bs.EndEdit();

            if (dgv.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn) return;

            if (e.Button == MouseButtons.Left)
            {
                string columnName = dgv.Columns[e.ColumnIndex].DataPropertyName;
                if (bs.Sort == null || bs.Sort == "")
                {
                    bs.Sort = columnName + " asc";
                }
                else
                {
                    if (bs.Sort.IndexOf(columnName + " ") < 0)
                    {
                        bs.Sort = columnName + " asc";
                    }
                    else
                    {
                        if (bs.Sort.IndexOf(" asc") < 0)
                            bs.Sort = bs.Sort.Replace(" desc", " asc");
                        else
                            bs.Sort = bs.Sort.Replace(" asc", " desc");
                    }
                }
            }
            else
            {
                bs.Sort = "";
            }
        }
        #endregion
    }
}

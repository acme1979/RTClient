using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BaseModel;

namespace WorkStation
{
    public partial class FPYQuerry : Form
    {
        private IDBHelper dbHelper;
        /// <summary>
        /// 数据库链接
        /// </summary>
        public IDBHelper FPY_DBHelper
        {
            get { return dbHelper; }
            set { dbHelper = value; }
        }

        private string data_auth;
        /// <summary>
        /// 组织机构
        /// </summary>
        public string FPY_DATA_AUTH
        {
            get { return data_auth; }
            set { data_auth = value; }
        }

        #region 构造函数
        public FPYQuerry()
        {
            InitializeComponent();
        }
        #endregion

        #region Form_Load
        private void FPYQuerry_Load(object sender, EventArgs e)
        {
            tscbbLineName.ComboBox.DataSource = SelectLineName();
            tscbbLineName.ComboBox.DisplayMember = "CA_NAME";
            tscbbLineName.ComboBox.SelectedIndex = 0;
            dateTimePicker1.Value = dateTimePicker2.Value.AddDays(-1);
        }
        #endregion

        #region Querry_Click
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string sqlstr = "";
            if (tscbbLineName.ComboBox.Text != "")
            {
                sqlstr += "AND T.PM_AREA_SN = (SELECT CA_ID FROM T_CO_AREA WHERE CA_TYPE = '1' AND T.DATA_AUTH = '" + data_auth + "' AND CA_NAME = '" + tscbbLineName.ComboBox.Text + "') ";
            }
            if (tstbModelName.Text != "")
            {
                sqlstr += "AND T.PM_MODEL_CODE = '" + tstbModelName.Text + "' ";
            }
            dataGridView1.DataSource = SelectMoNumber(sqlstr);
        }
        #endregion

        #region SQL
        private DataTable SelectLineName()
        {
            string sql = "SELECT * FROM T_CO_AREA t " +
                         "WHERE T.CA_TYPE = '1' AND T.DATA_AUTH = '" + data_auth + "' " +
                         "ORDER BY T.CA_NAME";
            DataTable dt = dbHelper.GetDataTable(sql, "T_CO_AREA");
            return dt;
        }

        private DataTable SelectMoNumber(string str)
        {
            string sql = "SELECT T.PM_MO_NUMBER 制令单号 , T.PM_PROJECT_ID 工单号 , T.PM_AREA_SN 线别 , DECODE(T.PM_PROCESS_FACE,'0','单面','1','正面','2','反面','3','阴阳面','NULL') 面别 ,"
                       + "T.PM_MODEL_CODE 机种料号, T.PM_TARGET_QTY 计划数量, T.PM_INPUT_COUNT 投入数量, T.PM_FINISH_COUNT 产出数量, T.PM_START_DATE 投入时间, T.PM_CLOSE_DATE 关结时间 "
                       + "FROM T_PM_MO_BASE t "
                       + "WHERE T.DATA_AUTH = '" + data_auth + "' AND T.PM_START_DATE IS NOT NULL "
                       + "AND T.PM_START_DATE BETWEEN TO_DATE('" + dateTimePicker1.Text + "','YYYY/MM/DD') AND TO_DATE('" + dateTimePicker2.Text + "','YYYY/MM/DD') "
                       + str
                       + "ORDER BY T.PM_START_DATE DESC";
            DataTable dt = dbHelper.GetDataTable(sql, "T_PM_MO_BASE");
            return dt;
        }

        private DataTable SelectAOIRes(string projectid)
        {
            string sql = "SELECT S1.WIP_AOI_RES 测试结果 , C1 数量 , C2 总数 , TO_CHAR(ROUND(C1/C2 , 4) * 100 , 'FM990.00') || '%' 占比 " +
                "FROM(SELECT WIP_AOI_RES, COUNT(*) C1 " +
                "FROM T_WIP_AOI_INSPECT " +
                "WHERE WIP_AOI_SN IN(SELECT WT_SN FROM T_WIP_TRACKING T WHERE T.WT_PROJECT_ID = '" + projectid + "') " +
                "GROUP BY WIP_AOI_RES) S1 " +
                "JOIN " +
                "(SELECT COUNT(*) C2 FROM T_WIP_AOI_INSPECT WHERE WIP_AOI_SN IN(SELECT WT_SN FROM T_WIP_TRACKING T WHERE T.WT_PROJECT_ID = '" + projectid + "')) S2 " +
                "ON 1 = 1";
            DataTable dt = dbHelper.GetDataTable(sql, "T_WIP_AOI_INSPECT");
            return dt;
        }
        #endregion

        #region 工单列表点击
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataTable dt01 = SelectAOIRes(dataGridView1.CurrentRow.Cells["工单号"].Value.ToString());
                dataGridView2.DataSource = dt01;
            }
        }
        #endregion
    }
}

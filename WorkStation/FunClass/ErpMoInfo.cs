using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WorkStation.FunClass
{
    public class ErpMoInfo
    {
        /// <summary>
        /// 料号
        /// </summary>
        public string FMaterialId { get; set; }
        /// <summary>
        /// 生产数量
        /// </summary>
        public int FQty { get; set; }
        /// <summary>
        /// 生产批次
        /// </summary>
        public string F_PAEZ_scpc { get; set; }
        /// <summary>
        /// BOM
        /// </summary>
        public string FBomId { get; set; }
        /// <summary>
        /// 客户代码
        /// </summary>
        public string str_FKHDM { get; set; }
        /// <summary>
        /// 辅助信息程序FNumber
        /// </summary>
        public string FProgram { get; set; }
        /// <summary>
        /// 辅助信息客户FNumber
        /// </summary>
        public string FCustmer { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string str_FModel { get; set; }
        /// <summary>
        /// 名称型号
        /// </summary>
        public string str_FName { get; set; }
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string FBillNo { get; set; }

        /// <summary>
        /// 业务流程id
        /// </summary>
        public string FBFLowId { get; set; }

        /// <summary>
        /// 生产订单id
        /// </summary>
        public string FMoId { get; set; }
        /// <summary>
        /// 生产订单当前行计划id
        /// </summary>
        public string FMoEntryId { get; set; }

        /// <summary>
        /// 生产订单当前行计划行号
        /// </summary>
        public int FSrcEntrySeq { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        public string FStockId { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string F_PAEZ_xsy { get; set; }

        /// <summary>
        /// 标准工时
        /// </summary>
        public decimal FPerUnitStandHour { get; set; }

        /// <summary>
        /// 需求单据
        /// </summary>
        public string FSaleOrderNo { get; set; }

        public string FCostRate { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string FBaseUnitId { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string str_PiHao { get; set; }

        /// <summary>
        /// SN号集合
        /// </summary>
        public List<string> FSerialNo { get; set; }

        /// <summary>
        /// 实际工时
        /// </summary>
        public decimal FHrWorkTime { get; set; }
        /// <summary>
        /// 生产类型
        /// </summary>
        public string FBillType { get; set; }

        /// <summary>
        /// 生产类型名称
        /// </summary>
        public string FBillTypeName { get; set; }

        /// <summary>
        /// 计划跟踪号
        /// </summary>
        public string FMTONO { get; set; }
        /// <summary>
        /// 客户定制需求
        /// </summary>
        public string F_PAEZ_KHDZXQ { get; set; }
        /// <summary>
        /// 计划类别  1103追加
        /// </summary>
        public string F_PAEZ_JHLB { get; set; }
        /// <summary>
        /// 生产车间  1115追加
        /// </summary>
        public string FWorkShopID { get; set; }
        /// <summary>
        /// 生产汇报类型（固定字段，【101:01、103:02】）  1115追加
        /// </summary>
        public string FReportType { get; set; }
        /// <summary>
        /// 序列号子单据体,目前只有 SN号、箱号
        /// </summary>
        public List<string[]> FSerialSubEntity { get; set; }

        public ErpMoInfo()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }
}

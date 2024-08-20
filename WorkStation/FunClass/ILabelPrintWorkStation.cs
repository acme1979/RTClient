using System;
using System.Collections.Generic;
using System.Text;

namespace WorkStation
{
    interface ILabelPrintWorkStation
    {
        string LabelTemplet { get; set; }
        string PrintDriver { get; set; }
        string PrintType { get; set; }
    }
}

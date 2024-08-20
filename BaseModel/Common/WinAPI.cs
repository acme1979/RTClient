using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; 
using System.Text;

namespace BaseModel
{
    public class WinAPI
    {
        /// <summary>
        /// 获取系统颜色
        /// 原  型：DWORD GetSysColor(int nIndex);
        /// 说  明：判断指定windows显示对象的颜色。
        /// 返回值：DWORD，指定对象的RGB颜色。
        /// </summary>
        /// <param name="nIndex">
        /// 参  数：nIndex：一个常数，指出特定的windows显示对象。取值如下：
        /// 1.    COLOR_ACTIVEBORDER  [10]：活动窗口的边框。
        /// 2.    COLOR_ACTIVECAPTION  [2]：活动窗口的标题。
        /// 3.    COLOR_APPWORKSPACE  [12]：MDI桌面的背景。
        /// 4.    COLOR_BACKGROUND  [1]：windows桌面。
        /// 5.    COLOR_BTNFACE  [15]：按钮。
        /// 6.    COLOR_BTNHIGHLIGHT  [20]：按钮的3D加亮区。
        /// 7.    COLOR_BTNSHADOW  [16]：按钮的3D阴影。
        /// 8.    COLOR_BTNTEXT  [18]：按钮文字。
        /// 9.    COLOR_CAPTIONTEXT  [9]：窗口标题中的文字。
        /// 10.   COLOR_GRAYTEXT  [17]：灰色文字；如使用了抖动技术则为零。
        /// 11.   COLOR_HIGHLIGHT  [13]：选定的项目背景。
        /// 12.   COLOR_HIGHLIGHTTEXT  [14]：选定的项目文字。
        /// 13.   COLOR_INACTIVEBORDER  [11]：不活动窗口的边框。
        /// 14.   COLOR_INACTIVECAPTION  [3]:不活动窗口的标题。
        /// 15.   COLOR_INACTIVECAPTIONTEXT  [19]：不活动窗口的文字。
        /// 16.   COLOR_MENU  [4]：菜单。
        /// 17.   COLOR_MENUTEXT  [7]：菜单正文。
        /// 18.   COLOR_SCROLLBAR  [0]：滚动条。
        /// 19.   COLOR_WINDOW  [5]：窗口背景；
        /// 20.   COLOR_WINDOWFRAME  [6]：窗框
        /// 21.   COLOR_WINDOWTEXT  [8]：窗口正文。
        /// 22.   COLOR_3DDKSHADOW  [21]：3D深阴影。
        /// 23.   COLOR_3DFACE  [COLOR_BTNFACE]：3D阴影化对象的正面颜色。
        /// 24.   COLOR_3DHILIGHT [COLOR_BTNHIGHLIGHT]：3D加亮颜色（win95）。
        /// 25.   COLOR_3DLIGHT  [22]：3D阴影化对象的浅色。
        /// 26.   COLOR_INFOBK  [24]：工具提示的背景色。
        /// 27.   COLOR_INFOTEXT  [23]：工具提示的文本色。
        /// 28.   COLOR_HOTLIGHT  [26]：
        /// 29.   COLOR_GRADIENTACTIVECAPTION [27]：
        /// 30.   COLOR_GRADIENTINACTIVECAPTION [28]：
        /// 31.   COLOR_DESKTOP  [COLOR_BACKGROUND]:桌面颜色。
        /// 32.   COLOR_3DHIGHLIGHT  [COLOR_BTNHIGHLIGHT]:
        /// 33.   COLOR_BTNHILIGHT  [COLOR_BTNHIGHLIGHT]:
        /// 其中，中括号里面的为对应值就是要调用的参数值
        ///       序号22、23、25、26、27、31、32、33需要windows4.0版本以上支持;
        ///       序号28、29、30需要windows5.0版本以上支持。 
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetSysColor")]
        public static extern int GetSysColor(int nIndex);
        /// <summary>
        /// 获取开关键状态
        /// </summary>
        /// <param name="nVirtKey">NumLock、CapsLock等</param>
        /// <returns>1为打开状态，否则为关闭状态</returns>
        [DllImport("user32.dll")]
        public static extern int GetKeyState(int nVirtKey);

        #region 声明一些API函数，用于获取或设置输入法属性
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr himc);
        [DllImport("imm32.dll")]
        public static extern bool ImmSetOpenStatus(IntPtr himc, bool b);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);
        [DllImport("imm32.dll")]
        public static extern int ImmSimulateHotKey(IntPtr hwnd, int lngHotkey);
        public const int IME_CMODE_FULLSHAPE = 0x8;
        public const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;
        #endregion
    }
}

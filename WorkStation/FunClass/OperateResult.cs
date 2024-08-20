using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkStation.FunClass
{
    //
    // 摘要:
    //     操作结果
    public class OperateResult
    {
        //
        // 摘要:
        //     是否成功
        public bool IsSuccess { get; set; }

        //
        // 摘要:
        //     消息
        public string Message { get; set; } = string.Empty;


        //
        // 摘要:
        //     代码
        public int Code { get; set; }

        //
        // 摘要:
        //     使用指定的消息实例化一个默认的结果对象
        public OperateResult()
        {
        }

        //
        // 摘要:
        //     使用指定的消息实例化一个默认的结果对象
        //
        // 参数:
        //   msg:
        //     错误消息
        public OperateResult(string msg)
        {
            Message = msg;
        }

        //
        // 摘要:
        //     使用错误代码，消息文本来实例化对象
        //
        // 参数:
        //   errorCode:
        //     错误代码
        //
        //   msg:
        //     错误消息
        public OperateResult(int errorCode, string msg)
        {
            Message = msg;
            Code = errorCode;
        }

        //
        // 摘要:
        //     创建并返回一个成功的结果对象
        //
        // 返回结果:
        //     成功的结果对象
        public static OperateResult CreateSuccessResult()
        {
            return new OperateResult
            {
                IsSuccess = true,
                Code = 0,
                Message = "成功"
            };
        }

        //
        // 摘要:
        //     创建一个失败结果
        //
        // 参数:
        //   message:
        //
        //   code:
        public static OperateResult CreateFailResult(string message, int code = 1)
        {
            return new OperateResult
            {
                IsSuccess = false,
                Code = code,
                Message = message
            };
        }
    }
}

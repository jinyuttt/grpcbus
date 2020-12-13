#region   文件版本注释
/************************************************************************
*CLR版本  ：4.0.30319.42000
*机器名称 ：DESKTOP-TJ76JPF
*项目名称 ：ClientConsole
*项目描述 ：
*命名空间 ：ClientConsole
*文件名称 ：ClientLoggerInterceptor.cs
*版本号   :   2020|V1.0.0.0 
---------------------------------------------------------------------
* Copyright @ jinyu 2020. All rights reserved.
---------------------------------------------------------------------

***********************************************************************/
#endregion

using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientConsole
{

   /// <summary>
   /// 客户端拦截器
   /// </summary>
  public  class ClientLoggerInterceptor: Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
       TRequest request,
       ClientInterceptorContext<TRequest, TResponse> context,
       AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            LogCall(context.Method);

            return continuation(request, context);
        }

        private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method)
            where TRequest : class
            where TResponse : class
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
            Console.ForegroundColor = initialColor;
        }
    }
}

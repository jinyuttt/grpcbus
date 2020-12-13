using System.Text;

namespace SrvClient
{

    /// <summary>
    /// 类型转换
    /// </summary>
    public static class RpcBusConvert
    {
        /// <summary>
        /// string转ByteString
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Google.Protobuf.ByteString ConvertString(this string msg)
        {
            return Google.Protobuf.ByteString.CopyFrom(Encoding.Default.GetBytes(msg));
        }

        /// <summary>
        /// byte[]转ByteString
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Google.Protobuf.ByteString ConvertString(this byte[] msg)
        {
            return Google.Protobuf.ByteString.CopyFrom(msg);
        }

        /// <summary>
        /// ByteString转String
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ConvertString(this Google.Protobuf.ByteString msg)
        {
            return msg.ToString(Encoding.UTF8);
        }

        /// <summary>
        /// ByteString转byte[]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ConvertBytes(this Google.Protobuf.ByteString msg)
        {
            return msg.ToByteArray();
        }

        /// <summary>
        /// 结构类型转换
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static  gRpcBus.BusRequest  ConvertRequest(this RpcRequest request)
        {
            var req= new gRpcBus.BusRequest()
            {
                Active = request.Active,
                Id = request.Id,
                Name = request.SrvName,
            };
            if (!string.IsNullOrEmpty(request.ReqJson))
            {
                req.ReqJson.Add(request.ReqJson);
            }
            if (request.ReqBytes != null)
            {
                req.ReqBytes.Add(request.ReqBytes.ConvertString());
            }
            return req;
        }

        /// <summary>
        /// 结构类型转换
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static gRpcBus.BusReply ConvertRequest(this RspResult result)
        {
            var req = new gRpcBus.BusReply()
            {
                  Id=result.Id,
                  Reqid=result.ReqId,
                   
            };
            if (!string.IsNullOrEmpty(result.RspJson))
            {
                req.RspJson.Add(result.RspJson);
            }
            if (result.RspBytes != null)
            {
                req.RspBytes.Add(result.RspBytes.ConvertString());
            }
            return req;
        }

        /// <summary>
        /// 结构类型转换
        /// </summary>
        /// <param name="rsp"></param>
        /// <returns></returns>
        public static RspResult ConvertResult(this gRpcBus.BusReply rsp)
        {
            var result = new RspResult()
            {
                Id = rsp.Id,
              ReqId= rsp.Reqid,
               
            };
            if (rsp.RspJson.Count>0)
            {
                result.RspJson = rsp.RspJson[0];
            }
            if (rsp.RspBytes != null&& rsp.RspBytes.Count>0)
            {
                result.RspBytes = rsp.RspBytes[0].ConvertBytes();
            }
            return result;
        }
    }
}

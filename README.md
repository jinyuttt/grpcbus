# grpcbus
定义统一的Rpc接口，采用grpc作为传输组件
## rpc传输  
   封装了客户端，服务端。  
   4种模式对应各种功能应用；  
## 实体  
 1.客户端的请求实体RpcRequest  
    ReqJson属性：传输实体JSON格式  
	ReqBytes属性:传输实体byte[]  
	SrvName属性：作为C/S模式时，可设置为服务名称  
	Active属性:作为C/S模式时，可设置为服务方法名称  
	Id属性：则使用时作为唯一请求ID  
2. 返回实体RspResult  
    RspJson属性：返回实体的JSON格式  
	RspBytes属性：返回实体的Byte[]  
	ReqId属性:对应的客户端ID  
	ID属性：响应的服务ID  
	ErrorCode属性：作为C/S模式时，错误编码  
	ErrorMsg属性：作为C/S模式时，错误信息
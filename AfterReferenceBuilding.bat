CLS
CD %1 

rem 在客户端、服务端的目录中，创建bin\Debug目录
for %%a in (RSNClient RTServer) do (if NOT EXIST %1%%a\bin\Debug MD %1%%a\bin\Debug)

rem 在客户端、服务端的目录中,创建bin\Debug\Config目录
for %%b in (RSNClient RTServer) do (if NOT EXIST %1%%b\bin\Debug\Config MD %1%%b\bin\Debug\Config)

rem 在客户端、服务端的目录中,创建bin\Debug\Config\sql目录
for %%c in (RSNClient RTServer) do (if NOT EXIST %1%%c\bin\Debug\Config\sql MD %1%%c\bin\Debug\Config\sql)

rem 在客户端目录中，创建bin\Debug\Config\Resources目录,
for %%e in (RSNClient) do (if NOT EXIST %1%%e\bin\Debug\Config\Resources MD %1\%%e\bin\Debug\Config\Resources)

rem 在客户端目录中，创建bin\Debug\Images目录
for %%i in (RSNClient) do (if NOT EXIST %1%%i\bin\Debug\Images MD %1%%i\bin\Debug\Images) 

rem 在客户端目录中，创建bin\Debug\Images\System目录
for %%j in (RSNClient) do (if NOT EXIST %1%%j\bin\Debug\Images\System MD %1%%j\bin\Debug\Images\System) 


rem Copy DLL目录中所有文件到客户端、服务端目录
for %%p in (RSNClient RTServer) do (copy "%2Dll\*.*" "%1%%p\bin\Debug")

rem Copy Config目录中所有文件到客户端、服务端目录
for %%q in (RSNClient RTServer) do (copy "%2Config\*.*" "%1%%q\bin\Debug\Config")

rem Copy Config\sql目录中所有文件到客户端、服务端目录
for %%r in (RSNClient RTServer) do (copy "%2Config\sql\*.*" "%1%%r\bin\Debug\Config\sql")

rem Copy Config\Resources目录中所有文件到客户端目录
for %%s in (RSNClient ) do (copy "%2Config\Resources\*.*" "%1%%s\bin\Debug\Config\Resources")

rem Copy Images目录中所有文件到客户端目录
for %%t in (RSNClient ) do (copy "%2Images\*.*" "%1%%t\bin\Debug\Images")

rem Copy Images\System目录中所有文件到客户端目录
for %%u in (RSNClient ) do (copy "%2Images\System\*.*" "%1%%u\bin\Debug\Images\System")
CLS
CD %1 

rem �ڿͻ��ˡ�����˵�Ŀ¼�У�����bin\DebugĿ¼
for %%a in (RSNClient RTServer) do (if NOT EXIST %1%%a\bin\Debug MD %1%%a\bin\Debug)

rem �ڿͻ��ˡ�����˵�Ŀ¼��,����bin\Debug\ConfigĿ¼
for %%b in (RSNClient RTServer) do (if NOT EXIST %1%%b\bin\Debug\Config MD %1%%b\bin\Debug\Config)

rem �ڿͻ��ˡ�����˵�Ŀ¼��,����bin\Debug\Config\sqlĿ¼
for %%c in (RSNClient RTServer) do (if NOT EXIST %1%%c\bin\Debug\Config\sql MD %1%%c\bin\Debug\Config\sql)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Config\ResourcesĿ¼,
for %%e in (RSNClient) do (if NOT EXIST %1%%e\bin\Debug\Config\Resources MD %1\%%e\bin\Debug\Config\Resources)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\ImagesĿ¼
for %%i in (RSNClient) do (if NOT EXIST %1%%i\bin\Debug\Images MD %1%%i\bin\Debug\Images) 

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Images\SystemĿ¼
for %%j in (RSNClient) do (if NOT EXIST %1%%j\bin\Debug\Images\System MD %1%%j\bin\Debug\Images\System) 


rem Copy DLLĿ¼�������ļ����ͻ��ˡ������Ŀ¼
for %%p in (RSNClient RTServer) do (copy "%2Dll\*.*" "%1%%p\bin\Debug")

rem Copy ConfigĿ¼�������ļ����ͻ��ˡ������Ŀ¼
for %%q in (RSNClient RTServer) do (copy "%2Config\*.*" "%1%%q\bin\Debug\Config")

rem Copy Config\sqlĿ¼�������ļ����ͻ��ˡ������Ŀ¼
for %%r in (RSNClient RTServer) do (copy "%2Config\sql\*.*" "%1%%r\bin\Debug\Config\sql")

rem Copy Config\ResourcesĿ¼�������ļ����ͻ���Ŀ¼
for %%s in (RSNClient ) do (copy "%2Config\Resources\*.*" "%1%%s\bin\Debug\Config\Resources")

rem Copy ImagesĿ¼�������ļ����ͻ���Ŀ¼
for %%t in (RSNClient ) do (copy "%2Images\*.*" "%1%%t\bin\Debug\Images")

rem Copy Images\SystemĿ¼�������ļ����ͻ���Ŀ¼
for %%u in (RSNClient ) do (copy "%2Images\System\*.*" "%1%%u\bin\Debug\Images\System")
CLS
CD %1 

rem �ڿͻ��ˡ�����˵�Ŀ¼�У�����bin\DebugĿ¼
for %%a in (RSNClient RTServer) do (if NOT EXIST %1%%a\bin\Debug MD %1%%a\bin\Debug)

rem ��WebCenterĿ¼�У�����Bin��Config��Config\sqlĿ¼
rem if NOT EXIST %1WebCenter\Bin MD %1WebCenter\Bin
rem if NOT EXIST %1WebCenter\Config MD %1WebCenter\Config
rem if NOT EXIST %1WebCenter\Config\sql MD %1WebCenter\Config\sql

rem �ڿͻ��ˡ�����˵�Ŀ¼��,����bin\Debug\ConfigĿ¼
rem for %%b in (RSNClient RTServer) do (if NOT EXIST %1%%b\bin\Debug\Config MD %1%%b\bin\Debug\Config)

rem �ڿͻ��ˡ�����˵�Ŀ¼��,����bin\Debug\Config\sqlĿ¼
for %%c in (RSNClient RTServer) do (if NOT EXIST %1%%c\bin\Debug\Config\sql MD %1%%c\bin\Debug\Config\sql)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Config\ValidateĿ¼,
for %%d in (RSNClient) do (if NOT EXIST %1%%d\bin\Debug\Config\Validate MD %1\%%d\bin\Debug\Config\Validate)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Config\ResourcesĿ¼,
for %%e in (RSNClient) do (if NOT EXIST %1%%e\bin\Debug\Config\Resources MD %1\%%e\bin\Debug\Config\Resources)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Config\ReportĿ¼,
for %%f in (RSNClient) do (if NOT EXIST %1%%f\bin\Debug\Config\Report MD %1%%f\bin\Debug\Config\Report)

rem �ڿͻ��ˡ������Ŀ¼�У�����bin\Debug\ReportResultĿ¼
rem for %%g in (RSNClient RTServer) do (if NOT EXIST %1%%g\bin\Debug\ReportResult MD %1%%g\bin\Debug\ReportResult)

rem �ڿͻ���Ŀ¼�У�����bin\Debug\XsltĿ¼
for %%h in (RSNClient) do (if NOT EXIST %1%%h\bin\Debug\Xslt MD %1%%h\bin\Debug\Xslt) 

rem �ڿͻ���Ŀ¼�У�����bin\Debug\ImagesĿ¼
for %%i in (RSNClient) do (if NOT EXIST %1%%i\bin\Debug\Images MD %1%%i\bin\Debug\Images) 

rem �ڿͻ���Ŀ¼�У�����bin\Debug\Images\SystemĿ¼
for %%j in (RSNClient) do (if NOT EXIST %1%%j\bin\Debug\Images\System MD %1%%j\bin\Debug\Images\System) 


rem Copy *.Dal.dll/pdb���ͻ��ˡ������Ŀ¼
set ProjNm=%3
if "%ProjNm:~-4%"==".Dal" (
	for %%p in (RSNClient RTServer) do (copy "%2bin\Debug\%3.dll" "%1%%p\bin\Debug")
	for %%q in (RSNClient RTServer) do (copy "%2bin\Debug\%3.pdb" "%1%%q\bin\Debug")

	for %%r in (RSNClient RTServer) do (
		if exist %2%ProjNm:~0,-3%sql.Config copy "%2%ProjNm:~0,-3%sql.Config" "%1%%r\bin\Debug\Config\sql")

	rem Copy *.Dal.dll/pdb��WebCenterĿ¼
	rem copy "%2bin\Debug\%3.dll" "%1WebCenter\Bin"
	rem copy "%2bin\Debug\%3.pdb" "%1WebCenter\Bin"
	rem if exist %2%ProjNm:~0,-3%sql.Config copy "%2%ProjNm:~0,-3%sql.Config" "%1WebCenter\Config\sql"
) else (
    rem  Copy *.Win.dll/pdb���ͻ���Ŀ¼��Copy *.Biz.dll/pdb�Լ�*.Model.dll/pdb���ͻ��˼���������Ŀ¼��
    if "%ProjNm:~-4%"==".Win" (
	for %%x in (RSNClient) do (copy "%2bin\Debug\%3.dll" "%1%%x\bin\Debug")
	for %%y in (RSNClient) do (copy "%2bin\Debug\%3.pdb" "%1%%y\bin\Debug")
 
 	for %%z in (RSNClient) do (
 		if exist %2%ProjNm:~0,-3%Validate.Config copy "%2%ProjNm:~0,-3%Validate.Config" "%1%%z\bin\Debug\Config\Validate")
	for %%z in (RSNClient) do (
		if exist %2Xslt\*.xslt copy "%2Xslt\*.xslt" "%1%%z\bin\Debug\Xslt")
    ) else (
 	for %%w in (RSNClient RTServer) do (copy "%2bin\Debug\%3.*" "%1%%w\bin\Debug")
	
	rem rem Copy *.Biz.dll/pdb��*.Model.dll/pdb��*.Common.dll/pdb��WebCenterĿ¼
	rem copy "%2bin\Debug\%3.dll" "%1WebCenter\Bin"
	rem copy "%2bin\Debug\%3.pdb" "%1WebCenter\Bin"
    )
)

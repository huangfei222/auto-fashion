@echo off
rem 创建必要文件夹
if not exist "config" mkdir config
if not exist "scripts" mkdir scripts

rem 移动JSON文件
echo 正在整理JSON文件...
for %%f in (*.json) do (
    if not exist "config\%%~nxf" (
        move "%%f" config\ >nul
        echo 已移动：%%f ➔ config\
    ) else (
        echo 跳过已存在：config\%%~nxf
    )
)

rem 移动Python文件
echo 正在整理Python脚本...
for %%f in (*.py) do (
    if exist "scripts\%%~nxf" (
        copy "%%f" "scripts\%%~nf_备份%%~xf" >nul
        echo 已创建备份：scripts\%%~nf_备份%%~xf
    ) else (
        move "%%f" scripts\ >nul
        echo 已移动：%%f ➔ scripts\
    )
)

echo 整理完成！
pause
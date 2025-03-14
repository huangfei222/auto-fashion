@echo off
title AI模板生成系统
color 0A
setlocal enabledelayedexpansion

echo.
echo ██████╗ ██╗███╗   ██╗███████╗
echo ██╔══██╗██║████╗  ██║██╔════╝
echo ██████╔╝██║██╔██╗ ██║█████╗  
echo ██╔══██╗██║██║╚██╗██║██╔══╝  
echo ██║  ██║██║██║ ╚████║███████╗
echo ╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚══════╝
echo.

:menu
cls
echo 请选择操作：
echo 1. 生成全部模板
echo 2. 仅生成PPT模板
echo 3. 仅生成设计素材
echo 4. 打开输出目录
echo 5. 查看日志
echo 6. 退出
set /p choice="请输入选项数字："

if "%choice%"=="1" goto all
if "%choice%"=="2" goto ppt
if "%choice%"=="3" goto design
if "%choice%"=="4" goto open
if "%choice%"=="5" goto log
if "%choice%"=="6" exit

goto menu

:all
call :run_script "ppt_generator.py"
call :run_script "card_maker.py"
call :run_script "table_builder.py"
call :run_script "EPUB电子书.py"
goto success

:ppt
call :run_script "ppt_generator.py"
goto success

:design
call :run_script "card_maker.py"
call :run_script "table_builder.py"
goto success

:open
start explorer "D:\AI_System\data\raw"
goto menu

:log
notepad "D:\AI_System\scripts\gen_log.txt"
goto menu

:run_script
echo [%time%] 正在执行：%~1
python "%~1" >> "D:\AI_System\scripts\gen_log.txt" 2>&1
if %errorlevel% neq 0 (
    echo [%time%] 执行失败：%~1 >> "D:\AI_System\scripts\gen_log.txt"
    exit /b 1
)
exit /b 0

:success
echo 操作已完成！按任意键返回菜单...
pause >nul
goto menu
@echo off
setlocal enabledelayedexpansion

echo 正在清理旧数据...
del /q D:\AI_System\data\raw\* /s >nul 2>&1

echo 正在生成PPT模板...
call :run_script ppt_generator.py
if errorlevel 1 (
    echo PPT模板生成失败，错误码：!errorlevel!
    exit /b 1
)

echo 正在生成各类请柬...
call :run_script card_maker.py
if errorlevel 1 (
    echo 请柬生成失败，错误码：!errorlevel!
    exit /b 1
)

echo 正在生成表格模板...
call :run_script table_builder.py
if errorlevel 1 (
    echo 表格生成失败，错误码：!errorlevel!
    exit /b 1
)

echo 正在生成电子书模板...
call :run_script EPUB电子书.py
if errorlevel 1 (
    echo 电子书生成失败，错误码：!errorlevel!
    exit /b 1
)

echo 正在打开成果目录...
timeout /t 2 >nul
explorer D:\AI_System\data\raw
exit /b 0

:run_script
set "script=%~1"
echo [%time%] 开始执行：%script%
python "D:\AI_System\scripts\%script%"
if errorlevel 1 (
    echo [%time%] 执行失败：%script%
    exit /b 1
)
echo [%time%] 成功完成：%script%
exit /b 0
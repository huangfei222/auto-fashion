@echo off
setlocal enabledelayedexpansion

echo �������������...
del /q D:\AI_System\data\raw\* /s >nul 2>&1

echo ��������PPTģ��...
call :run_script ppt_generator.py
if errorlevel 1 (
    echo PPTģ������ʧ�ܣ������룺!errorlevel!
    exit /b 1
)

echo �������ɸ������...
call :run_script card_maker.py
if errorlevel 1 (
    echo �������ʧ�ܣ������룺!errorlevel!
    exit /b 1
)

echo �������ɱ��ģ��...
call :run_script table_builder.py
if errorlevel 1 (
    echo �������ʧ�ܣ������룺!errorlevel!
    exit /b 1
)

echo �������ɵ�����ģ��...
call :run_script EPUB������.py
if errorlevel 1 (
    echo ����������ʧ�ܣ������룺!errorlevel!
    exit /b 1
)

echo ���ڴ򿪳ɹ�Ŀ¼...
timeout /t 2 >nul
explorer D:\AI_System\data\raw
exit /b 0

:run_script
set "script=%~1"
echo [%time%] ��ʼִ�У�%script%
python "D:\AI_System\scripts\%script%"
if errorlevel 1 (
    echo [%time%] ִ��ʧ�ܣ�%script%
    exit /b 1
)
echo [%time%] �ɹ���ɣ�%script%
exit /b 0
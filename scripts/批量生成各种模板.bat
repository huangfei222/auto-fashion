@echo off
title AIģ������ϵͳ
color 0A
setlocal enabledelayedexpansion

echo.
echo �������������[ �����[�������[   �����[���������������[
echo �����X�T�T�����[�����U���������[  �����U�����X�T�T�T�T�a
echo �������������X�a�����U�����X�����[ �����U�����������[  
echo �����X�T�T�����[�����U�����U�^�����[�����U�����X�T�T�a  
echo �����U  �����U�����U�����U �^���������U���������������[
echo �^�T�a  �^�T�a�^�T�a�^�T�a  �^�T�T�T�a�^�T�T�T�T�T�T�a
echo.

:menu
cls
echo ��ѡ�������
echo 1. ����ȫ��ģ��
echo 2. ������PPTģ��
echo 3. ����������ز�
echo 4. �����Ŀ¼
echo 5. �鿴��־
echo 6. �˳�
set /p choice="������ѡ�����֣�"

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
call :run_script "EPUB������.py"
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
echo [%time%] ����ִ�У�%~1
python "%~1" >> "D:\AI_System\scripts\gen_log.txt" 2>&1
if %errorlevel% neq 0 (
    echo [%time%] ִ��ʧ�ܣ�%~1 >> "D:\AI_System\scripts\gen_log.txt"
    exit /b 1
)
exit /b 0

:success
echo ��������ɣ�����������ز˵�...
pause >nul
goto menu
@echo off
setlocal enabledelayedexpansion

:: ������
set INPUT_DIR=D:\AI_System\data\raw\����ģ���ز�\�������
set OUTPUT_DIR=D:\AI_System\output\������Ʒ\PSDģ��\�������

:: �������Ŀ¼
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

:: ������
for %%i in ("%INPUT_DIR%\*.png") do (
    echo ���ڴ���%%~nxi
    magick "%%i" -font Arial -pointsize 50 -fill black -annotate +100+100 "˫���༭����" "%OUTPUT_DIR%\%%~ni.psd"
)

echo ������ɣ������ļ��б�
dir /b "%OUTPUT_DIR%\*.psd"
pause
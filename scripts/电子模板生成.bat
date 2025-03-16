@echo off
setlocal enabledelayedexpansion

:: 配置区
set INPUT_DIR=D:\AI_System\data\raw\电子模板素材\婚礼请柬
set OUTPUT_DIR=D:\AI_System\output\电子商品\PSD模板\婚礼请柬

:: 创建输出目录
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

:: 主处理
for %%i in ("%INPUT_DIR%\*.png") do (
    echo 正在处理：%%~nxi
    magick "%%i" -font Arial -pointsize 50 -fill black -annotate +100+100 "双击编辑文字" "%OUTPUT_DIR%\%%~ni.psd"
)

echo 处理完成！生成文件列表：
dir /b "%OUTPUT_DIR%\*.psd"
pause
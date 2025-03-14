@echo off
setlocal enabledelayedexpansion

:: 配置区
set "INPUT=D:\AI_System\assets\wallpaper\raw"
set "OUTPUT=D:\AI_System\assets\wallpaper\processed"
set "WATERMARK=D:\AI_System\assets\watermark\logo.png"

:: 创建水印（如果不存在）
if not exist "%WATERMARK%" (
   magick -size 600x200 xc:none ^
      -fill "rgba(255,255,255,0.3)" ^
      -font Arial ^
      -pointsize 36 ^
      -gravity center ^
      -annotate 0 "AI Design" ^
      "%WATERMARK%"
)

:: 主处理循环
:loop
for %%F in ("%INPUT%\*.png") do (
   if not exist "%OUTPUT%\%%~nF.done" (
      magick "%%F" ^
         -resize "3840x2160^" ^
         -gravity center -extent 3840x2160 ^
         "%WATERMARK%" -gravity southeast -geometry +50+50 -composite ^
         -quality 85 ^
         "%OUTPUT%\%%~nF.webp"
      echo %date% %time% > "%OUTPUT%\%%~nF.done"
   )
)
timeout /t 5 >nul
goto loop
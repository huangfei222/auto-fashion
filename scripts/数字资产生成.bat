@echo off & chcp 65001 > nul
setlocal enabledelayedexpansion

:: ---------- 配置区 ----------
set "BASE=D:\AI_System"
set "SOURCE=%BASE%\data\raw\digital_assets"
set "OUTPUT=%BASE%\output\digital_output"
set "LIBREOFFICE=C:\Program Files\LibreOffice\program\soffice.exe"

:: ---------- 创建目录 ----------
mkdir "%OUTPUT%\ppt_pdf" 2>nul
mkdir "%OUTPUT%\excel_pdf" 2>nul
mkdir "%OUTPUT%\epub_books" 2>nul

:: ---------- PPT转PDF ----------
echo === 开始转换PPT文件 ===
for /R "%SOURCE%\ppt_templates" %%i in (*.pptx) do (
  echo 正在处理: %%~nxi
  "%LIBREOFFICE%" --headless --convert-to pdf "%%i" --outdir "%OUTPUT%\ppt_pdf"
  if exist "%OUTPUT%\ppt_pdf\%%~ni.pdf" (
    echo [成功] %%~nxi 已转换为PDF
  ) else (
    echo [失败] %%~nxi 转换失败
  )
)

:: ---------- Excel转PDF ----------
echo === 开始转换Excel文件 ===
for /R "%SOURCE%\excel_templates" %%i in (*.xlsx) do (
  echo 正在处理: %%~nxi
  "%LIBREOFFICE%" --headless --convert-to pdf "%%i" --outdir "%OUTPUT%\excel_pdf"
  if exist "%OUTPUT%\excel_pdf\%%~ni.pdf" (
    echo [成功] %%~nxi 已转换为PDF
  ) else (
    echo [失败] %%~nxi 转换失败
  )
)

:: ---------- 生成电子书 ----------
echo === 开始生成电子书 ===
if not exist "%SOURCE%\ebook_templates\*.docx" (
  echo 警告：未找到电子书模板文件！
  goto :end
)
xcopy /Y "%SOURCE%\ebook_templates\*.docx" "%OUTPUT%\epub_books\" >nul
cd /d "%OUTPUT%\epub_books"
for %%i in (*.docx) do (
  echo 正在转换: %%~nxi
  pandoc "%%i" -o "%%~ni.epub"
  if exist "%%~ni.epub" (
    echo [成功] %%~ni.epub 已生成
  ) else (
    echo [失败] %%~ni.epub 生成失败
  )
)

:end
echo === 所有操作已完成 ===
pause
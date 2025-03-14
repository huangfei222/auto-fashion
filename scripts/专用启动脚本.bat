@echo off
title 宗教艺术品生产系统
color 0a

:: 强制设置工作目录
cd /d D:\AI_System

:: 激活虚拟环境
call venv\Scripts\activate.bat

:: 带参数启动
python scripts/stable_factory.py %*

pause
@echo off
cd /d D:\AI_System
python auto_shop_v6.py --upload >> logs\%date:~0,4%%date:~5,2%%date:~8,2%.log 2>&1
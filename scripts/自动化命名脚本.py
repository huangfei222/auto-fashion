# -*- coding: utf-8 -*-
import os
import time
from pathlib import Path

def auto_rename(folder_path):
    """
    自动命名规则：类型_日期_序号.扩展名
    示例：PPT_20250315_001.pptx
    """
    categories = {
        'ppt': ['pptx', 'ppt'],
        'image': ['png', 'jpg'],
        'table': ['xlsx', 'csv']
    }
    
    for root, dirs, files in os.walk(folder_path):
        for idx, file in enumerate(files, 1):
            # 获取文件信息
            ext = file.split('.')[-1].lower()
            file_type = next((k for k,v in categories.items() if ext in v), 'other')
            date_str = time.strftime("%Y%m%d")
            
            # 生成新文件名
            new_name = f"{file_type}_{date_str}_{idx:03d}.{ext}"
            src = Path(root) / file
            dest = Path(root) / new_name
            
            # 执行重命名
            src.rename(dest)
            print(f"✅ 已重命名：{file} → {new_name}")

if __name__ == "__main__":
    path = "D:/AI_System/data/raw/电子模板素材"
    auto_rename(path)
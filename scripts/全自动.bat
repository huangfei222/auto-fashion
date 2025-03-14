# -*- coding: utf-8 -*-
"""
全自动宗教数字资产生产系统 v1.3
功能包含：
1. 自动去背景
2. 商品图生成
3. 多尺寸壁纸生产
4. 设计模板套装生成
"""

# ==================== 导入依赖 ====================
import os
import time
import shutil
from pathlib import Path
from PIL import Image
from rembg import remove
from rembg.session_factory import new_session
from reportlab.pdfgen import canvas
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from psd_tools import PSDImage

# ==================== 配置参数 ====================
# 路径配置（请确保这些目录已创建）
INPUT_DIR = r"D:\AI_System\data\raw"            # 原始图片目录
PROCESSED_DIR = r"D:\AI_System\data\processed"  # 基础成品目录
WALLPAPER_DIR = r"D:\AI_System\data\templates"  # 壁纸目录
TEMPLATE_DIR = r"D:\AI_System\data\模板目录"    # 模板目录
LOG_FILE = r"D:\AI_System\logs"  # 生产日志

# 壁纸尺寸配置
WALLPAPER_SIZES = {
    "Phone": (1080, 2400),    # 手机壁纸 (9:21)
    "Desktop": (3840, 2160),  # 电脑壁纸 (16:9)
    "Tablet": (2048, 1536)    # 平板壁纸 (4:3)
}

# ==================== 初始化设置 ====================
# 注册中文字体（将simhei.ttf放入脚本目录）
try:
    pdfmetrics.registerFont(TTFont('SimHei', 'simhei.ttf'))
except:
    print("→ 未检测到中文字体，使用默认字体")

# 创建会话（CPU优化模式）
session = new_session("u2netp", providers=["CPUExecutionProvider"])

# ==================== 功能函数 ====================
def generate_wallpapers(clean_img: Image.Image, design_name: str):
    """生成多尺寸壁纸"""
    try:
        for device, size in WALLPAPER_SIZES.items():
            output_path = Path(WALLPAPER_DIR) / device / f"{design_name}_{device}.png"
            resized_img = clean_img.resize(size, Image.LANCZOS)
            resized_img.save(output_path)
            print(f"    → 生成 {device} 壁纸：{output_path.name}")
    except Exception as e:
        print(f"    ❌ 壁纸生成失败：{str(e)}")

def create_template_pack(clean_img: Image.Image, design_name: str):
    """生成设计模板套装"""
    try:
        # 创建PSD文件
        psd_path = Path(TEMPLATE_DIR) / "PSD" / f"{design_name}.psd"
        psd = PSDImage.new()
        layer = psd.add_layer(name='主设计图层')
        layer.image = clean_img
        psd.save(psd_path)
        print(f"    → 生成PSD模板：{psd_path.name}")

        # 创建PDF说明书
        pdf_path = Path(TEMPLATE_DIR) / "PDF" / f"{design_name}_使用指南.pdf"
        c = canvas.Canvas(str(pdf_path))
        c.setFont("SimHei", 14)
        c.drawString(100, 800, f"{design_name} 使用说明书")
        c.drawImage(str(psd_path), 50, 500, width=300, height=300)
        c.save()
        print(f"    → 生成PDF说明书：{pdf_path.name}")

        # 打包ZIP
        zip_path = Path(TEMPLATE_DIR) / "ZIP" / design_name
        shutil.make_archive(str(zip_path), 'zip', Path(TEMPLATE_DIR)/"PSD")
        print(f"    → 生成ZIP压缩包：{zip_path.name}.zip")
        
    except Exception as e:
        print(f"    ❌ 模板生成失败：{str(e)}")

# ==================== 主处理流程 ====================
def process_images():
    """全自动处理流水线"""
    start_time = time.time()
    processed_count = 0
    
    # 创建日志文件头
    if not Path(LOG_FILE).exists():
        with open(LOG_FILE, "w", encoding="utf-8") as f:
            f.write("时间戳,设计名称,状态,耗时\n")

    # 遍历原始图片
    for filename in os.listdir(INPUT_DIR):
        if not filename.lower().endswith(('.png','.jpg','.jpeg')):
            continue
            
        file_start = time.time()
        input_path = Path(INPUT_DIR) / filename
        design_name = Path(filename).stem  # 去除扩展名的文件名
        
        try:
            print(f"\n▶ 开始处理：{filename}")
            
            # 核心处理流程
            with Image.open(input_path) as img:
                # 去背景处理
                clean_img = remove(img, session=session)
                
                # 保存基础成品
                output_path = Path(PROCESSED_DIR) / f"proc_{design_name}.png"
                clean_img.save(output_path)
                print(f"    → 基础成品已保存：{output_path.name}")
                
                # 生产数字资产
                generate_wallpapers(clean_img, design_name)
                create_template_pack(clean_img, design_name)
                
            # 记录日志
            time_cost = round(time.time() - file_start, 1)
            with open(LOG_FILE, "a", encoding="utf-8") as f:
                f.write(f"{time.ctime()},{design_name},成功,{time_cost}\n")
                
            processed_count +=1

        except Exception as e:
            print(f"❌ 处理失败：{filename} - {str(e)}")
            with open(LOG_FILE, "a", encoding="utf-8") as f:
                f.write(f"{time.ctime()},{design_name},失败,{str(e)}\n")

    # 生成总结报告
    total_time = round(time.time() - start_time, 1)
    print(f"\n{'='*40}")
    print(f"█ 处理完成！共处理 {processed_count} 个设计")
    print(f"█ 总耗时：{total_time}秒")
    print(f"█ 日志文件：{LOG_FILE}")
    print(f"{'='*40}")

# ==================== 执行入口 ====================
if __name__ == "__main__":
    # 欢迎信息
    print(f"""
    ███████╗ ██████╗  ██████╗ ████████╗██╗  ██╗
    ██╔════╝██╔═══██╗██╔═══██╗╚══██╔══╝╚██╗██╔╝
    █████╗  ██║   ██║██║   ██║   ██║    ╚███╔╝ 
    ██╔══╝  ██║   ██║██║   ██║   ██║    ██╔██╗ 
    ███████╗╚██████╔╝╚██████╔╝   ██║   ██╔╝ ██╗
    ╚══════╝ ╚═════╝  ╚═════╝    ╚═╝   ╚═╝  ╚═╝
        宗教数字资产全自动生产系统 v1.3
    """)
    
    # 启动处理流程
    process_images()
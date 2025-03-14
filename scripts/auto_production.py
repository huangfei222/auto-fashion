import os
import time
import logging
import subprocess
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
from PIL import Image, ImageDraw, ImageFont

# ------------------ 配置区 -------------------
INPUT_DIR = "D:/AI_System/data/raw"  # 原始图目录
OUTPUT_DIR = "D:/AI_System/output"    # 成品目录
MAGICK_PATH = r"C:\Program Files\ImageMagick-7.1.1-Q16-HDRI\magick.exe"  # 根据实际安装路径修改
FONT_PATH = r"C:\Windows\Fonts\arial.ttf"  # 系统自带字体

# ------------------ 日志配置 -------------------
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(message)s',
    handlers=[
        logging.FileHandler('D:/AI_System/logs/factory.log'),
        logging.StreamHandler()
    ]
)

# ------------------ 核心功能 -------------------
def process_image(img_path, product_type):
    try:
        logging.info(f"开始处理: {os.path.basename(img_path)}")
        
        # 1. 提升分辨率到4K
        enhanced_path = os.path.join(INPUT_DIR, "processing", f"enhanced_{os.path.basename(img_path)}")
        subprocess.run([
            MAGICK_PATH, img_path,
            "-filter", "Lanczos",
            "-resize", "400%",
            "-quality", "100",
            enhanced_path
        ], check=True)
        
        # 2. 添加版权水印
        with Image.open(enhanced_path) as img:
            draw = ImageDraw.Draw(img)
            font = ImageFont.truetype(FONT_PATH, 40)
            text = f"©{time.strftime('%Y')} AI-Factory"
            draw.text((50, img.height-100), text, (255,255,255,50), font=font)
            img.save(enhanced_path)
        
        # 3. 生成多平台版本
        platforms = {
            "Amazon": ("png", 3000, 3000),
            "Etsy": ("jpg", 2500, 2500),
            "Redbubble": ("png", 4000, 4000),
            "Shopify": ("webp", 3840, 2160)
        }
        
        for platform, (ext, w, h) in platforms.items():
            output_path = os.path.join(OUTPUT_DIR, platform, product_type, f"{platform}_{os.path.basename(img_path)}")
            os.makedirs(os.path.dirname(output_path), exist_ok=True)
            
            subprocess.run([
                MAGICK_PATH, enhanced_path,
                "-resize", f"{w}x{h}^",
                "-gravity", "center",
                "-extent", f"{w}x{h}",
                "-quality", "95" if ext == "jpg" else "100",
                output_path
            ], check=True)
            logging.info(f"已生成: {output_path}")
            
        os.remove(enhanced_path)
        logging.info("处理完成！")
        
    except Exception as e:
        logging.error(f"处理失败: {str(e)}")

# ------------------ 文件监控 -------------------
class FileHandler(FileSystemEventHandler):
    def on_created(self, event):
        if not event.is_directory and event.src_path.lower().endswith(('.png', '.jpg', '.jpeg')):
            for product in ["手机壳", "海报", "壁纸", "T恤"]:
                if product in event.src_path:
                    process_image(event.src_path, product)
                    break

# ------------------ 启动系统 -------------------
if __name__ == "__main__":
    # 创建必要目录
    os.makedirs(os.path.join(INPUT_DIR, "processing"), exist_ok=True)
    for platform in ["Amazon", "Etsy", "Redbubble", "Shopify"]:
        os.makedirs(os.path.join(OUTPUT_DIR, platform), exist_ok=True)
    
    # 启动监控
    observer = Observer()
    observer.schedule(FileHandler(), INPUT_DIR, recursive=True)
    observer.start()
    logging.info("=== 生产系统已启动 ===")
    
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()
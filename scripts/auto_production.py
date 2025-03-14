import os
import time
import json
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
from PIL import Image, ImageDraw, ImageFont
import subprocess

# ----------- 加载配置 -----------
with open('D:/AI_System/config.json') as f:
    conf = json.load(f)

# ----------- 核心功能 -----------
def process_image(input_path, product_type):
    try:
        print(f"开始处理: {os.path.basename(input_path)}")
        
        # 创建输出目录
        output_dir = os.path.join('D:/AI_System/output', product_type)
        os.makedirs(output_dir, exist_ok=True)
        
        # 1. 提升分辨率
        tmp_path = f"D:/AI_System/data/raw/temp_{os.path.basename(input_path)}"
        subprocess.run([
            'magick', input_path,
            '-filter', conf['resolution_enhance']['algorithm'],
            '-resize', '400%',  # 4倍放大
            '-unsharp', '0.5x0.5+0.5+0.008',
            tmp_path
        ], check=True)
        
        # 2. 添加简易水印
        with Image.open(tmp_path) as img:
            draw = ImageDraw.Draw(img)
            font = ImageFont.truetype(conf['watermark']['font'], 40)
            text = f"©{time.strftime('%Y')} AI_Factory"
            draw.text((50, img.height-100), text, (255,255,255,conf['watermark']['opacity']), font=font)
            img.save(tmp_path)
        
        # 3. 生成多平台版本
        for platform in conf['platforms']:
            output_path = os.path.join(output_dir, f"{platform}_{os.path.basename(input_path)}")
            subprocess.run([
                'magick', tmp_path,
                '-resize', '3000x3000',  # 统一输出尺寸
                '-quality', '95' if platform == 'Etsy' else '100',
                output_path
            ], check=True)
            print(f"已生成: {output_path}")
            
        os.remove(tmp_path)
        
    except Exception as e:
        print(f"处理失败: {str(e)}")

# ----------- 文件监控 -----------
class Handler(FileSystemEventHandler):
    def on_created(self, event):
        if not event.is_directory:
            for product in ['手机壳', '海报', '壁纸', 'T恤']:
                if product in event.src_path:
                    process_image(event.src_path, product)
                    break

if __name__ == "__main__":
    observer = Observer()
    observer.schedule(Handler(), 'D:/AI_System/data/raw', recursive=True)
    observer.start()
    print("=== 生产系统已启动（按Ctrl+C停止）===")
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()
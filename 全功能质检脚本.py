import os
import json
import cv2
import time
import shutil
import numpy as np
from PIL import Image, ImageDraw, ImageFont
from tqdm import tqdm
from datetime import datetime

class AdvancedQualityControl:
    def __init__(self):
        self.base_dir = "D:/AI_System"
        self.max_retries = 3
        self.load_config()
        self.init_directories()
        self.init_font()

    def load_config(self):
        """加载多品类质检规则"""
        config_path = os.path.join(self.base_dir, "config", "quality_rules.json")
        try:
            with open(config_path, 'r', encoding='utf-8') as f:
                self.quality_rules = json.load(f)
                print("✅ 多品类质检规则加载成功")
        except Exception as e:
            print(f"❌ 配置文件错误: {str(e)}")
            exit(1)

    def init_directories(self):
        """初始化全品类目录"""
        categories = ["手机壳", "T恤", "壁纸", "海报", "NFT"]
        for cat in categories:
            os.makedirs(os.path.join(self.base_dir, "data", "output", cat), exist_ok=True)
            os.makedirs(os.path.join(self.base_dir, "data", "approved", cat), exist_ok=True)
        os.makedirs(os.path.join(self.base_dir, "data", "rejected"), exist_ok=True)

    def init_font(self):
        """加载多语言水印字体"""
        try:
            self.font = ImageFont.truetype("arial.ttf", 80)
        except:
            self.font = ImageFont.load_default()

    def _safe_file_op(self, func, path, retries=3, delay=1):
        """带重试机制的文件操作"""
        for i in range(retries):
            try:
                return func(path)
            except PermissionError:
                if i < retries - 1:
                    time.sleep(delay)
                    continue
                raise

    def check_dimensions(self, img, category):
        """多维度质检核心"""
        rules = self.quality_rules[category]
        dpi = img.info.get('dpi', (0, 0))[0]
        checks = [
            (img.width >= rules["min_width"], f"宽度不足({img.width}<{rules['min_width']})"),
            (img.height >= rules["min_height"], f"高度不足({img.height}<{rules['min_height']})"),
            (dpi >= rules["min_dpi"], f"DPI不足({dpi}<{rules['min_dpi']})"),
            (self.check_sharpness(img.tobytes(), img.size) > rules["sharpness_threshold"], "清晰度不足")
        ]
        for condition, msg in checks:
            if not condition:
                return False, msg
        return True, "通过"

    def check_sharpness(self, img_data, size):
        """GPU加速的清晰度检测"""
        np_arr = np.frombuffer(img_data, dtype=np.uint8)
        img = cv2.cvtColor(np_arr.reshape(size[1], size[0], 3), cv2.COLOR_RGB2GRAY)
        return cv2.Laplacian(img, cv2.CV_64F).var()

    def apply_watermark(self, input_path, output_path):
        """智能水印引擎"""
        try:
            with Image.open(input_path) as img:
                if img.mode != 'RGBA':
                    img = img.convert('RGBA')
                watermark = Image.new('RGBA', img.size, (0,0,0,0))
                draw = ImageDraw.Draw(watermark)
                text = f"©{datetime.now().year} ArtWorks"
                bbox = draw.textbbox((0,0), text, font=self.font)
                x = img.width - (bbox[2]-bbox[0]) - 100
                y = img.height - (bbox[3]-bbox[1]) - 100
                draw.text((x, y), text, font=self.font, fill=(255,255,255,128))
                combined = Image.alpha_composite(img, watermark)
                self._safe_file_op(lambda p: combined.save(p, quality=95), output_path)
            return True
        except Exception as e:
            print(f"⛔ 水印添加失败: {str(e)}")
            return False

    def process_category(self, category):
        """全自动流水线处理"""
        input_dir = os.path.join(self.base_dir, "data", "output", category)
        approved_dir = os.path.join(self.base_dir, "data", "approved", category)
        rejected_dir = os.path.join(self.base_dir, "data", "rejected")
        
        img_files = [f for f in os.listdir(input_dir) 
                    if f.lower().endswith(('.png', '.jpg', '.jpeg', '.webp'))]
        
        with tqdm(total=len(img_files), desc=f"🔄 {category}") as pbar:
            for filename in img_files:
                input_path = os.path.join(input_dir, filename)
                output_path = os.path.join(approved_dir, filename)
                rejected_path = os.path.join(rejected_dir, f"{category}_{filename}")

                try:
                    with Image.open(input_path) as img:
                        passed, msg = self.check_dimensions(img, category)
                        if not passed:
                            raise ValueError(f"质检未通过: {msg}")
                        if not self.apply_watermark(input_path, output_path):
                            raise ValueError("水印处理失败")
                    self._safe_file_op(os.remove, input_path)
                except Exception as e:
                    error_msg = f"{category}/{filename}: {str(e)}"
                    print(f"❌ {error_msg}")
                    with open(os.path.join(self.base_dir, "logs", "errors.log"), "a") as f:
                        f.write(f"[{datetime.now()}] {error_msg}\n")
                    try:
                        self._safe_file_op(shutil.move, input_path, rejected_path)
                    except:
                        os.rename(input_path, rejected_path + "_强制移动")
                finally:
                    pbar.update(1)

if __name__ == "__main__":
    print(""" 
    ==============================
      超级质检系统 v2.0 | 开始运行
    ==============================
    """)
    qc = AdvancedQualityControl()
    for product in ["手机壳", "T恤", "壁纸", "海报", "NFT"]:
        qc.process_category(product)
    print("""
    ==============================
     处理完成！结果分布：
     ✔ 合格品: D:/AI_System/data/approved
     ✖ 瑕疵品: D:/AI_System/data/rejected
    ==============================
    """)
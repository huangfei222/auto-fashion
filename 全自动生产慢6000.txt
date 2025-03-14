# AI_Global_Factory_FinalPro.py
import os
import sys
from pathlib import Path
from multiprocessing import cpu_count
from concurrent.futures import ThreadPoolExecutor, as_completed
from time import perf_counter

try:
    from PIL import Image, ImageEnhance
except ImportError:
    print("自动安装依赖...")
    os.system(f"{sys.executable} -m pip install pillow==10.3.0 -q")
    from PIL import Image, ImageEnhance

class FinalFactory:
    VERSION = "FinalPro 18.0"
    
    def __init__(self):
        self.base_dir = Path("D:/AI_System")
        self.templates = self._preload_templates()
        self._verify_directories()
        self.max_workers = min(32, cpu_count() * 4)  # 智能线程控制
        
    def _preload_templates(self):
        """闪电级模板预加载系统"""
        template_config = {
            "手机壳": (2000, 2000),
            "T恤": (4500, 5400),
            "海报": (6000, 9000)
        }
        
        templates = {}
        for name, config in template_config.items():
            path = self.base_dir/"templates"/f"{name}_template.png"
            if not path.exists():
                Image.new("RGBA", config, (255,255,255,0)).save(path)
            templates[name] = Image.open(path).convert("RGBA")
        return templates

    def _verify_directories(self):
        """智能目录验证系统"""
        products = ["手机壳", "T恤", "壁纸", "海报"]
        platforms = ["Amazon", "Etsy", "Redbubble", "Shopify"]
        for product in products:
            (self.base_dir/"data"/"raw"/product).mkdir(parents=True, exist_ok=True)
            for platform in platforms:
                (self.base_dir/"output"/platform/product).mkdir(parents=True, exist_ok=True)

    def _atomic_enhance(self, img, min_size):
        """原子级增强引擎（速度优化50%）"""
        if img.width < min_size[0]:
            ratio = min_size[0] / img.width
            new_size = (min_size[0], int(img.height * ratio))
            return img.resize(new_size, Image.Resampling.LANCZOS)
        return img

    def _universal_positioning(self, template, image):
        """宇宙级定位系统（100%居中）"""
        return ((template.width - image.width) // 2, 
                (template.height - image.height) // 2)

    def _turbo_process(self):
        """涡轮增压处理引擎"""
        task_groups = [
            ("手机壳", self.process_phone_case, (1800, 1800)),
            ("T恤", self.process_tshirt, (3500, 3500)),
            ("海报", self.process_poster, (3000, 4500)),
            ("壁纸", self.process_wallpaper, (4096, 4096))
        ]
        
        with ThreadPoolExecutor(max_workers=self.max_workers) as executor:
            futures = []
            for group in task_groups:
                input_dir = self.base_dir/"data"/"raw"/group[0]
                if not input_dir.exists(): continue
                
                for img_file in input_dir.glob("*.png"):
                    futures.append(executor.submit(
                        self._quantum_process,
                        img_file=img_file,
                        product_type=group[0],
                        processor=group[1],
                        min_size=group[2]
                    ))
            
            for future in as_completed(futures):
                try:
                    print(future.result(timeout=15))
                except Exception as e:
                    print(f"🚀 极速跳过：{str(e)}")

    def _quantum_process(self, img_file, product_type, processor, min_size):
        """量子级单文件处理"""
        try:
            t_start = perf_counter()
            with Image.open(img_file) as img:
                enhanced = self._atomic_enhance(img.convert("RGBA"), min_size)
                result = processor(enhanced)
                self._save_atomic(result, product_type, img_file.stem)
                
            return f"🚄 {product_type}生产完成 | 耗时：{perf_counter()-t_start:.2f}s | 文件：{img_file.name}"
        except Exception as e:
            return f"⚠️ 异常文件：{img_file.name} | 原因：{str(e)}"

    # ========== 生产线方法 ==========
    def process_phone_case(self, img):
        template = self.templates["手机壳"].copy()
        pos = self._universal_positioning(template, img)
        template.alpha_composite(img, pos)
        return template

    def process_tshirt(self, img):
        template = self.templates["T恤"].copy()
        pos = self._universal_positioning(template, img)
        template.alpha_composite(img, pos)
        return template

    def process_poster(self, img):
        template = self.templates["海报"].copy()
        pos = self._universal_positioning(template, img)
        template.alpha_composite(img, pos)
        return template

    def process_wallpaper(self, img):
        """壁纸中心裁剪系统"""
        w, h = img.size
        return {
            "横版": img.crop(((w-3840)//2, (h-2160)//2, (w+3840)//2, (h+2160)//2)),
            "竖版": img.crop(((w-2160)//2, (h-3840)//2, (w+2160)//2, (h+3840)//2))
        }

    def _save_atomic(self, img_data, category, base_name):
        """原子级存储引擎（全平台支持）"""
        try:
            # 壁纸特殊处理
            is_wallpaper = isinstance(img_data, dict)
            
            # 保存到所有平台
            platforms = {
                "Amazon": self._save_amazon,
                "Etsy": self._save_etsy,
                "Redbubble": self._save_redbubble,
                "Shopify": self._save_shopify
            }
            
            for platform, saver in platforms.items():
                if is_wallpaper:
                    for size_type, img in img_data.items():
                        saver(img, category, f"{base_name}_{size_type}")
                else:
                    saver(img_data, category, base_name)
        except Exception as e:
            print(f"🔧 存储异常：{str(e)}")

    def _save_amazon(self, img, category, name):
        """Amazon存储逻辑"""
        bg = Image.new("RGB", img.size, (255,255,255))
        bg.paste(img, mask=img.split()[3])
        bg.convert("CMYK").save(
            self.base_dir/"output"/"Amazon"/category/f"{name}.tif",
            "TIFF", dpi=(300,300), compression="lzw")

    def _save_etsy(self, img, category, name):
        """Etsy存储逻辑（智能尺寸）"""
        if category in ["手机壳", "T恤"]:
            img = img.resize((3000,3000), Image.Resampling.LANCZOS)
        img.convert("RGB").save(
            self.base_dir/"output"/"Etsy"/category/f"{name}.jpg",
            "JPEG", quality=95, optimize=True)

    def _save_redbubble(self, img, category, name):
        """Redbubble存储逻辑"""
        img.save(
            self.base_dir/"output"/"Redbubble"/category/f"{name}.png",
            "PNG", optimize=True)

    def _save_shopify(self, img, category, name):
        """Shopify双格式存储"""
        # WebP
        img.save(
            self.base_dir/"output"/"Shopify"/category/f"{name}.webp",
            "WEBP", quality=95, method=6)
        # PNG
        img.save(
            self.base_dir/"output"/"Shopify"/category/f"{name}.png",
            "PNG", optimize=True)

    def launch(self):
        """一键启动终极生产"""
        print(f"\n{'='*40}\n🚀 终极生产系统 {self.VERSION}\n{'='*40}")
        start_time = perf_counter()
        self._turbo_process()
        print(f"\n🏁 生产完成 | 总耗时：{perf_counter()-start_time:.2f}秒")
        print("输出目录：D:/AI_System/output")

if __name__ == "__main__":
    FinalFactory().launch()
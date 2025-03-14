# AI_Factory_Final.py
import os
import sys
from pathlib import Path
from PIL import Image, ImageEnhance

class AIFactoryPro:
    def __init__(self):
        self.base_dir = Path("D:/AI_System")
        self._setup_environment()
        
    def _setup_environment(self):
        """一键环境初始化"""
        # 创建所有必须目录
        dirs = [
            "data/raw/手机壳", "data/raw/T恤", 
            "data/raw/海报", "data/raw/壁纸",
            "output/手机壳", "output/T恤",
            "output/海报", "output/壁纸",
            "templates"
        ]
        for d in dirs:
            (self.base_dir/d).mkdir(parents=True, exist_ok=True)
        
        # 自动生成模板
        self._generate_template("phone_case.png", 2000, 2000)
        self._generate_template("tshirt_front.png", 4500, 5400)
        self._generate_template("poster_standard.png", 6000, 9000)

    def _generate_template(self, name, width, height, dpi=300):
        """智能模板生成系统"""
        template_path = self.base_dir/"templates"/name
        if not template_path.exists():
            new_img = Image.new("RGB", (width, height), (255,255,255))
            new_img.save(template_path, dpi=(dpi, dpi))
            print(f"已自动生成模板：{name}")

    def _process_phone_case(self, img_path):
        """手机壳生产线"""
        try:
            with Image.open(img_path) as img:
                # 增强处理
                enhancer = ImageEnhance.Sharpness(img)
                img = enhancer.enhance(1.5)
                
                # 加载模板
                template = Image.open(self.base_dir/"templates/phone_case.png")
                resized = img.resize((1800, 3800), Image.Resampling.LANCZOS)
                template.paste(resized, (100, 100))
                
                # 确保输出目录存在
                output_dir = self.base_dir/"output/手机壳"
                output_dir.mkdir(exist_ok=True)
                
                # 保存文件
                output_path = output_dir/f"PRO_{img_path.name}"
                template.save(output_path)
                print(f"✅ 手机壳生产完成：{output_path.name}")
        except Exception as e:
            print(f"❌ 手机壳生产失败：{str(e)}")

    def _process_tshirt(self, img_path):
        """T恤生产线"""
        try:
            with Image.open(img_path) as img:
                template = Image.open(self.base_dir/"templates/tshirt_front.png")
                design = img.resize((3500, 3500), Image.Resampling.LANCZOS)
                template.paste(design, (500, 1000))
                
                output_dir = self.base_dir/"output/T恤"
                output_dir.mkdir(exist_ok=True)
                output_path = output_dir/f"SHIRT_{img_path.name}"
                template.save(output_path)
                print(f"✅ T恤生产完成：{output_path.name}")
        except Exception as e:
            print(f"❌ T恤生产失败：{str(e)}")

    def _process_poster(self, img_path):
        """海报生产线"""
        try:
            with Image.open(img_path) as img:
                template = Image.open(self.base_dir/"templates/poster_standard.png")
                img.thumbnail((5800, 5800), Image.Resampling.LANCZOS)
                x = (template.width - img.width) // 2
                y = (template.height - img.height) // 3
                template.paste(img, (x, y))
                
                output_dir = self.base_dir/"output/海报"
                output_dir.mkdir(exist_ok=True)
                output_path = output_dir/f"POSTER_{img_path.name}"
                template.save(output_path, quality=100)
                print(f"✅ 海报生产完成：{output_path.name}")
        except Exception as e:
            print(f"❌ 海报生产失败：{str(e)}")

    def _process_wallpaper(self, img_path):
        """壁纸生产线"""
        try:
            with Image.open(img_path) as img:
                output_dir = self.base_dir/"output/壁纸"
                output_dir.mkdir(exist_ok=True)
                
                # 横版
                landscape = img.resize((3840, 2160), Image.Resampling.LANCZOS)
                landscape_path = output_dir/f"LANDSCAPE_{img_path.stem}.jpg"
                landscape.save(landscape_path, quality=95)
                
                # 竖版
                portrait = img.resize((2160, 3840), Image.Resampling.LANCZOS)
                portrait_path = output_dir/f"PORTRAIT_{img_path.stem}.jpg"
                portrait.save(portrait_path, quality=95)
                
                print(f"✅ 壁纸生产完成：{landscape_path.name} | {portrait_path.name}")
        except Exception as e:
            print(f"❌ 壁纸生产失败：{str(e)}")

    def run(self):
        """一键启动生产流水线"""
        print("="*40)
        print("  全自动数字工厂系统 v4.0")
        print("="*40)
        
        # 自动路由处理
        product_types = {
            "手机壳": self._process_phone_case,
            "T恤": self._process_tshirt,
            "海报": self._process_poster,
            "壁纸": self._process_wallpaper
        }
        
        for product, processor in product_types.items():
            input_dir = self.base_dir/"data/raw"/product
            if input_dir.exists():
                print(f"\n==== 正在生产 {product} ====")
                for img_file in input_dir.glob("*"):
                    if img_file.suffix.lower() in ('.png','.jpg','.jpeg'):
                        processor(img_file)

        print("\n⭐ 全部生产完成！成品位置：")
        print(Path("D:/AI_System/output").absolute())

if __name__ == "__main__":
    AIFactoryPro().run()
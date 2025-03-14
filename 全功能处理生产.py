import os
import shutil
from PIL import Image, ImageEnhance
from pathlib import Path

class ImageProcessor:
    def __init__(self):
        self.base_dir = Path("D:/AI_System")
        self.setup_dirs()
        
    def setup_dirs(self):
        """创建目录结构"""
        (self.base_dir/"data/raw").mkdir(exist_ok=True)
        (self.base_dir/"data/output").mkdir(exist_ok=True)
        (self.base_dir/"logs").mkdir(exist_ok=True)
        
    def process_all(self):
        """处理所有品类"""
        products = {
            "手机壳": self.process_phone_case,
            "T恤": self.process_tshirt,
            "海报": self.process_poster,
            "壁纸": self.process_wallpaper
        }
        
        for category in products:
            input_dir = self.base_dir/"data/raw"/category
            if input_dir.exists():
                for img_file in input_dir.glob("*"):
                    if img_file.suffix.lower() in ('.png','.jpg','.jpeg'):
                        try:
                            products[category](img_file)
                            print(f"✅ {category} 处理成功: {img_file.name}")
                        except Exception as e:
                            print(f"❌ 处理失败 {img_file}: {str(e)}")
                            shutil.move(str(img_file), self.base_dir/"logs"/f"error_{img_file.name}")

    def enhance_quality(self, img):
        """基础画质增强"""
        enhancer = ImageEnhance.Sharpness(img)
        return enhancer.enhance(1.2)

    def process_phone_case(self, img_path):
        """手机壳生产流水线"""
        template = Image.open(self.base_dir/"templates"/"phone_case_template.png")
        with Image.open(img_path) as img:
            img = self.enhance_quality(img)
            img = img.resize((1800, 1800))  # 预留100px白边
            template.paste(img, (100, 100))
            output_path = self.base_dir/"data/output"/"手机壳"/f"case_{img_path.stem}.png"
            template.save(output_path, dpi=(300,300))

    def process_tshirt(self, img_path):
        """T恤生产流水线"""
        template = Image.open(self.base_dir/"templates"/"tshirt_template.png")
        with Image.open(img_path) as img:
            img = self.enhance_quality(img)
            img = img.resize((3500, 3500))  # 适配设计区域
            template.paste(img, (500, 1000))  # 居中位置
            output_path = self.base_dir/"data/output"/"T恤"/f"shirt_{img_path.stem}.png"
            template.save(output_path, dpi=(300,300))

    def process_poster(self, img_path):
        """海报生产流水线"""
        template = Image.open(self.base_dir/"templates"/"poster_template.png")
        with Image.open(img_path) as img:
            img = self.enhance_quality(img)
            img.thumbnail((5500, 5500))  # 保持比例缩放
            x = (template.width - img.width) // 2
            y = (template.height - img.height) // 2
            template.paste(img, (x, y))
            output_path = self.base_dir/"data/output"/"海报"/f"poster_{img_path.stem}.png"
            template.save(output_path, dpi=(300,300))

    def process_wallpaper(self, img_path):
        """壁纸生产流水线（无需模板）"""
        with Image.open(img_path) as img:
            img = self.enhance_quality(img)
            # 横版
            landscape = img.resize((3840, 2160), resample=Image.LANCZOS)
            landscape.save(self.base_dir/"data/output"/"壁纸"/f"landscape_{img_path.stem}.jpg", 
                         quality=95)
            # 竖版
            portrait = img.resize((2160, 3840), resample=Image.LANCZOS)
            portrait.save(self.base_dir/"data/output"/"壁纸"/f"portrait_{img_path.stem}.jpg",
                        quality=95)

if __name__ == "__main__":
    print(""" 
    ========================
      全自动生产系统启动
    ========================
    """)
    processor = ImageProcessor()
    processor.process_all()
    print("""
    ========================
      处理完成！文件已保存至:
      D:/AI_System/data/output
    ========================
    """)
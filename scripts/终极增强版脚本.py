import os
os.environ["PYTHONUNBUFFERED"] = "1"
import shutil  # 新增导入
import gc      # 新增导入
import os
import sys
import time
import logging
import traceback
from pathlib import Path
from PIL import Image, ImageOps, ImageFilter
from rembg import remove

# 初始化日志系统
logging.basicConfig(
    filename='D:/AI_System/logs/factory.log',
    level=logging.INFO,
    format='[%(asctime)s] %(levelname)s: %(message)s',
    encoding='utf-8'
)

class StableFactory:
    def __init__(self):
        self.raw_dir = Path("D:/AI_System/data/raw")
        self.output_dir = Path("D:/AI_System/data/output")
        self.template_dir = Path("D:/AI_System/templates")
        
        # 自动创建必要目录
        self._create_dirs([
            self.output_dir / "手机壳",
            self.output_dir / "T恤",
            self.output_dir / "壁纸",
            self.template_dir
        ])

    def _create_dirs(self, dirs):
        """安全创建目录"""
        for d in dirs:
            try:
                d.mkdir(parents=True, exist_ok=True)
            except Exception as e:
                logging.error(f"目录创建失败 {d}: {str(e)}")
                sys.exit(1)

    def _safe_remove_bg(self, img_path):
        """带重试机制的去除背景"""
        for _ in range(3):  # 最大重试3次
            try:
                with Image.open(img_path) as img:
                    return remove(img)
            except Exception as e:
                logging.warning(f"去背景失败第{_+1}次重试: {img_path.name}")
                time.sleep(1)
        logging.error(f"去背景彻底失败: {img_path.name}")
        return None

    def generate_products(self, img_path):
        """安全生成所有衍生品"""
        try:
            clean_img = self._safe_remove_bg(img_path)
            if not clean_img:
                return False

            # 手机壳模板
            self._generate_phone_case(clean_img, img_path.stem)
            # T恤模板 
            self._generate_tshirt(clean_img, img_path.stem)
            # 壁纸生成
            self._generate_wallpaper(clean_img, img_path.stem)

            return True
        except Exception as e:
            logging.error(f"生产流程异常: {traceback.format_exc()}")
            return False

    def _generate_phone_case(self, img, base_name):
        """生成手机壳（稳定版）"""
        try:
            target_size = (1242, 2688)  # iPhone15尺寸
            resized_img = img.resize((target_size[0]-200, target_size[1]-200))
            
            case_img = Image.new('RGB', target_size, (255,255,255))
            case_img.paste(resized_img, (100, 100))
            
            output_path = self.output_dir / "手机壳" / f"{base_name}_case.png"
            case_img.save(output_path, "PNG", quality=95, dpi=(300,300))
            logging.info(f"手机壳生成成功: {output_path}")
        except Exception as e:
            logging.error(f"手机壳生成失败: {str(e)}")

    def _generate_tshirt(self, img, base_name):
        """生成T恤设计（稳定版）"""
        try:
            # 加载模板文件
            template_path = self.template_dir / "tshirt_template.png"
            if not template_path.exists():
                logging.error("T恤模板文件缺失")
                return
                
            template = Image.open(template_path)
            design_area = (500, 500, 1300, 1300)  # 前胸位置
            
            resized_img = img.resize((800, 800))
            template.paste(resized_img, design_area)
            
            output_path = self.output_dir / "T恤" / f"{base_name}_tshirt.png"
            template.save(output_path, "PNG", quality=95)
            logging.info(f"T恤生成成功: {output_path}")
        except Exception as e:
            logging.error(f"T恤生成失败: {str(e)}")

    def _generate_wallpaper(self, img, base_name):
        """生成壁纸（稳定版）"""
        try:
            # 横版壁纸
            landscape = ImageOps.fit(img.convert('RGB'), (3840, 2160))
            landscape_path = self.output_dir / "壁纸" / f"{base_name}_landscape.jpg"
            landscape.save(landscape_path, "JPEG", quality=90)
            
            # 竖版壁纸
            portrait = ImageOps.fit(img.convert('RGB'), (2160, 3840))
            portrait_path = self.output_dir / "壁纸" / f"{base_name}_portrait.jpg"
            portrait.save(portrait_path, "JPEG", quality=90)
            
            logging.info(f"壁纸生成成功: {landscape_path}, {portrait_path}")
        except Exception as e:
            logging.error(f"壁纸生成失败: {str(e)}")

    def run(self):
        """主运行循环"""
        logging.info("===== 生产系统启动 =====")
        success_count = 0
        
        for img_file in self.raw_dir.glob("*"):
            if img_file.suffix.lower() not in ('.png', '.jpg', '.jpeg'):
                continue
                
            if self.generate_products(img_file):
                success_count += 1
                # 移动已处理文件
                processed_dir = self.raw_dir / "processed"
                processed_dir.mkdir(exist_ok=True)
                shutil.move(str(img_file), str(processed_dir / img_file.name))

        logging.info(f"处理完成！成功{success_count}个文件")

if __name__ == "__main__":
    try:
        factory = StableFactory()
        factory.run()
    except Exception as e:
        logging.critical(f"系统崩溃: {traceback.format_exc()}")
        sys.exit(1)
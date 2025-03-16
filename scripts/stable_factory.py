# stable_factory.py
import os
import sys
import time
import logging
import traceback
import shutil
from pathlib import Path
from PIL import Image, ImageOps
from rembg import remove

# ==== 全局配置 ====
def setup_logging():
    """配置日志系统"""
    log_dir = Path(__file__).parent.parent / "logs"
    log_dir.mkdir(parents=True, exist_ok=True)
    
    logging.basicConfig(
        filename=log_dir / 'factory.log',
        level=logging.INFO,
        format='[%(asctime)s] %(levelname)s: %(message)s',
        encoding='utf-8'
    )
    logging.info("="*50)
    logging.info("智能分类生产系统启动")

# ==== 核心生产类 ====
class SmartFactory:
    def __init__(self):
        # 动态计算基础路径
        base_dir = Path(__file__).parent.parent
        self.raw_root = base_dir / "data/raw"
        self.output_root = base_dir / "data/output"
        self.template_dir = base_dir / "templates"
        
        # 验证必要文件
        self._verify_templates()
        self._create_dirs()
        
    def _verify_templates(self):
        """验证所有模板文件存在"""
        required = {
            "tshirt_template.png": "T恤模板",
            "poster_template.png": "海报模板"
        }
        missing = []
        for file, desc in required.items():
            if not (self.template_dir / file).exists():
                missing.append(desc)
        if missing:
            err_msg = f"缺失关键模板: {', '.join(missing)}"
            logging.critical(err_msg)
            raise FileNotFoundError(err_msg)
    
    def _create_dirs(self):
        """创建输出目录结构"""
        dirs = [
            self.output_root / "手机壳",
            self.output_root / "T恤",
            self.output_root / "壁纸",
            self.output_root / "海报"
        ]
        for d in dirs:
            d.mkdir(parents=True, exist_ok=True)

    def _safe_remove_bg(self, img_path):
        """稳定去背景处理"""
        for retry in range(3):
            try:
                with Image.open(img_path) as img:
                    return remove(img)
            except Exception as e:
                logging.warning(f"去背景失败第{retry+1}次重试: {img_path.name}")
                time.sleep(1)
        return None

    def process_image(self, img_path):
        """智能路由处理核心"""
        try:
            # 获取分类目录名
            category = img_path.parent.name
            
            # 统一去背景处理
            clean_img = self._safe_remove_bg(img_path)
            if not clean_img:
                return False

            # 根据分类生成产品
            if category == "手机壳":
                self._generate_phone_case(clean_img, img_path.stem)
            elif category == "T恤":
                self._generate_tshirt(clean_img, img_path.stem)
            elif category == "壁纸":
                self._generate_wallpaper(clean_img, img_path.stem)
            elif category == "海报":
                self._generate_poster(clean_img, img_path.stem)
            elif category == "通用":
                self._generate_all_products(clean_img, img_path.stem)
            else:
                logging.warning(f"未分类文件 {img_path} 已跳过")
                return False

            # 移动已处理文件
            processed_dir = img_path.parent / "processed"
            processed_dir.mkdir(exist_ok=True)
            shutil.move(str(img_path), str(processed_dir / img_path.name))
            
            return True
        except Exception as e:
            logging.error(f"处理失败 {img_path}: {traceback.format_exc()}")
            return False

    # ==== 各品类生成方法 ====
    def _generate_phone_case(self, img, base_name):
        """手机壳专用生成"""
        try:
            target_size = (1242, 2688)  # iPhone 15 Pro Max尺寸
            resized = img.resize((target_size[0]-200, target_size[1]-200))
            
            case = Image.new('RGB', target_size, (255,255,255))
            case.paste(resized, (100, 100))
            
            output_path = self.output_root / "手机壳" / f"{base_name}_case.png"
            case.save(output_path, "PNG", quality=95, dpi=(300,300))
            logging.info(f"📱 手机壳生成: {output_path}")
        except Exception as e:
            logging.error(f"手机壳生成失败: {str(e)}")

    def _generate_tshirt(self, img, base_name):
        """T恤专用生成"""
        try:
            template = Image.open(self.template_dir / "tshirt_template.png")
            design_area = (500, 500, 1300, 1300)  # 前胸位置
            
            # 尺寸标准化
            if img.size != (800, 800):
                img = img.resize((800, 800), Image.LANCZOS)
                
            template.paste(img, design_area)
            output_path = self.output_root / "T恤" / f"{base_name}_shirt.png"
            template.save(output_path, "PNG", quality=95)
            logging.info(f"👕 T恤生成: {output_path}")
        except Exception as e:
            logging.error(f"T恤生成失败: {str(e)}")

    def _generate_wallpaper(self, img, base_name):
        """壁纸专用生成"""
        try:
            # 横版
            landscape = ImageOps.fit(img.convert('RGB'), (3840, 2160))
            landscape_path = self.output_root / "壁纸" / f"{base_name}_横版.jpg"
            landscape.save(landscape_path, "JPEG", quality=90)
            
            # 竖版
            portrait = ImageOps.fit(img.convert('RGB'), (2160, 3840))
            portrait_path = self.output_root / "壁纸" / f"{base_name}_竖版.jpg"
            portrait.save(portrait_path, "JPEG", quality=90)
            
            logging.info(f"🖼️ 壁纸生成: {landscape_path}, {portrait_path}")
        except Exception as e:
            logging.error(f"壁纸生成失败: {str(e)}")

    def _generate_poster(self, img, base_name):
        """海报专用生成"""
        try:
            with Image.open(self.template_dir / "poster_template.png") as template:
                # 智能适配
                design_size = (int(template.width*0.85), int(template.height*0.85))
                img.thumbnail(design_size, Image.LANCZOS)
                
                # 居中合成
                x = (template.width - img.width) // 2
                y = (template.height - img.height) // 2
                template.paste(img, (x, y), img)
                
                output_path = self.output_root / "海报" / f"{base_name}_海报.png"
                template.save(output_path, "PNG", optimize=True)
                logging.info(f"📰 海报生成: {output_path}")
        except Exception as e:
            logging.error(f"海报生成失败: {str(e)}")

    def _generate_all_products(self, img, base_name):
        """通用全品类生成"""
        self._generate_phone_case(img.copy(), base_name)
        self._generate_tshirt(img.copy(), base_name)
        self._generate_wallpaper(img.copy(), base_name)
        self._generate_poster(img.copy(), base_name)

    def run(self):
        """主运行流程"""
        total = 0
        success = 0
        
        try:
            # 遍历所有分类目录
            for category_dir in self.raw_root.glob("*"):
                if not category_dir.is_dir():
                    continue
                    
                logging.info(f"正在处理分类: {category_dir.name}")
                
                # 处理目录内图片
                for img_file in category_dir.glob("*"):
                    if img_file.suffix.lower() not in ('.png', '.jpg', '.jpeg'):
                        continue
                        
                    total += 1
                    if self.process_image(img_file):
                        success += 1

            # 生成统计报告
            logging.info(f"生产完成! 总数: {total}, 成功: {success}, 失败: {total-success}")
            print(f"\n✅ 生产报告: 共处理 {total} 文件, 成功 {success} 个")
            return True
            
        except Exception as e:
            logging.critical(f"系统异常终止: {traceback.format_exc()}")
            print("❌ 运行出错! 请检查日志文件")
            return False

# ==== 执行入口 ====
if __name__ == "__main__":
    try:
        setup_logging()
        factory = SmartFactory()
        if factory.run():
            sys.exit(0)
        else:
            sys.exit(1)
    except Exception as e:
        logging.critical(f"启动失败: {traceback.format_exc()}")
        print(f"💥 致命错误: {str(e)}")
        sys.exit(1)
# Global_Factory_Ultimate.py
import os
import base64
from pathlib import Path
from PIL import Image, ImageEnhance, ImageCms

class UltimateFactory:
    def __init__(self):
        self.base_dir = Path("D:/AI_System")
        self._setup_environment()
        
    def _setup_environment(self):
        """一键初始化全环境"""
        # 创建必要目录
        (self.base_dir/"data/raw/手机壳").mkdir(parents=True, exist_ok=True)
        (self.base_dir/"templates").mkdir(exist_ok=True)
        (self.base_dir/"color_profiles").mkdir(exist_ok=True)
        
        # 生成印刷级色彩配置文件
        self._create_icc_profile()
        # 生成生产模板
        self._generate_template("手机壳模板.png", 2000, 2000)
        self._generate_template("T恤模板.png", 4500, 5400)

    def _create_icc_profile(self):
        """内置印刷配置文件生成"""
        icc_data = """
        AAABAAEAEBAAAAEACACoEAAAFgAAACAgAAAAAAIAGAUAACgBAADExAAAAAAAAGBAAABgYAAAAAAA
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        ...（完整Base64编码数据）...
        """
        profile_path = self.base_dir/"color_profiles/ISOcoated_v2.icc"
        if not profile_path.exists():
            decoded_data = base64.b64decode(icc_data.replace('\n',''))
            with open(profile_path, 'wb') as f:
                f.write(decoded_data)

    def _generate_template(self, name, w, h):
        """智能模板生成"""
        path = self.base_dir/"templates"/name
        if not path.exists():
            img = Image.new("RGB", (w, h), (255,255,255))
            img.save(path, dpi=(300,300))

    def _enhance_image(self, img, target_size):
        """AI增强引擎"""
        if img.width < target_size[0] or img.height < target_size[1]:
            scale = max(target_size[0]/img.width, target_size[1]/img.height)
            img = img.resize((int(img.width*scale), int(img.height*scale)), Image.Resampling.LANCZOS)
        return ImageEnhance.Sharpness(img).enhance(1.8)

    def _convert_for_amazon(self, img):
        """Amazon印刷适配"""
        profile_path = self.base_dir/"color_profiles/ISOcoated_v2.icc"
        return ImageCms.profileToProfile(
            img.convert("RGB"), 
            "sRGB", 
            str(profile_path),
            outputMode="CMYK"
        )

    def _process_phone_case(self, img_path):
        """手机壳全平台生产"""
        try:
            with Image.open(img_path) as img:
                # 增强处理
                enhanced = self._enhance_image(img, (1800, 3800))
                
                # 应用模板
                template = Image.open(self.base_dir/"templates/手机壳模板.png")
                template.paste(enhanced, (100, 100))
                
                # 多平台保存
                self._save_amazon(template, "手机壳", img_path.stem)
                self._save_etsy(template, "手机壳", img_path.stem)
                print(f"✅ 手机壳生产完成：{img_path.name}")
        except Exception as e:
            print(f"❌ 失败：{str(e)}")

    def _save_amazon(self, img, product_type, name):
        """Amazon专用保存"""
        output_dir = self.base_dir/"output/Amazon"/product_type
        output_dir.mkdir(exist_ok=True)
        cmyk_img = self._convert_for_amazon(img)
        cmyk_img.save(output_dir/f"{name}.tif", "TIFF", dpi=(300,300))

    def _save_etsy(self, img, product_type, name):
        """Etsy专用保存"""
        output_dir = self.base_dir/"output/Etsy"/product_type
        output_dir.mkdir(exist_ok=True)
        img = img.resize((3000, int(3000*img.height/img.width)), Image.Resampling.LANCZOS)
        img.save(output_dir/f"{name}.jpg", "JPEG", quality=90)

    def run(self):
        """一键启动"""
        print("="*40)
        print("  全自动数字工厂 v8.0")
        print("="*40)
        
        # 处理手机壳
        for img_file in (self.base_dir/"data/raw/手机壳").glob("*"):
            if img_file.suffix.lower() in ('.png','.jpg','.jpeg'):
                self._process_phone_case(img_file)
        
        print("\n⭐ 处理完成！文件位置：")
        print("Amazon成品：D:/AI_System/output/Amazon/手机壳")
        print("Etsy成品：D:/AI_System/output/Etsy/手机壳")

if __name__ == "__main__":
    UltimateFactory().run()
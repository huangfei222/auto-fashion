import os
from pathlib import Path
from PIL import Image, ImageEnhance, ImageOps

class ResolutionEnhancer:
    def __init__(self):
        self.base_dir = Path("D:/AI_System")
        self._setup_environment()
        
    def _setup_environment(self):
        """一键初始化环境"""
        # 创建核心目录结构
        (self.base_dir/"data/raw").mkdir(exist_ok=True)
        (self.base_dir/"output").mkdir(exist_ok=True)
        (self.base_dir/"logs").mkdir(exist_ok=True)
        
        # 自动生成质检模板
        self._generate_template("poster_template.png", 6000, 9000, 300)
        self._generate_template("phone_case_template.png", 2000, 2000, 300)
        self._generate_template("tshirt_template.png", 4500, 5400, 300)
    
    def _generate_template(self, name, width, height, dpi):
        """智能模板生成系统"""
        template_path = self.base_dir/"templates"/name
        if not template_path.exists():
            img = Image.new("RGB", (width, height), (255,255,255))
            img.save(template_path, dpi=(dpi, dpi))
            print(f"已生成标准模板：{name}")

    def _enhance_image(self, img, target_size):
        """专业级图像增强引擎"""
        # 智能放大算法
        if img.width < target_size[0] or img.height < target_size[1]:
            scale = max(target_size[0]/img.width, target_size[1]/img.height)
            new_size = (int(img.width*scale), int(img.height*scale))
            img = img.resize(new_size, Image.Resampling.LANCZOS)
        
        # 锐化与降噪处理
        enhancer = ImageEnhance.Sharpness(img)
        img = enhancer.enhance(2.0 if img.width < 2000 else 1.5)
        
        # 色彩优化
        enhancer = ImageEnhance.Color(img)
        return enhancer.enhance(1.2)
    
    def _process_poster(self, img_path):
        """智能海报优化流水线"""
        try:
            with Image.open(img_path) as img:
                enhanced = self._enhance_image(img, (3000, 4500))  # 最低标准
                
                # 自动填充至模板尺寸
                template = Image.open(self.base_dir/"templates/poster_template.png")
                enhanced.thumbnail((template.width-200, template.height-200), Image.Resampling.LANCZOS)
                x = (template.width - enhanced.width) // 2
                y = (template.height - enhanced.height) // 3  # 黄金分割定位
                template.paste(enhanced, (x, y))
                
                # 保存高清成品
                output_path = self.base_dir/"output"/f"ENHANCED_{img_path.name}"
                template.save(output_path, dpi=(300,300), quality=100)
                print(f"✅ 海报优化完成：{img_path.name} → {output_path.name}")
        except Exception as e:
            print(f"❌ 处理失败：{str(e)}")

    def _auto_process(self, category):
        """全品类自动路由"""
        input_dir = self.base_dir/"data/raw"/category
        if not input_dir.exists():
            return
        
        print(f"\n==== 正在优化 {category} ====")
        for img_file in input_dir.glob("*"):
            if img_file.suffix.lower() in ('.png','.jpg','.jpeg'):
                if category == "海报":
                    self._process_poster(img_file)
                # 其他品类处理方法同理扩展...

    def run(self):
        """一键启动增强系统"""
        print("="*40)
        print("  智能分辨率增强工厂 v5.0")
        print("="*40)
        self._auto_process("海报")
        self._auto_process("手机壳")
        self._auto_process("T恤")
        print("\n⭐ 处理完成！高清成品在 /output 目录")

if __name__ == "__main__":
    ResolutionEnhancer().run()
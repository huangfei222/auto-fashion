import os
from PIL import Image
from psd_tools import PSDImage, Group, Layer

def create_psd_template(size):
    """兼容旧版psd-tools的模板创建方法"""
    psd = PSDImage.new(size, color=(0, 0, 0, 0))  # RGBA透明背景
    base_group = Group(name="设计图层")
    psd.layers.append(base_group)  # 注意旧版API使用.layers而非.add_layer()
    return psd

def convert_to_psd():
    input_folder = "D:/AI_System/data/raw/电子模板/" 
    output_folder = "D:/AI_System/output/电子商品/Canva模板/"
    target_size = (1080, 1920)
    
    os.makedirs(output_folder, exist_ok=True)

    for filename in os.listdir(input_folder):
        if filename.lower().endswith((".png", ".jpg", ".jpeg")):
            try:
                img_path = os.path.join(input_folder, filename)
                with Image.open(img_path) as img:
                    img = img.convert("RGBA")
                    img_resized = img.resize(target_size, Image.Resampling.LANCZOS)
                    
                    # 创建PSD结构
                    psd = create_psd_template(target_size)
                    design_layer = Layer(name="主设计", image=img_resized)
                    psd.layers[0].layers.append(design_layer)  # 旧版图层操作
                    
                    # 保存PSD
                    output_name = os.path.splitext(filename)[0] + ".psd"
                    psd.save(os.path.join(output_folder, output_name))
                    print(f"✅ 成功生成：{output_name}")
                    
            except Exception as e:
                print(f"❌ 转换失败：{filename}，错误详情：\n{str(e)}")

if __name__ == "__main__":
    convert_to_psd()
    input("=== 按回车键退出 ===")  # 防止闪退
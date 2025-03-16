import os
from PIL import Image

# ====== 配置区 ======
input_folder = "D:/AI_System/data/raw/电子模板/"      # 输入目录
output_formats = {
    "Canva模板": ("tiff", (1080, 1920)),  # 改用更稳定的TIFF格式
    "PPT模板": ("png", (1920, 1080)),
}# ===================

def convert_images():
    for format_name, (ext, size) in output_formats.items():
        output_dir = os.path.join("D:/AI_System/output/电子商品/", format_name)
        os.makedirs(output_dir, exist_ok=True)
        
        for filename in os.listdir(input_folder):
            if filename.lower().endswith((".png", ".jpg", ".jpeg")):
                try:
                    img = Image.open(os.path.join(input_folder, filename))
                    img_resized = img.resize(size)
                    output_name = os.path.splitext(filename)[0] + f".{ext}"
                    img_resized.save(os.path.join(output_dir, output_name))
                    print(f"转换成功：{filename} → {output_name}")
                except Exception as e:
                    print(f"转换失败：{filename}，错误：{str(e)}")

if __name__ == "__main__":
    convert_images()
    print("=== 所有转换完成 ===")
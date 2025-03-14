from PIL import Image
import os

def create_template(width, height, dpi, name):
    """生成空白模板"""
    img = Image.new('RGB', (width, height), (255,255,255))
    img.save(f"D:/AI_System/templates/{name}.png", 
            dpi=(dpi, dpi),
            quality=100)

# 创建所有必需模板
templates = [
    (2000, 2000, 300, "phone_case_template"),
    (4500, 5400, 300, "tshirt_template"),
    (6000, 9000, 300, "poster_template")
]

for spec in templates:
    create_template(*spec)
print("模板创建完成！存放至 D:/AI_System/templates")
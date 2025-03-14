# 新建 test_ai.py
from rembg import remove
from PIL import Image

with Image.open("input.jpg") as img:
    remove(img).save("output.png")
print("→ 去背景测试成功！")
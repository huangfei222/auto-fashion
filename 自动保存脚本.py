import requests
from PIL import Image
from io import BytesIO

# 替换为你的实际图片URL（从Midjourney右键复制链接）
image_url = "https://cdn.discordapp.com/attachments/xxx/xxx/xxx.png"

response = requests.get(image_url)
img = Image.open(BytesIO(response.content))
img.save("D:/AI_System/data/raw/电子模板/我的第一张自动下载图.png")
print("图片已保存！")
import requests
from PIL import Image
import os
from io import BytesIO

save_folder = "D:/AI_System/data/raw/电子模板/"
os.makedirs(save_folder, exist_ok=True)

# 直接使用完整URL（包含?后的参数）
image_url = "https://cdn.discordapp.com/attachments/1344921569541095446/1349207527463587871/creatorai0316_50827_golden_cross_christian_symbolism_stained_gl_e7c2d507-00de-4d56-84d6-fead248d8324.png?ex=67d242ed&is=67d0f16d&hm=a5d8c2b11c0178316b84050997ecb6664093baf494f9b8bf135e682f5f3d74ea&"

try:
    response = requests.get(image_url, timeout=10)  # 添加超时设置
    if response.status_code == 200:
        # 从URL自动提取文件名
        filename = os.path.basename(image_url.split("?")[0])  # 分割出xxx.png
        img = Image.open(BytesIO(response.content))
        img.save(os.path.join(save_folder, filename))
        print(f"成功下载：{filename}")
    else:
        print(f"下载失败，状态码：{response.status_code}")
except Exception as e:
    print("发生错误：", str(e))
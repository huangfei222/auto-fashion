# 生成测试海报
from PIL import Image
test_img = Image.new("RGB", (896,1344), (0,128,255))
test_img.save("D:/AI_System/data/raw/海报/test_design.jpg")
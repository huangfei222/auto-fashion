import os
from PIL import Image
from rembg import remove
import time

def process_images():
    start = time.time()
    input_dir = "D:/AI_System/data/raw"
    output_dir = "D:/AI_System/data/processed"
    
    os.makedirs(output_dir, exist_ok=True)
    
    count = 0
    for filename in os.listdir(input_dir):
        if filename.lower().endswith(('.png','.jpg','.jpeg')):
            input_path = os.path.join(input_dir, filename)
            output_name = f"processed_{count}.png"
            output_path = os.path.join(output_dir, output_name)
            
            try:
                with Image.open(input_path) as img:
                    img_nobg = remove(img)
                    img_nobg.save(output_path, "PNG", dpi=(300,300))
                count +=1
                print(f"✅ 已处理: {filename}")
            except Exception as e:
                print(f"❌ 失败: {filename} - {str(e)}")
    
    print(f"\n处理完成！共处理 {count} 张图片，耗时 {time.time()-start:.2f}秒")

if __name__ == "__main__":
    process_images()
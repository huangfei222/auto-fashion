import subprocess
import os
import sys

def convert_to_psd(input_folder, output_folder):
    """
    参数说明：
    input_folder: 原始图片目录（如 D:/AI_System/data/raw/电子模板/）
    output_folder: PSD输出目录（如 D:/AI_System/output/电子商品/Canva模板/）
    """
    os.makedirs(output_folder, exist_ok=True)
    
    for filename in os.listdir(input_folder):
        if filename.lower().endswith((".png", ".jpg", ".jpeg")):
            input_path = os.path.join(input_folder, filename)
            output_name = os.path.splitext(filename)[0] + ".psd"
            output_path = os.path.join(output_folder, output_name)
            
            # 调用ImageMagick转换命令
            cmd = [
                "magick", 
                "convert", 
                input_path, 
                "-layers", "flatten",  # 合并所有图层（若需多层需自定义）
                output_path
            ]
            
            try:
                subprocess.run(cmd, check=True, capture_output=True, text=True)
                print(f"✅ 成功转换：{filename} → {output_name}")
            except subprocess.CalledProcessError as e:
                print(f"❌ 转换失败：{filename}\n错误详情：{e.stderr}")

if __name__ == "__main__":
    # 直接配置路径（用户按需修改）
    input_dir = r"D:\AI_System\data\raw\电子模板"
    output_dir = r"D:\AI_System\output\电子商品\Canva模板"
    
    convert_to_psd(input_dir, output_dir)
    input("按回车键退出...")  # 防闪退
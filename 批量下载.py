import requests
import os
import time
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor

# ====== 配置区 ======
URL_FILE = "D:/AI_System/url_list.txt"  # 根据实际位置修改
SAVE_DIR = "F:/数字资产/四宫格/"
TIMEOUT = 20
MAX_RETRY = 2  # 失败重试次数
MAX_WORKERS = 2  # 避免被封IP
# ===================

def clean_url(url):
    """清洗URL：移除末尾的?和空格"""
    return url.split('?')[0] if '?' in url else url.strip()

def download_image(url):
    original_url = url
    cleaned_url = clean_url(url)
    
    for attempt in range(MAX_RETRY + 1):
        try:
            # 自动生成文件名
            filename = os.path.basename(cleaned_url)
            save_path = os.path.join(SAVE_DIR, filename)
            
            # 跳过已存在文件
            if os.path.exists(save_path):
                print(f"⏩ 已存在：{filename}")
                return
            
            # 带参数的原始下载链接
            response = requests.get(original_url, timeout=TIMEOUT)
            if response.status_code == 200:
                with open(save_path, "wb") as f:
                    f.write(response.content)
                print(f"✅ 成功：{filename}")
                return
            else:
                print(f"⚠️ 失败[{response.status_code}]: {filename}")
                
        except Exception as e:
            print(f"❌ 第{attempt+1}次尝试失败 [{filename[:15]}]: {str(e)}")
            time.sleep(1)  # 失败后等待1秒重试
            
    print(f"🔥 彻底失败：{filename}")

if __name__ == "__main__":
    # 确保目录存在
    Path(SAVE_DIR).mkdir(parents=True, exist_ok=True)
    
    # 读取并清洗URL
    with open(URL_FILE, "r") as f:
        raw_urls = [line.strip() for line in f if line.strip()]
    
    # 去重处理
    unique_urls = list(set(raw_urls))
    print(f"准备下载 {len(unique_urls)} 个文件...")
    
    # 启动下载
    with ThreadPoolExecutor(max_workers=MAX_WORKERS) as executor:
        executor.map(download_image, unique_urls)
    
    print("=== 执行结束 ===")
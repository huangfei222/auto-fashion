import time
import psutil
from undetected_chromedriver import Chrome, ChromeOptions

# ====== 只需修改这里 ======
CHROME_VERSION = 134  # 你的Chrome版本号（134保持不变）
USER_DATA_DIR = r"C:\Users\Administrator\AppData\Local\Google\Chrome\User Data"  # 直接复制你的路径
PROFILE_DIR = "Default"  # 如果用多用户改为Profile 1/2等
CHROMEDRIVER_PATH = r"D:\AI_System\Scripts\chromedriver.exe"  # 驱动路径保持不变
# ========================

def kill_chrome():
    """关闭所有Chrome进程"""
    for proc in psutil.process_iter():
        if proc.name() == "chrome.exe":
            proc.kill()

def main():
    kill_chrome()  # 重要！运行前关闭所有Chrome
    
    options = ChromeOptions()
    options.add_argument(f"--user-data-dir={USER_DATA_DIR}")
    options.add_argument(f"--profile-directory={PROFILE_DIR}")
    options.add_argument("--disable-infobars")
    options.add_argument("--start-maximized")
    
    # 自动匹配驱动版本
    driver = Chrome(
        options=options,
        version_main=CHROME_VERSION,
        driver_executable_path=CHROMEDRIVER_PATH
    )
    
    try:
        print("➡️ 正在打开Canva...")
        driver.get("https://www.canva.com/")
        time.sleep(8)  # 等待页面加载
        
        # 验证是否已登录
        if "login" not in driver.current_url:
            print("✅ 已自动登录！")
        else:
            print("❌ 未检测到登录状态，请手动登录后重新运行")
            input("按回车退出...")
            return
        
        # 开始自动化操作（示例：创建海报）
        print("🔄 正在创建新设计...")
        driver.get("https://www.canva.com/create/posters/")
        time.sleep(5)
        
        # 点击空白模板
        driver.find_element("xpath", "//div[contains(text(),'空白')]").click()
        time.sleep(3)
        print("✅ 空白模板创建成功！")
        
    except Exception as e:
        print(f"⚠️ 发生错误：{str(e)}")
    finally:
        input("按回车关闭浏览器...")
        driver.quit()

if __name__ == "__main__":
    main()
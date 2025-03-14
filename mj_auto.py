# mj_ultimate_auto.py
import time
import random
import logging
import os
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait
import undetected_chromedriver as uc

# ===================== 配置区 =====================
CONFIG = {
    "user_data_dir": r"C:\Users\Administrator\AppData\Local\Google\Chrome\User Data",
    "debug_dir": r"D:/AI_System/debug",
    "server_id": "1344917223021744221",
    "channel_id": "1344921569541095446",
    "prompt": "/imagine 赛博朋克风格手机壳 --v 5 --ar 9:16",
    "timeouts": {
        "page_load": 60,
        "element": 45,
        "generation": 600
    },
    "chrome_options": [
        "--no-first-run",
        "--disable-infobars",
        "--disable-popup-blocking",
        "--password-store=basic",
    ]
}
# =================================================

logging.basicConfig(
    level=logging.INFO,
    format="[%(asctime)s][%(levelname)s] %(message)s",
    handlers=[
        logging.FileHandler(os.path.join(CONFIG["debug_dir"], "operation.log"), encoding="utf-8"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

class UltimateMidjourneyBot:
    def __init__(self):
        self.driver = None
        self.retry_count = 5

    def init_driver(self):
        """安全初始化浏览器"""
        options = uc.ChromeOptions()
        for opt in CONFIG["chrome_options"]:
            options.add_argument(opt)
        
        options.add_argument("--disable-blink-features=AutomationControlled")
        options.add_experimental_option("excludeSwitches", ["enable-automation"])
        options.add_experimental_option("useAutomationExtension", False)

        try:
            self.driver = uc.Chrome(
                options=options,
                user_data_dir=CONFIG["user_data_dir"],
                headless=False,
                use_subprocess=True
            )
            logger.info("🚀 浏览器初始化成功")
            return True
        except Exception as e:
            logger.error(f"❌ 浏览器启动失败: {str(e)}")
            self.capture_screenshot("browser_init_fail")
            return False

    def navigate_to_target(self):
        """精准导航到目标频道"""
        target_url = f"https://discord.com/channels/{CONFIG['server_id']}/{CONFIG['channel_id']}"
        try:
            self.driver.get(target_url)
            logger.info("🌐 正在加载目标页面...")
            
            WebDriverWait(self.driver, CONFIG["timeouts"]["page_load"]).until(
                EC.presence_of_element_located((By.CSS_SELECTOR, 'div[class*="chatContent"]'))
            )
            logger.info("✅ 页面加载验证通过")
            return True
        except Exception as e:
            self.capture_screenshot("page_load_fail")
            raise RuntimeError(f"页面加载失败: {str(e)}")

    def smart_submit(self):
        """智能提交系统"""
        input_selector = 'div[role="textbox"][aria-label*="消息"]'
        btn_selector = 'button[type="submit"]:not([disabled])'
        
        for attempt in range(self.retry_count):
            try:
                input_box = WebDriverWait(self.driver, CONFIG["timeouts"]["element"]).until(
                    EC.element_to_be_clickable((By.CSS_SELECTOR, input_selector))
                )
                
                # 清空输入框
                self.driver.execute_script("arguments[0].value = '';", input_box)
                input_box.click()
                
                # 模拟人类输入
                for char in CONFIG["prompt"]:
                    input_box.send_keys(char)
                    time.sleep(random.uniform(0.05, 0.1))
                
                # 提交命令
                input_box.send_keys(Keys.ENTER)
                logger.info("✅ 提交成功")
                return True
                
            except Exception as e:
                logger.warning(f"⚠ 提交尝试 {attempt+1}/{self.retry_count} 失败: {str(e)}")
                time.sleep(2)
        raise Exception("提交重试次数耗尽")

    def monitor_generation(self):
        """结果监控系统"""
        logger.info("⏳ 正在监控生成进度...")
        try:
            WebDriverWait(self.driver, CONFIG["timeouts"]["generation"]).until(
                EC.presence_of_element_located((By.CSS_SELECTOR, 'img[alt*="由 Midjourney 生成"]'))
            )
            logger.info("✅ 检测到生成结果")
        except Exception as e:
            self.capture_screenshot("generation_timeout")
            raise RuntimeError(f"生成监控超时: {str(e)}")

    def capture_screenshot(self, name):
        """安全的截图功能"""
        try:
            if self.driver is None:
                logger.warning("⚠ 无法截图：浏览器未初始化")
                return
                
            os.makedirs(CONFIG["debug_dir"], exist_ok=True)
            path = os.path.join(CONFIG["debug_dir"], f"{name}_{int(time.time())}.png")
            self.driver.save_screenshot(path)
            logger.info(f"📸 已保存调试截图: {path}")
        except Exception as e:
            logger.error(f"截图失败: {str(e)}")

    def execute(self):
        """主执行流程"""
        try:
            if not self.init_driver():
                logger.error("❌ 浏览器初始化失败，请检查：")
                logger.error("1. 是否已经关闭所有Chrome进程")
                logger.error("2. 用户数据目录路径是否正确")
                logger.error("3. 是否安装了正确版本的Chrome浏览器")
                return

            self.navigate_to_target()
            self.smart_submit()
            self.monitor_generation()
            logger.info("🎉 全流程执行成功！")
            
        except Exception as e:
            logger.error(f"💥 致命错误: {str(e)}")
            self.capture_screenshot("final_error")
        finally:
            if self.driver:
                self.driver.quit()
                logger.info("🛑 浏览器已安全关闭")

if __name__ == "__main__":
    os.makedirs(CONFIG["debug_dir"], exist_ok=True)
    bot = UltimateMidjourneyBot()
    bot.execute()
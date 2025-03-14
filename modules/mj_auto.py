# mj_final_working.py
import os
import time
import logging
from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait
import undetected_chromedriver as uc

# ===================== 完整配置 =====================
CONFIG = {
    "user_data_dir": r"C:\Users\Administrator\AppData\Local\Google\Chrome\User Data",
    "driver_path": r"D:\AI_System\venv\Scripts\chromedriver.exe",
    "chrome_version": 134,
    "server_info": {
        "server_id": "1344917223021744221",  # 替换你的服务器ID
        "channel_id": "1344921569541095446",  # 替换你的频道ID
        "name_keyword": "creator Ai",
        "icon_src_part": "a533d38"
    },
    "prompt": "/imagine 赛博朋克风格手机壳 --v 5 --ar 9:16",
    "timeouts": {
        "page_load": 45,
        "element": 30,
        "generation": 600
    }
}
# ===================================================

logging.basicConfig(
    level=logging.INFO,
    format="[%(asctime)s][%(levelname)s] %(message)s",
    handlers=[
        logging.FileHandler("D:/AI_System/operation.log", encoding="utf-8"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

class PrecisionDiscordBot:
    def __init__(self):
        self.driver = None

    def init_driver(self):
        """初始化浏览器"""
        options = uc.ChromeOptions()
        options.add_argument("--disable-blink-features=AutomationControlled")
        
        try:
            self.driver = uc.Chrome(
                options=options,
                user_data_dir=CONFIG["user_data_dir"],
                driver_executable_path=CONFIG["driver_path"],
                version_main=CONFIG["chrome_version"],
                headless=False
            )
            logger.info("🚀 浏览器初始化成功")
            return True
        except Exception as e:
            logger.error(f"❌ 浏览器启动失败: {str(e)}")
            return False

    def navigate_server(self):
        """导航到目标服务器"""
        logger.info("🔄 正在定位服务器...")
        try:
            # 通过服务器ID精准定位
            server_selector = f'div[data-server-id="{CONFIG["server_info"]["server_id"]}"]'
            server = WebDriverWait(self.driver, CONFIG["timeouts"]["element"]).until(
                EC.element_to_be_clickable((By.CSS_SELECTOR, server_selector))
            )
            server.click()
            logger.info("✅ 已进入目标服务器")
            return True
        except Exception as e:
            logger.error(f"❌ 服务器定位失败: {str(e)}")
            self.driver.save_screenshot("D:/AI_System/server_error.png")
            raise

    def navigate_channel(self):
        """导航到目标频道"""
        logger.info("🔄 正在定位频道...")
        try:
            # 通过频道ID精准定位
            channel_selector = f'a[href*="channels/{CONFIG["server_info"]["server_id"]}/{CONFIG["server_info"]["channel_id"]}"]'
            channel = WebDriverWait(self.driver, CONFIG["timeouts"]["element"]).until(
                EC.element_to_be_clickable((By.CSS_SELECTOR, channel_selector))
            )
            channel.click()
            logger.info("✅ 已进入目标频道")
            return True
        except Exception as e:
            logger.error(f"❌ 频道定位失败: {str(e)}")
            self.driver.save_screenshot("D:/AI_System/channel_error.png")
            raise

    def input_prompt(self):
        """强化输入方法"""
        logger.info("⌨ 正在输入提示词...")
        try:
            input_selector = 'div[data-slate-editor="true"][role="textbox"]'
            input_box = WebDriverWait(self.driver, CONFIG["timeouts"]["element"]).until(
                EC.element_to_be_clickable((By.CSS_SELECTOR, input_selector))
            )
            
            # JavaScript直接操作
            self.driver.execute_script(f"""
                arguments[0].focus();
                arguments[0].innerText = '{CONFIG["prompt"]}';
                document.querySelector('button[type="submit"]').click();
            """, input_box)
            logger.info("✅ 提示词已提交")
        except Exception as e:
            logger.error(f"❌ 输入失败: {str(e)}")
            raise

    def execute(self):
        """主流程控制"""
        try:
            if not self.init_driver():
                return
                
            self.driver.get("https://discord.com/channels/@me")
            self.navigate_server()
            self.navigate_channel()
            self.input_prompt()
            
            logger.info("⏳ 等待生成完成...")
            time.sleep(CONFIG["timeouts"]["generation"])
            logger.info("🎉 流程执行成功")
            
        except Exception as e:
            logger.error(f"💥 致命错误: {str(e)}")
        finally:
            if self.driver:
                self.driver.quit()
                logger.info("🛑 浏览器已关闭")

if __name__ == "__main__":
    os.makedirs("D:/AI_System/debug", exist_ok=True)
    bot = PrecisionDiscordBot()
    bot.execute()
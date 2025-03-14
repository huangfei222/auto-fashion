# mj_final_version.py
import time
import logging
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import undetected_chromedriver as uc

# ================= 用户配置区 =================
CONFIG = {
    "chrome_profile": "D:/AI_System/chrome_profile",
    "chromedriver_path": "D:/AI_System/venv/Scripts/chromedriver.exe",
    "chrome_version": 134,
    "target_server": {
        "name_keyword": "creator Ai",  # 服务器名称包含的关键词
        "channel_name": "生成图片",  # 目标频道名称
        "icon_src_keyword": "a533d38",  # 图标链接包含的特征（见下方获取方法）
    },
    "wait_timeout": 30
}

# ================ 初始化日志 ================
logging.basicConfig(
    level=logging.INFO,
    format="[%(asctime)s] %(message)s",
    handlers=[
        logging.FileHandler("D:/AI_System/auto.log", encoding="utf-8"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

class ServerNavigator:
    def __init__(self):
        self.driver = None
        self._init_driver()
        
    def _init_driver(self):
        """初始化浏览器实例"""
        options = uc.ChromeOptions()
        options.add_argument("--disable-blink-features=AutomationControlled")
        self.driver = uc.Chrome(
            options=options,
            user_data_dir=CONFIG["chrome_profile"],
            driver_executable_path=CONFIG["chromedriver_path"],
            version_main=CONFIG["chrome_version"],
            headless=False
        )
        logger.info("✅ 浏览器初始化完成")

    def _get_server_element(self):
        """智能定位服务器元素"""
        strategies = [
            # 策略1：通过服务器名称模糊匹配
            (By.XPATH, f'//div[contains(@aria-label, "{CONFIG["target_server"]["name_keyword"]}")]'),
            
            # 策略2：通过图标链接特征匹配
            (By.CSS_SELECTOR, f'img[src*="{CONFIG["target_server"]["icon_src_keyword"]}"]'),
            
            # 策略3：通过服务器列表位置定位
            (By.CSS_SELECTOR, 'div[aria-label="服务器列表"] > div:nth-child(3)')  # 第3个服务器
        ]
        
        for strategy in strategies:
            try:
                element = WebDriverWait(self.driver, CONFIG["wait_timeout"]).until(
                    EC.presence_of_element_located(strategy)
                )
                logger.info(f"✅ 使用策略 {strategy} 定位成功")
                return element
            except:
                continue
        return None

    def navigate_to_server(self):
        """导航到目标服务器"""
        logger.info("🔄 正在定位目标服务器...")
        self.driver.get("https://discord.com/channels/@me")
        
        server_element = self._get_server_element()
        if server_element:
            server_element.click()
            logger.info("✅ 已进入目标服务器")
            return True
        
        logger.warning("⚠ 自动定位失败，进入手动模式")
        self._save_debug_info("before_server_select.png")
        input("请手动选择服务器后按回车继续...")
        self._save_debug_info("after_server_select.png")
        return True

    def navigate_to_channel(self):
        """导航到目标频道"""
        logger.info("🔄 正在定位目标频道...")
        try:
            channel = WebDriverWait(self.driver, CONFIG["wait_timeout"]).until(
                EC.presence_of_element_located((
                    By.XPATH,
                    f'//div[contains(@aria-label, "{CONFIG["target_server"]["channel_name"]}")]'
                ))
            )
            channel.click()
            logger.info("✅ 已进入目标频道")
            return True
        except Exception as e:
            self._save_debug_info("channel_error.png")
            logger.error(f"❌ 频道定位失败: {str(e)}")
            raise

    def _save_debug_info(self, filename):
        """保存调试信息"""
        self.driver.save_screenshot(f"D:/AI_System/debug/{filename}")
        with open(f"D:/AI_System/debug/{filename.split('.')[0]}.html", "w", encoding="utf-8") as f:
            f.write(self.driver.page_source)
        logger.info(f"⚠ 已保存调试文件：{filename}")

    def execute(self, prompt):
        """执行完整流程"""
        try:
            self.navigate_to_server()
            self.navigate_to_channel()
            
            input_box = WebDriverWait(self.driver, CONFIG["wait_timeout"]).until(
                EC.presence_of_element_located((By.XPATH, '//div[@role="textbox"]'))
            )
            input_box.send_keys(f"{prompt}\n")
            logger.info("✅ 提示词已发送")
            
        except Exception as e:
            logger.error(f"💥 流程异常终止: {str(e)}")
            raise
        finally:
            self.driver.quit()

if __name__ == "__main__":
    try:
        bot = ServerNavigator()
        bot.execute("/imagine 未来科技主题手机壳 --v 5 --ar 9:16")
        logger.info("🎉 流程执行成功！")
    except Exception as e:
        logger.error(f"💣 主程序崩溃: {str(e)}")
        exit(1)
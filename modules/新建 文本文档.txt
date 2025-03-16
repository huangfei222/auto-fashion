# mj_validation_script.py
import time
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

def validate_input_box():
    driver = webdriver.Chrome()
    try:
        driver.get("https://discord.com/login")
        input("请手动登录并导航到目标频道后按回车继续...")
        
        # 自动验证元素
        input_selector = 'div[data-slate-editor="true"][aria-label*="消息"]'
        box = WebDriverWait(driver, 30).until(
            EC.presence_of_element_located((By.CSS_SELECTOR, input_selector))
        )
        
        print("✅ 验证成功！输入框特征：")
        print(f"Class: {box.get_attribute('class')}")
        print(f"Attributes: {box.get_attribute('outerHTML')[:200]}")
        
    finally:
        driver.quit()

if __name__ == "__main__":
    validate_input_box()
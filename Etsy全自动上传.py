import os
import time
import random
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from dotenv import load_dotenv

load_dotenv("D:/AI_System/config/.env")

class EtsyAutoUploader:
    def __init__(self):
        self.driver = webdriver.Chrome()
        self.wait = WebDriverWait(self.driver, 20)
        
    def human_delay(self):
        """模拟人工操作间隔"""
        time.sleep(random.uniform(1.5, 4.2))
        
    def login(self):
        print("正在登录Etsy...")
        self.driver.get("https://www.etsy.com/signin")
        self.human_delay()
        
        # 输入邮箱
        email_field = self.wait.until(EC.presence_of_element_located((By.NAME, "email")))
        email_field.send_keys(os.getenv("ETSY_EMAIL"))
        
        # 输入密码
        self.driver.find_element(By.NAME, "password").send_keys(os.getenv("ETSY_PASSWORD"))
        self.human_delay()
        
        # 点击登录
        self.driver.find_element(By.NAME, "submit_attempt").click()
        print("✅ 登录成功！")
        time.sleep(8)
        
    def upload_product(self, psd_path, title="灵性设计模板", price=14.99):
        print(f"准备上传文件：{psd_path}")
        self.driver.get("https://www.etsy.com/your/shops/me/tools/listings/create")
        self.human_delay()
        
        # 上传文件
        upload_btn = self.wait.until(EC.presence_of_element_located((By.XPATH, "//input[@type='file']")))
        upload_btn.send_keys(psd_path)
        print("文件上传中...")
        time.sleep(10)  # 等待文件处理
        
        # 填写标题
        title_field = self.driver.find_element(By.NAME, "title")
        title_field.clear()
        for char in title:
            title_field.send_keys(char)
            time.sleep(random.uniform(0.1, 0.3))  # 模拟人工输入
        
        # 设置价格
        price_field = self.driver.find_element(By.NAME, "price")
        price_field.clear()
        price_field.send_keys(str(price))
        
        # 选择数字商品类型
        self.driver.find_element(By.XPATH, "//span[text()='数字文件']").click()
        
        # 提交
        submit_btn = self.driver.find_element(By.XPATH, "//button[contains(text(),'保存并继续')]")
        submit_btn.click()
        print("商品已提交审核！")
        time.sleep(15)

if __name__ == "__main__":
    uploader = EtsyAutoUploader()
    try:
        uploader.login()
        # 示例上传第一个PSD文件
        sample_psd = os.path.join(
            "D:\\AI_System\\output\\电子商品\\Canva模板",
            os.listdir("D:\\AI_System\\output\\电子商品\\Canva模板")[0]
        )
        uploader.upload_product(sample_psd)
    except Exception as e:
        print(f"❌ 发生错误：{str(e)}")
    finally:
        uploader.driver.quit()
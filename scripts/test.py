from undetected_chromedriver import Chrome
import time

options = ChromeOptions()
options.add_argument("--user-data-dir=C:/Users/你的用户名/AppData/Local/Google/Chrome/User Data")
options.add_argument("--profile-directory=Default")

driver = Chrome(
    options=options,
    headless=False,
    driver_executable_path="D:/AI_System/venv/Scripts/chromedriver.exe"
)
driver.get("https://discord.com")
input("看到正常界面后按回车退出...")
driver.quit()
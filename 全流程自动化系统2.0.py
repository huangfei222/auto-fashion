from undetected_chromedriver import ChromeOptions, Chrome

def quick_test():
    options = ChromeOptions()
    options.add_argument("--headless=new")  # 无头模式测试
    driver = Chrome(options=options)
    driver.get("https://www.canva.com/")
    print(driver.title)
    driver.quit()

if __name__ == "__main__":
    quick_test()
# 修改trend_scraper.py，增加以下防御机制：
from fake_useragent import UserAgent
import random
import time

class TrendHunter:
    def __init__(self):
        # 加载配置文件
        with open('D:/AI_System/config/scraper_protection.json') as f:
            self.protection = json.load(f)
        
    def _safe_request(self, url):
        """防封禁请求方法"""
        headers = {
            'User-Agent': random.choice(self.protection['user_agents']),
            'Accept-Language': 'en-US,en;q=0.9'
        }
        proxies = {
            'http': random.choice(self.protection['proxy_servers']),
            'https': random.choice(self.protection['proxy_servers'])
        }
        
        try:
            res = requests.get(url, headers=headers, proxies=proxies, timeout=10)
            time.sleep(self.protection['request_interval'])
            return res
        except Exception as e:
            print(f"请求失败: {str(e)}")
            return None
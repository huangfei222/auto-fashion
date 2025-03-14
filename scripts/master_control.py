import schedule
import time
from modules.trend_scraper import TrendHunter
from modules.prompt_generator import PromptGenerator
from modules.mj_auto import MidjourneyBot
from stable_factory import EnhancedFactory

class AutoPilot:
    def __init__(self):
        self.trend_hunter = TrendHunter()
        self.prompt_gen = PromptGenerator()
        self.mj_bot = MidjourneyBot()
        self.factory = EnhancedFactory()
        
    def daily_job(self):
        """每日全流程任务"""
        try:
            # 阶段1：获取趋势
            trends = self.trend_hunter.get_daily_trends()
            
            # 阶段2：生成提示词
            prompts = [
                self.prompt_gen.generate('手机壳', trends['pinterest']),
                self.prompt_gen.generate('T恤', trends['amazon'])
            ]
            
            # 阶段3：Midjourney生成
            for prompt in prompts:
                self.mj_bot.generate(prompt)
            
            # 阶段4：自动处理
            self.factory.run_continuously()
            
        except Exception as e:
            self.send_alert(f"每日任务失败: {str(e)}")
    
    def send_alert(self, message):
        """发送报警通知"""
        # 接入短信/邮件通知API
        print(f"⚠️ 系统警报: {message}")

if __name__ == "__main__":
    pilot = AutoPilot()
    schedule.every().day.at("03:00").do(pilot.daily_job)
    
    while True:
        schedule.run_pending()
        time.sleep(60)
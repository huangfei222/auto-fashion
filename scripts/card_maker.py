# card_maker.py
from PIL import Image, ImageDraw, ImageFont
import random
import time
from pathlib import Path

def generate_cards():
    card_types = ['婚礼', '节日']
    
    for card_type in card_types:
        output_dir = Path(f"D:/AI_System/data/raw/电子模板素材/{card_type}请柬")
        output_dir.mkdir(parents=True, exist_ok=True)
        
        # 字体安全检测
        font_paths = [
            "C:/Windows/Fonts/simhei.ttf",
            "C:/Windows/Fonts/msyh.ttc",
            "C:/Windows/Fonts/simfang.ttf"
        ]
        available_fonts = [f for f in font_paths if Path(f).exists()]
        
        for i in range(5):  # 每种生成5个
            try:
                # 随机颜色方案
                bg_color = (
                    random.randint(200, 255),
                    random.randint(200, 255),
                    random.randint(200, 255)
                )
                img = Image.new('RGB', (1200, 800), color=bg_color)
                draw = ImageDraw.Draw(img)
                
                # 动态生成内容
                if card_type == '婚礼':
                    groom = random.choice(['张', '王', '李']) + random.choice(['伟','强','磊'])
                    bride = random.choice(['陈','刘','杨']) + random.choice(['芳','娜','婷'])
                    text = f"诚邀您参加\n{groom} 先生 & {bride} 女士\n婚礼盛典\n时间：2025年良辰吉日\n地点：幸福大酒店"
                else:
                    festival = random.choice(['春节', '中秋', '元旦'])
                    text = f"恭祝{random.choice(['全家','贵公司','亲朋好友'])}\n{festival}快乐\n{random.choice(['心想事成','财源广进','阖家幸福'])}"
                
                # 排版设置
                font = ImageFont.truetype(random.choice(available_fonts), random.choice([36, 40, 44]))
                draw.multiline_text(
                    (600, 300), 
                    text, 
                    fill=(0,0,0), 
                    font=font, 
                    align='center', 
                    anchor='mm'
                )
                
                # 唯一文件名（毫秒级时间戳）
                timestamp = int(time.time() * 1000) + i
                img.save(output_dir / f"{card_type}请柬_{timestamp}.png")
                print(f"✅ {card_type}请柬已生成：{card_type}请柬_{timestamp}.png")
                
            except Exception as e:
                print(f"❌ {card_type}请柬生成失败：{str(e)}")

if __name__ == "__main__":
    generate_cards()
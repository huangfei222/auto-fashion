# price_updater.py
import pandas as pd
from datetime import datetime

def update_price():
    df = pd.read_excel("D:/AI_System/config/pricing.xlsx")
    
    # 动态调整PPT价格
    ppt_mask = df['产品类型'] == 'PPT模板'
    df.loc[ppt_mask, '基础价'] = df.loc[ppt_mask, '基础价'] * 1.05  # 每月涨价5%
    
    # 保存更新
    df['最近调价'] = datetime.now().strftime("%Y-%m-%d")
    df.to_excel("D:/AI_System/config/pricing.xlsx", index=False)

if __name__ == "__main__":
    update_price()
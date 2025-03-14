# table_builder.py
import pandas as pd
from faker import Faker
import random
from pathlib import Path
import time

def generate_all_tables():
    fake = Faker('zh_CN')
    types = ['年度预算表', '课程表', '记账表']
    
    for table_type in types:
        output_dir = Path(f"D:/AI_System/data/raw/电子模板素材/表格模板/{table_type}")
        output_dir.mkdir(parents=True, exist_ok=True)
        
        for i in range(5):  # 每种生成5个
            try:
                data = []
                timestamp = int(time.time() * 1000) + i  # 唯一时间戳
                
                # 数据生成逻辑
                if table_type == '年度预算表':
                    columns = ['项目名称', '预算金额', '实际支出', '负责人']
                    for _ in range(50):
                        data.append([
                            fake.bs(),
                            round(random.uniform(1000,50000), 2),
                            round(random.uniform(800,45000), 2),
                            fake.name()
                        ])
                elif table_type == '课程表':
                    columns = ['星期', '节次', '课程名称', '教室']
                    for _ in range(50):
                        data.append([
                            f"周{['一','二','三','四','五'][random.randint(0,4)]}",
                            f"{random.randint(1,6)}-{random.randint(7,8)}节",
                            fake.catch_phrase(),
                            f"{random.choice(['A','B','C'])}{random.randint(101,599)}"
                        ])
                else:
                    columns = ['日期', '类型', '金额', '支付方式']
                    for _ in range(50):
                        data.append([
                            fake.date_this_year(),
                            random.choice(['餐饮','交通','购物']),
                            round(random.uniform(10,2000), 2),
                            random.choice(['微信','支付宝','现金'])
                        ])
                
                df = pd.DataFrame(data, columns=columns)
                df.to_excel(output_dir / f"{table_type}_{timestamp}.xlsx", index=False)
                print(f"✅ {table_type}已生成：{table_type}_{timestamp}.xlsx")
                
            except Exception as e:
                print(f"❌ {table_type}生成失败：{str(e)}")

if __name__ == "__main__":
    generate_all_tables()
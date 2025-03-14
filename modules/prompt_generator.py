from transformers import pipeline
import random

class PromptGenerator:
    def __init__(self):
        self.classifier = pipeline('zero-shot-classification', 
                                 model='facebook/bart-large-mnli')

    def generate(self, trends):
        styles = ['赛博朋克', '水墨风', '极简主义']
        classification = self.classifier(trends, styles)
        selected_style = classification['labels'][0]
        
        prompt = f"{selected_style}风格{random.choice(['手机壳','T恤'])}设计，"
        prompt += f"包含{random.choice(trends[:2])}元素 --v 5 --ar 3:4"
        return prompt

if __name__ == "__main__":
    pg = PromptGenerator()
    print(pg.generate(["透明磁吸", "卡通人物"]))
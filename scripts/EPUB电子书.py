# EPUB电子书.py
import os
from pathlib import Path
from ebooklib import epub
from datetime import datetime
import time
import random

def generate_ebook(book_type):
    try:
        # 创建输出目录
        output_dir = Path(f"D:/AI_System/data/raw/电子书模板/{book_type}")
        output_dir.mkdir(parents=True, exist_ok=True)
        
        # 唯一文件名（毫秒+随机数）
        timestamp = f"{datetime.now().strftime('%Y%m%d%H%M%S')}_{int(time.time()*1000%1000):03d}"
        filename = output_dir / f"{book_type}_模板_{timestamp}.epub"
        
        # 生成书籍内容
        book = epub.EpubBook()
        book.set_identifier(str(random.randint(100000,999999)))
        book.set_title(f"AI生成{book_type}模板")
        book.add_author("智能创作助手")
        
        # 添加示例章节
        chap = epub.EpubHtml(title="示例章节", file_name="chap_1.xhtml")
        chap.content = f'<h1>{book_type}模板示例</h1><p>{" ".join(["示例内容"]*50)}</p>'
        book.add_item(chap)
        
        # 保存文件
        epub.write_epub(filename, book)
        print(f"✅ 真实生成：{filename}")
        return True
    except Exception as e:
        print(f"❌ 生成失败：{str(e)}")
        return False

if __name__ == "__main__":
    success_count = 0
    for book_type in ['小说', '学术', '儿童']:
        if generate_ebook(book_type):
            success_count += 1
    print(f"生成完成，成功{success_count}/3种电子书")
    exit(0 if success_count == 3 else 1)
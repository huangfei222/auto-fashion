# ppt_to_pdf.py
import os
from pptx import Presentation
from pdf2image import convert_from_path
from pathlib import Path

def convert_pptx_to_pdf(ppt_path, pdf_path):
    """ 使用python-pptx原生渲染生成高保真PDF """
    prs = Presentation(ppt_path)
    
    # 设置打印选项
    prs.slide_width = 12192000  # 16:9宽屏尺寸（单位：EMU）
    prs.slide_height = 6858000
    
    # 保存为PDF
    prs.save(pdf_path)

def enhance_pdf_quality(pdf_path, dpi=300):
    """ 将PDF转换为高清图片再重组为PDF """
    images = convert_from_path(
        pdf_path, 
        dpi=dpi,
        poppler_path=r"D:\AI_System\tools\poppler-25.03.0"
    )
    
    # 保存为增强版PDF
    images[0].save(
        pdf_path.with_stem(f"{pdf_path.stem}_enhanced"),
        "PDF", 
        resolution=dpi,
        save_all=True,
        append_images=images[1:]
    )

if __name__ == "__main__":
    input_folder = Path(r"D:\AI_System\data\raw\电子模板素材\可编辑PPT")
    output_folder = input_folder / "pdf_output"
    output_folder.mkdir(exist_ok=True)
    
    for ppt_file in input_folder.glob("*.pptx"):
        pdf_path = output_folder / f"{ppt_file.stem}.pdf"
        
        # 第一步：生成基础PDF
        convert_pptx_to_pdf(ppt_file, pdf_path)
        
        # 第二步：质量增强
        enhance_pdf_quality(pdf_path)
        
        print(f"✅ 已生成高清PDF：{pdf_path}")
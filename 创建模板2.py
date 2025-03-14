from PIL import Image

def create_template(name, size, dpi=300):
    img = Image.new('RGB', size, (255,255,255))
    img.save(f"D:/AI_System/templates/{name}.png", dpi=(dpi, dpi))

create_template("tshirt_template", (4500,5400))  # T恤模板
create_template("poster_template", (6000,9000))   # 海报模板
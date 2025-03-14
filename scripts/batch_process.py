def batch_process(filename):
    img = pdb.gimp_file_load(filename, filename)
    layer = pdb.gimp_text_layer_new(img, "双击编辑文字", "Arial", 50, 0)
    pdb.gimp_image_insert_layer(img, layer, None, 0)
    pdb.file_psd_save(img, img.layers[0], filename.replace('.png','.psd'), filename.replace('.png','.psd'))
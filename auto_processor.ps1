# ---------- 用户配置区 ----------
$sourceDir = "D:\AI_System\assets\wallpaper\raw"
$outputDir = "D:\AI_System\assets\wallpaper\processed"
$watermark = "D:\AI_System\assets\watermark\logo.png"  # 水印文件路径
$sizes = @("3840x2160", "1920x1080")                   # 生成尺寸
$quality = 90                                          # 图片质量
# ------------------------------

# 创建水印（如果不存在）
if (-not (Test-Path $watermark)) {
    magick -size 600x200 xc:none -fill "rgba(255,255,255,0.3)" -font "Arial" -pointsize 36 -gravity center -annotate 0 "© AI Design 2025" $watermark
}

# 处理函数
function Process-Image {
    param($file)
    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file)
    
    foreach ($size in $sizes) {
        $output = Join-Path $outputDir "$baseName`_$size.webp"
        
        magick convert $file `
            -resize "$size^" -gravity center -extent $size `  # 智能缩放裁剪
            $watermark -gravity southeast -geometry +50+50 -composite `  # 右下角水印
            -quality $quality `
            -define webp:method=6 `
            $output
        
        # 添加元数据
        exiftool -overwrite_original -Copyright="© 2025 AI Generated" $output
    }
}

# 实时监控
$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $sourceDir
$watcher.Filter = "*.png"
$watcher.EnableRaisingEvents = $true

Register-ObjectEvent $watcher "Created" -Action {
    Start-Process powershell -ArgumentList "-File `"$PSScriptRoot\auto_processor.ps1`""
}

# 保持脚本运行
while ($true) { Start-Sleep -Seconds 60 }
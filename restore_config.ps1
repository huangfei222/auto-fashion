cd D:\AI_System
git lfs install
git lfs track "output/**/*.tif"
git add .
git commit -m "恢复配置 $(Get-Date)"
git push origin main

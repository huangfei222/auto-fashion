$workflow = ".github/workflows/auto-pipeline.yml"
$checksum = ".continuity/config_checksum.sha256"

# 生成哈希
$hash = python -c "import hashlib; f=open(r'$workflow','rb'); print(hashlib.sha256(f.read()).hexdigest())"
$hash | Out-File $checksum -Encoding ASCII

# 验证哈希
$stored = Get-Content $checksum
$actual = python -c "import hashlib; f=open(r'$workflow','rb'); print(hashlib.sha256(f.read()).hexdigest())"

if ($stored -eq $actual) {
    Write-Host "[√] 验证通过" -ForegroundColor Green
} else {
    Write-Host "[×] 验证失败" -ForegroundColor Red
    Write-Host "存储指纹: $stored"
    Write-Host "实际指纹: $actual"
}
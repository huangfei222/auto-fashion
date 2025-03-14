$context = Get-Content D:/AI_SYSTEM/.continuity/config.json | ConvertFrom-Json
Write-Host "✅ 已加载配置：项目[$($context.current_project)] 最后步骤[$($context.last_step)]"
Write-Host "!restore-success"
# Script tải hình ảnh từ sasin.vn
$outputDir = Join-Path $PSScriptRoot '..\wwwroot\images\products'

# Tạo thư mục nếu chưa có
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    Write-Host "Created directory: $outputDir" -ForegroundColor Green
}

$Urls = @(
    'https://sasin.vn/menu/index/5?pageIndex=1',
    'https://sasin.vn/menu/index/6?pageIndex=1',
    'https://sasin.vn/menu/index/18?pageIndex=1',
    'https://sasin.vn/menu/index/7?pageIndex=1',
    'https://sasin.vn/menu/index/10?pageIndex=1',
    'https://sasin.vn/menu/index/19?pageIndex=1',
    'https://sasin.vn/menu/index/3?pageIndex=1',
    'https://sasin.vn/menu/index/9?pageIndex=1',
    'https://sasin.vn/menu/index/2?pageIndex=1',
    'https://sasin.vn/menu/index/8?pageIndex=1'
)

$allImages = @()
$downloadedCount = 0

foreach ($url in $Urls) {
    Write-Host "`nFetching: $url" -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 30
        $html = $response.Content
        
        # Pattern: https://sasin.vn:8002//Resource/Image/...
        $imgMatches = [regex]::Matches($html, 'src="(https://sasin\.vn:8002//Resource/Image/[^"]+)"')
        
        foreach ($match in $imgMatches) {
            $fullUrl = $match.Groups[1].Value
            
            if ($allImages -notcontains $fullUrl) {
                $allImages += $fullUrl
            }
        }
        
        Write-Host "  Found images so far: $($allImages.Count)" -ForegroundColor Yellow
        
    } catch {
        Write-Host "  Error fetching $url : $_" -ForegroundColor Red
    }
    
    Start-Sleep -Milliseconds 500
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "Total unique images found: $($allImages.Count)" -ForegroundColor Magenta
Write-Host "========================================`n" -ForegroundColor Magenta

# Tải từng hình ảnh
foreach ($imgUrl in $allImages) {
    # Tạo tên file từ URL path
    # VD: https://sasin.vn:8002//Resource/Image/MenuItem/M00012/Image/MenuItem.webp
    # -> MenuItem_M00012.webp
    
    $urlPath = $imgUrl -replace 'https://sasin\.vn:8002//Resource/Image/', ''
    $parts = $urlPath -split '/'
    
    if ($parts.Count -ge 2) {
        $category = $parts[0]  # MenuItem, MenuItemGroup
        $code = $parts[1]      # M00012, MG00005
        $ext = [System.IO.Path]::GetExtension($imgUrl.Split('?')[0])
        $fileName = "${category}_${code}${ext}"
    } else {
        $fileName = [System.IO.Path]::GetFileName($imgUrl.Split('?')[0])
    }
    
    $outputPath = Join-Path $outputDir $fileName
    
    if (Test-Path $outputPath) {
        Write-Host "  Skipped (exists): $fileName" -ForegroundColor Gray
        continue
    }
    
    try {
        Write-Host "  Downloading: $fileName" -ForegroundColor White
        Invoke-WebRequest -Uri $imgUrl -OutFile $outputPath -UseBasicParsing -TimeoutSec 60
        $downloadedCount++
        Start-Sleep -Milliseconds 300
    } catch {
        Write-Host "  Failed: $fileName - $_" -ForegroundColor Red
    }
}

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "Download complete!" -ForegroundColor Green
Write-Host "Downloaded: $downloadedCount new images" -ForegroundColor Green
Write-Host "Location: $outputDir" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

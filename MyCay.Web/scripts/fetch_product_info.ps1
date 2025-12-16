# Script lay thong tin san pham tu sasin.vn
$outputDir = $PSScriptRoot

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

foreach ($url in $Urls) {
    Write-Host "Fetching: $url" -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 30
        $html = $response.Content
        
        # Tim cac product - img, title
        $imgMatches = [regex]::Matches($html, 'src="(https://sasin\.vn:8002//Resource/Image/MenuItem/[^"]+)"')
        $titleMatches = [regex]::Matches($html, '<h5[^>]*>([^<]+)</h5>')
        $priceMatches = [regex]::Matches($html, '(\d{1,3}(?:[.,]\d{3})*)\s*d')
        
        Write-Host "  Found: $($imgMatches.Count) images, $($titleMatches.Count) titles" -ForegroundColor Yellow
        
        if ($titleMatches.Count -gt 0) {
            Write-Host "  Sample titles:" -ForegroundColor Green
            for ($i = 0; $i -lt [Math]::Min(5, $titleMatches.Count); $i++) {
                Write-Host "    - $($titleMatches[$i].Groups[1].Value)" -ForegroundColor White
            }
        }
        
    } catch {
        Write-Host "  Error: $_" -ForegroundColor Red
    }
    
    Start-Sleep -Milliseconds 300
}

# Luu sample HTML de phan tich
$sampleUrl = 'https://sasin.vn/menu/index/5?pageIndex=1'
try {
    Write-Host "`nSaving sample HTML..." -ForegroundColor Cyan
    $response = Invoke-WebRequest -Uri $sampleUrl -UseBasicParsing -TimeoutSec 30
    $sampleFile = Join-Path $outputDir 'sample_menu.html'
    $response.Content | Out-File -FilePath $sampleFile -Encoding UTF8
    Write-Host "Saved to: $sampleFile" -ForegroundColor Green
} catch {
    Write-Host "Error: $_" -ForegroundColor Red
}

Write-Host "`nDone!" -ForegroundColor Green

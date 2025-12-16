# Script lay thong tin san pham tu sasin.vn
Add-Type -AssemblyName System.Web

$outputDir = $PSScriptRoot
$outputFile = Join-Path $outputDir 'products_data.json'

$Urls = @(
    @{url='https://sasin.vn/menu/index/5?pageIndex=1'; category='Mi Cay'},
    @{url='https://sasin.vn/menu/index/6?pageIndex=1'; category='Mi Tuong Den'},
    @{url='https://sasin.vn/menu/index/18?pageIndex=1'; category='Mi Xao'},
    @{url='https://sasin.vn/menu/index/7?pageIndex=1'; category='Mon Khac'},
    @{url='https://sasin.vn/menu/index/10?pageIndex=1'; category='Mon Them Mi'},
    @{url='https://sasin.vn/menu/index/19?pageIndex=1'; category='Combo'},
    @{url='https://sasin.vn/menu/index/3?pageIndex=1'; category='Lau Han Quoc'},
    @{url='https://sasin.vn/menu/index/9?pageIndex=1'; category='Mon Them Lau'},
    @{url='https://sasin.vn/menu/index/2?pageIndex=1'; category='Khai Vi'},
    @{url='https://sasin.vn/menu/index/8?pageIndex=1'; category='Giai Khat'}
)

$allProducts = @()

foreach ($item in $Urls) {
    $url = $item.url
    $category = $item.category
    Write-Host "Fetching: $category" -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 30
        $html = $response.Content
        
        $itemPattern = '(?s)<div[^>]*class="[^"]*item[^"]*"[^>]*>.*?<img[^>]*src="(https://sasin\.vn:8002//Resource/Image/MenuItem/([^/]+)/[^"]+)"[^>]*>.*?<p[^>]*class="barlow-bold-24[^"]*"[^>]*>([^<]+)</p>.*?<p[^>]*class="barlow-regular-21[^"]*"[^>]*>([^<]*)</p>.*?<p[^>]*class="barlow-bold-21[^"]*"[^>]*>([^<]+)</p>.*?</div>'
        
        $matches = [regex]::Matches($html, $itemPattern)
        
        Write-Host "  Found: $($matches.Count) products" -ForegroundColor Yellow
        
        foreach ($m in $matches) {
            $imgUrl = $m.Groups[1].Value
            $code = $m.Groups[2].Value
            $name = $m.Groups[3].Value.Trim()
            $desc = $m.Groups[4].Value.Trim()
            $price = $m.Groups[5].Value.Trim()
            
            # Decode HTML entities
            $name = [System.Web.HttpUtility]::HtmlDecode($name)
            $desc = [System.Web.HttpUtility]::HtmlDecode($desc)
            
            $product = [PSCustomObject]@{
                code = $code
                name = $name
                description = $desc
                price = $price
                category = $category
                image = "MenuItem_$code.webp"
            }
            
            $allProducts += $product
            Write-Host "    - $name : $price" -ForegroundColor White
        }
        
    } catch {
        Write-Host "  Error: $_" -ForegroundColor Red
    }
    
    Start-Sleep -Milliseconds 300
}

Write-Host "`nTotal products: $($allProducts.Count)" -ForegroundColor Green

# Luu JSON
$json = $allProducts | ConvertTo-Json -Depth 3
$json | Out-File -FilePath $outputFile -Encoding UTF8
Write-Host "Saved to: $outputFile" -ForegroundColor Green

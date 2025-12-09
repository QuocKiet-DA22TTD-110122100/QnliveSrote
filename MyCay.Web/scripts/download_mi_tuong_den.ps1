# Script to download images for Mi Tuong Den from sasin.vn
# Category ID: 6 (Mi Tuong Den)

$categoryId = 6
$categoryName = "Mì Tương Đen"
$outputDir = "MyCay.Web\wwwroot\images\sasin\index-6"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DOWNLOAD IMAGES - $categoryName" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Create output directory if not exists
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    Write-Host "[✓] Created directory: $outputDir" -ForegroundColor Green
} else {
    Write-Host "[✓] Directory exists: $outputDir" -ForegroundColor Green
}

# Set TLS 1.2 for HTTPS
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Write-Host ""
Write-Host "Fetching product data from sasin.vn..." -ForegroundColor Yellow

try {
    # Fetch the page
    $url = "https://sasin.vn/menu/index/$categoryId`?pageIndex=1"
    $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 30
    
    Write-Host "[✓] Successfully fetched page" -ForegroundColor Green
    
    # Parse HTML to find images
    $html = $response.Content
    
    # Find all image URLs (looking for product images)
    $imagePattern = 'https://sasin\.vn/uploads/product/[^"'']+\.(jpg|jpeg|png|webp)'
    $matches = [regex]::Matches($html, $imagePattern)
    
    if ($matches.Count -eq 0) {
        Write-Host "[!] No images found. Trying alternative pattern..." -ForegroundColor Yellow
        
        # Try alternative pattern
        $imagePattern = 'src="([^"]+uploads/product/[^"]+)"'
        $matches = [regex]::Matches($html, $imagePattern)
    }
    
    $imageUrls = @()
    foreach ($match in $matches) {
        $imgUrl = $match.Value -replace 'src="', '' -replace '"', ''
        if ($imgUrl -notlike "http*") {
            $imgUrl = "https://sasin.vn" + $imgUrl
        }
        if ($imgUrl -notin $imageUrls) {
            $imageUrls += $imgUrl
        }
    }
    
    Write-Host "[✓] Found $($imageUrls.Count) unique images" -ForegroundColor Green
    Write-Host ""
    
    if ($imageUrls.Count -eq 0) {
        Write-Host "[!] No images found. Please check the URL or try manual download." -ForegroundColor Red
        Write-Host "URL: $url" -ForegroundColor Yellow
        exit 1
    }
    
    # Download images
    $metadata = @()
    $counter = 1
    
    foreach ($imgUrl in $imageUrls) {
        try {
            Write-Host "[$counter/$($imageUrls.Count)] Downloading: $imgUrl" -ForegroundColor Cyan
            
            # Get file extension
            $ext = [System.IO.Path]::GetExtension($imgUrl)
            if ([string]::IsNullOrEmpty($ext)) {
                $ext = ".webp"
            }
            
            # Generate filename
            $filename = "img-$($counter.ToString('D3'))$ext"
            $outputPath = Join-Path $outputDir $filename
            
            # Download image
            Invoke-WebRequest -Uri $imgUrl -OutFile $outputPath -TimeoutSec 30
            
            Write-Host "    [✓] Saved as: $filename" -ForegroundColor Green
            
            # Add to metadata
            $metadata += @{
                Title = "$categoryName $counter"
                Price = "65,000₫"
                Image = $filename
            }
            
            $counter++
            Start-Sleep -Milliseconds 500  # Be nice to the server
            
        } catch {
            Write-Host "    [✗] Failed: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
    
    # Save metadata.json
    Write-Host ""
    Write-Host "Saving metadata..." -ForegroundColor Yellow
    
    $metadataPath = Join-Path $outputDir "metadata.json"
    $metadata | ConvertTo-Json -Depth 10 | Out-File -FilePath $metadataPath -Encoding UTF8
    
    Write-Host "[✓] Metadata saved: $metadataPath" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "  DOWNLOAD COMPLETED!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Total images downloaded: $($counter - 1)" -ForegroundColor Green
    Write-Host "Location: $outputDir" -ForegroundColor Green
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Host "[✗] Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "1. Check your internet connection" -ForegroundColor White
    Write-Host "2. Verify the URL is accessible: $url" -ForegroundColor White
    Write-Host "3. The website might be blocking automated requests" -ForegroundColor White
    Write-Host ""
    exit 1
}

param(
  [string]$OutDir = "MyCay.Web\wwwroot\images\sasin"
)

function Write-Json($path,$obj){
  $json = $obj | ConvertTo-Json -Depth 4
  $dir = Split-Path $path -Parent
  if (!(Test-Path $dir)) { New-Item -ItemType Directory -Path $dir | Out-Null }
  Set-Content -Path $path -Value $json -Encoding UTF8
}

$index5 = @(
  @{ title = "Mì Thập Cẩm No Nê (Kim Chi/ Soyum/ Sincay)"; price = "77,000VNĐ" },
  @{ title = "Mì Thập Cẩm (Kim chi/ Soyum/ Sincay)"; price = "69,000VNĐ" },
  @{ title = "Mì Hải Sản (Kim chi/ Soyum/ Sincay)"; price = "62,000VNĐ" },
  @{ title = "Mì Hải Sản Thanh Cua (Kim chi/ Soyum/ Sincay)"; price = "62,000VNĐ" },
  @{ title = "Mì Bò Mỹ (Kim chi/ Soyum/ Sincay)"; price = "59,000VNĐ" },
  @{ title = "Mì Bò Trứng (Kim chi/ Soyum/ Sincay)"; price = "65,000VNĐ" },
  @{ title = "Mì Đùi Gà (Kim chi/ Soyum/ Sincay)"; price = "59,000VNĐ" },
  @{ title = "Mì Kim Chi Cá"; price = "49,000VNĐ" },
  @{ title = "Mì Kim Chi Gogi"; price = "49,000VNĐ" },
  @{ title = "Mì Kim Chi Xúc Xích Cá Viên"; price = "39,000VNĐ" }
)

$out5 = Join-Path $OutDir 'index-5\metadata.json'
Write-Json -path $out5 -obj $index5
Write-Host "Wrote metadata: $out5"

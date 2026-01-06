param(
  [string]$OutDir = "MyCay.Web\wwwroot\images\sasin",
  [string[]]$Urls
)

if (!(Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

function Get-Ext($u){
  try { $uri = [Uri]$u; $ext = [System.IO.Path]::GetExtension($uri.AbsolutePath) } catch { $ext = '' }
  if ([string]::IsNullOrWhiteSpace($ext)) { $ext = '.jpg' }
  if ($ext.Contains('?')) { $ext = $ext.Split('?')[0] }
  return $ext
}

function AbsUrl($base,$src){
  if ($src.StartsWith('http')) { return $src }
  if ($src.StartsWith('//')) { return 'https:' + $src }
  if ($src.StartsWith('/')) { return ($base.TrimEnd('/')) + $src }
  return ($base.TrimEnd('/') + '/' + $src)
}

function HashName($text){
  $md5 = [System.Security.Cryptography.MD5]::Create()
  $bytes = [System.Text.Encoding]::UTF8.GetBytes($text)
  $hash = $md5.ComputeHash($bytes)
  -join ($hash | ForEach-Object { $_.ToString('x2') })
}

$base = 'https://sasin.vn'
if (-not $Urls -or $Urls.Count -eq 0) {
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
}

foreach ($u in $Urls) {
  try {
    $res = Invoke-WebRequest -Uri $u -UseBasicParsing -TimeoutSec 30
    $html = $res.Content
  } catch {
    Write-Host ("Fail GET {0}: {1}" -f $u, $_); continue
  }

  $matches = [regex]::Matches($html, '(?i)(?:src|data-src)="([^"]+\.(?:jpg|jpeg|png|webp))(?:\?[^\"]*)?"')
  $catMatch = [regex]::Match($u, 'index\/(\d+)')
  $catId = if ($catMatch.Success) { $catMatch.Groups[1].Value } else { 'unknown' }
  $catDir = Join-Path $OutDir ('index-' + $catId)
  if (!(Test-Path $catDir)) { New-Item -ItemType Directory -Path $catDir | Out-Null }

  $seen = @{}
  foreach ($m in $matches) {
    $src = $m.Groups[1].Value
    $abs = AbsUrl $base $src
    if ($seen.ContainsKey($abs)) { continue } else { $seen[$abs] = $true }
    $ext = Get-Ext $abs
    $name = 'img-' + (HashName $abs) + $ext
    $outFile = Join-Path $catDir $name
    try {
      Invoke-WebRequest -Uri $abs -OutFile $outFile -TimeoutSec 60
      Write-Host "Saved: $outFile"
    } catch {
      Write-Host ("Fail DL {0}: {1}" -f $abs, $_)
    }
  }
}

# Function to read .env file
function Get-EnvVariables {
    $envFile = ".\.env"
    if (Test-Path $envFile) {
        Get-Content $envFile | ForEach-Object {
            if ($_ -match '^\s*([^#][^=]+)=(.*)$') {
                $name = $matches[1].Trim()
                $value = $matches[2].Trim()
                Set-Variable -Name $name -Value $value -Scope Script
            }
        }
    }
    else {
        Write-Host "Warning: .env file not found" -ForegroundColor Yellow
        $TAG=latest
        $REGISTRY_URL=cr.haihv.vn
        $USERNAME=haihv
        $PASSWORD=Abc@1234
        $DOCKERHUB = haitnmt
    }
}
# Load environment variables at start
Get-EnvVariables

$ImageName = "tracuugcn-api"
#Lấy thời gian bắt đầu
$startTime = Get-Date
$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Write-Host "[$currentTime] Đang đăng nhập vào registry $REGISTRY_URL..." -ForegroundColor Yellow
${PASSWORD} | docker login ${REGISTRY_URL} -u ${USERNAME} --password-stdin

#Lấy thời gian hiện tại:
$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

Write-Host "[$currentTime] Bắt đầu build Api" -ForegroundColor Blue

# Đọc version từ file .csproj latest
$csprojContent = Get-Content -Path ".\Haihv.Elis.Tool.TraCuuGcn.Api\Haihv.Elis.Tool.TraCuuGcn.Api.csproj" -Raw
$version = [regex]::Match($csprojContent, '<AssemblyVersion>(.*?)</AssemblyVersion>').Groups[1].Value

$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
#Build new image
Write-Host "[$currentTime] Đang build api image verion: $version" -ForegroundColor Yellow
#Build new image without using cache: --no-cache
#Create Tag Image to $DOCKERHUB
docker build -t ${DOCKERHUB}/${ImageName}:${version} -f DockerfileApi .
$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Write-Host "[$currentTime] Đã build image verion: $version thành công." -ForegroundColor Green

#Create Tag Image latest to $DOCKERHUB
docker tag ${DOCKERHUB}/${ImageName}:${version} ${DOCKERHUB}/${ImageName}:${TAG}

#Push Image Api to $REGISTRY_URL
$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Write-Host "[$currentTime] Bắt đầu đẩy api image version $version lên $REGISTRY_URL" -ForegroundColor Yellow
docker tag ${DOCKERHUB}/${ImageName}:${version} ${REGISTRY_URL}/${ImageName}:${version}
docker push ${REGISTRY_URL}/${ImageName}:${version}

$currentTime = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Write-Host "[$currentTime] Bắt đầu đẩy api image [$TAG] lên [$REGISTRY_URL]" -ForegroundColor Yellow
docker tag ${REGISTRY_URL}/${ImageName} ${REGISTRY_URL}/${ImageName}:${TAG}
docker push ${REGISTRY_URL}/${ImageName}:${TAG}

$endTime = Get-Date
$totalTime = $endTime - $startTime
$formattedTime = "{0:hh\:mm\:ss}" -f $totalTime
Write-Host "[$currentTime] Kết thúc build Api, thời gian thực hiện: $formattedTime" -ForegroundColor Cyan
# This script installs / updates steamcmd and Unturned 3 on Windows machines
# Syntax: powershell ./update.ps1 <steam username> <steam password>
# Author: fr34kyn01535
# Note: To make sure Steam Guard is not bugging you better create a new Steam account and disable Steam Guard
param(
   [Parameter(Mandatory=$True,valueFromPipeline=$true)]
	[string]$steamUsername, 
   [Parameter(Mandatory=$True,valueFromPipeline=$true)]
	[string]$steamPassword
)
$STEAMCMD_HOME = "steamcmd"
$UNTURNED_HOME = "unturned"

New-Item -ItemType Directory -Force -Path $STEAMCMD_HOME
New-Item -ItemType Directory -Force -Path $UNTURNED_HOME

cd $STEAMCMD_HOME
if (-Not (Test-Path "steamcmd.exe")){
    Invoke-WebRequest "https://rocket.foundation/steamcmd.exe" -outFile "steamcmd.exe"
}
Start-Process steamcmd.exe -ArgumentList "+login $steamUsername $steamPassword +force_install_dir ../unturned +app_update 304930 validate -beta preview -betapassword OPERATIONMAPLELEAF +exit"
cd "../$UNTURNED_HOME"
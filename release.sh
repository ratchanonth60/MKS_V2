#!/bin/bash

# Configuration
APP_NAME="MKS_V2"
FRAMEWORK="net10.0"

echo "========================================="
echo " Building and Packaging $APP_NAME"
echo "========================================="

# 1. Windows x64
echo ""
echo "--> Publishing for Windows (win-x64)..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

echo "--> Zipping Windows release..."
pushd bin/Release/$FRAMEWORK/win-x64/publish > /dev/null
powershell.exe -NoProfile -Command "Compress-Archive -Path * -DestinationPath ../../../../../${APP_NAME}_win-x64.zip -Force"
popd > /dev/null

# 2. macOS arm64 (Apple Silicon)
echo ""
echo "--> Publishing for macOS (osx-arm64)..."
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true

echo "--> Zipping macOS release..."
pushd bin/Release/$FRAMEWORK/osx-arm64/publish > /dev/null
powershell.exe -NoProfile -Command "Compress-Archive -Path * -DestinationPath ../../../../../${APP_NAME}_osx-arm64.zip -Force"
popd > /dev/null

# 3. Linux x64
echo ""
echo "--> Publishing for Linux (linux-x64)..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

echo "--> Zipping Linux release..."
pushd bin/Release/$FRAMEWORK/linux-x64/publish > /dev/null
powershell.exe -NoProfile -Command "Compress-Archive -Path * -DestinationPath ../../../../../${APP_NAME}_linux-x64.zip -Force"
popd > /dev/null

echo ""
echo " All platforms published and zipped successfully!"
echo "Zip files are located in the project root."

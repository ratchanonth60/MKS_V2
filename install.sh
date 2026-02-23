#!/usr/bin/env bash
set -e

# ==========================================
# MKS_V2 Installer Script
# ==========================================

# ⚠️ IMPORTANT: Change this to your actual GitHub Username/Repository
REPO="ratchanonth60/MKS_V2" 
APP_NAME="MKS_V2"
INSTALL_DIR="$HOME/.local/bin"

echo "Looking for latest release of $APP_NAME..."

# Detect OS
OS="$(uname -s)"
case "$OS" in
    Linux*)     PLATFORM="linux" ;;
    Darwin*)    PLATFORM="osx" ;;
    *)          echo "❌ Unsupported OS: $OS" && exit 1 ;;
esac

# Detect Architecture
ARCH="$(uname -m)"
case "$ARCH" in
    x86_64)  ARCH_NAME="x64" ;;
    arm64|aarch64) ARCH_NAME="arm64" ;;
    *) echo "❌ Unsupported architecture: $ARCH" && exit 1 ;;
esac

ASSET_NAME="${APP_NAME}_${PLATFORM}-${ARCH_NAME}.zip"

# Fetch latest release data from GitHub API
RELEASE_JSON=$(curl -s "https://api.github.com/repos/$REPO/releases/latest")

if echo "$RELEASE_JSON" | grep -q '"message": "Not Found"'; then
    echo "❌ Error: Repository '$REPO' or latest release not found."
    echo "Ensure the repository is public and you have published a release on GitHub."
    exit 1
fi

# Parse download URL for the specific platform zip
DOWNLOAD_URL=$(echo "$RELEASE_JSON" | grep "browser_download_url.*$ASSET_NAME" | head -n 1 | cut -d '"' -f 4)

if [ -z "$DOWNLOAD_URL" ]; then
    echo "❌ Error: Could not find asset '$ASSET_NAME' in the latest release."
    exit 1
fi

echo "⬇️  Downloading $ASSET_NAME..."
TMP_DIR=$(mktemp -d)
ZIP_PATH="$TMP_DIR/$ASSET_NAME"

# Download with progress bar (-#)
curl -L -# -o "$ZIP_PATH" "$DOWNLOAD_URL"

echo "📦 Extracting..."
unzip -q -o "$ZIP_PATH" -d "$TMP_DIR"

# Ensure target bin directory exists
mkdir -p "$INSTALL_DIR"

# Move the binary and make it executable
mv "$TMP_DIR/$APP_NAME" "$INSTALL_DIR/$APP_NAME"
chmod +x "$INSTALL_DIR/$APP_NAME"

# Cleanup temporary files
rm -rf "$TMP_DIR"

echo "✅ Successfully installed $APP_NAME to $INSTALL_DIR/$APP_NAME"

# Warn user if the install directory is not in their PATH
if echo "$PATH" | grep -q "$INSTALL_DIR"; then
    echo "🚀 You can now run the app from anywhere by typing: $APP_NAME"
else
    echo "⚠️  Note: '$INSTALL_DIR' is not in your PATH."
    echo "Add the following line to your ~/.bashrc or ~/.zshrc file:"
    echo "export PATH=\"\$PATH:$INSTALL_DIR\""
fi

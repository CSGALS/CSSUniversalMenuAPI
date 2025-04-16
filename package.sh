#!/bin/bash
set -euf -o pipefail

# build CSSUniversalMenuAPI.zip
dst="./artifacts/CSSUniversalMenuAPI"
dst_shared="$dst/addons/counterstrikesharp/shared"
dst_plugins="$dst/addons/counterstrikesharp/plugins"

# package CSSUniversalMenuAPI
mkdir -p "$dst_shared/CSSUniversalMenuAPI"
src="./src/CSSUniversalMenuAPI/bin/Release/net8.0/publish"
cp -r "$src/." "$dst_shared/CSSUniversalMenuAPI/"

# package UniversalMenu.Compat.MenuManagerApi
mkdir -p "$dst_plugins/UniversalMenu.Compat.MenuManagerApi"
src="./src/UniversalMenu.Compat.MenuManagerApi/bin/Release/net8.0/publish"
cp -r "$src/." "$dst_plugins/UniversalMenu.Compat.MenuManagerApi/"

# zip CSSUniversalMenuAPI.zip
pushd "$dst"
7z a ../CSSUniversalMenuAPI.zip ./
popd
rm -rf "$dst"

# build UniversalMenu.Driver.ScreenMenuAPI.zip
dst="./artifacts/UniversalMenu.Driver.ScreenMenuAPI"
dst_shared="$dst/addons/counterstrikesharp/shared"
dst_plugins="$dst/addons/counterstrikesharp/plugins"

# package UniversalMenu.Driver.ScreenMenuAPI
mkdir -p "$dst_plugins/UniversalMenu.Driver.ScreenMenuAPI"
src="./src/UniversalMenu.Driver.ScreenMenuAPI/bin/Release/net8.0/publish"
cp -r "$src/." "$dst_plugins/UniversalMenu.Driver.ScreenMenuAPI/"

# zip UniversalMenu.Driver.ScreenMenuAPI.zip
pushd "$dst"
7z a ../UniversalMenu.Driver.ScreenMenuAPI.zip ./
popd
rm -rf "$dst"

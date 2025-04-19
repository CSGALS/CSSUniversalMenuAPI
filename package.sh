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

# package UniversalMenu.Compat.CSSharp
mkdir -p "$dst_plugins/UniversalMenu.Compat.CSSharp"
src="./src/UniversalMenu.Compat.CSSharp/bin/Release/net8.0/publish"
cp -r "$src/." "$dst_plugins/UniversalMenu.Compat.CSSharp/"
# shared part: allows modified methods to load the 0Harmony.dll dependency
mkdir -p "$dst_shared/0Harmony"
mv "$dst_plugins/UniversalMenu.Compat.CSSharp/0Harmony.dll" "$dst_shared/0Harmony/"

# package UniversalMenu.Compat.ScreenMenuAPI # this isn't implemented yet
#mkdir -p "$dst_plugins/UniversalMenu.Compat.ScreenMenuAPI"
#src="./src/UniversalMenu.Compat.ScreenMenuAPI/bin/Release/net8.0/publish"
#cp -r "$src/." "$dst_shared/UniversalMenu.Compat.ScreenMenuAPI/"

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

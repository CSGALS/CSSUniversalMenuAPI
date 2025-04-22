#!/bin/bash
set -euf -o pipefail

zip_name=""
dst=""
dst_css=""

setup_zip() {
	zip_name="$1"
	dst="./artifacts/$zip_name"
	dst_css="$dst/addons/counterstrikesharp"
}

copy_dir() {
	mkdir -p "$dst_css/$2/"
	cp -r "$1/." "$dst_css/$2/"
}

move_file() {
	mkdir -p "$dst_css/$2/"
	mv "$dst_css/$1" "$dst_css/$2/"
}

commit_zip() {
	pushd "$dst"
	7z a "../$zip_name.zip" ./
	popd
	rm -rf "$dst"
}

setup_zip CSSUniversalMenuAPI
copy_dir src/CSSUniversalMenuAPI/bin/Release/net8.0/publish shared/CSSUniversalMenuAPI
commit_zip

setup_zip UniversalMenu.Compat.CSSharp
copy_dir src/UniversalMenu.Compat.CSSharp/bin/Release/net8.0/publish plugins/UniversalMenu.Compat.CSSharp
# we move this into a shared location so that injected code can find the dll
move_file plugins/UniversalMenu.Compat.CSSharp/0Harmony.dll shared/0Harmony
commit_zip

setup_zip UniversalMenu.Compat.MenuManagerApi
copy_dir src/UniversalMenu.Compat.MenuManagerApi/bin/Release/net8.0/publish plugins/UniversalMenu.Compat.MenuManagerApi
commit_zip

#setup_zip UniversalMenu.Compat.ScreenMenuAPI
#copy_dir src/UniversalMenu.Compat.ScreenMenuAPI/bin/Release/net8.0/publish plugins/UniversalMenu.Compat.ScreenMenuAPI
#commit_zip

setup_zip UniversalMenu.Driver.ScreenMenuAPI
copy_dir src/UniversalMenu.Driver.ScreenMenuAPI/bin/Release/net8.0/publish shared/UniversalMenu.Driver.ScreenMenuAPI
commit_zip

setup_zip UniversalMenu.Driver.MenuManagerApi
copy_dir src/UniversalMenu.Driver.MenuManagerApi/bin/Release/net8.0/publish shared/UniversalMenu.Driver.MenuManagerApi
commit_zip

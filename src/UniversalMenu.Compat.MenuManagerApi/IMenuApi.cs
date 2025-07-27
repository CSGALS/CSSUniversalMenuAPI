using System;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace MenuManager;

public enum MenuType
{
	Default = -1,
	ChatMenu,
	ConsoleMenu,
	CenterMenu,
	ButtonMenu,
	MetamodMenu,
}

// note: the parameter names have been changed to follow C# naming conventions, and added nullbility annotations.
// this is safe as this should only link to already-compiled code (excluding CSSUniversalMenuApi),
// and thus only needs to retain binary compatibility and not source compatibility.
public interface IMenuApi
{
	IMenu GetMenu(string title, Action<CCSPlayerController>? backAction = null, Action<CCSPlayerController>? resetAction = null);
	[Obsolete]
	IMenu NewMenu(string title, Action<CCSPlayerController>? backAction = null);
	IMenu GetMenuForcetype(string title, MenuType type, Action<CCSPlayerController>? backAction = null, Action<CCSPlayerController>? resetAction = null);
	[Obsolete]
	IMenu NewMenuForcetype(string title, MenuType type, Action<CCSPlayerController>? backAction = null);
	void CloseMenu(CCSPlayerController player);
	MenuType GetMenuType(CCSPlayerController player);
	bool HasOpenedMenu(CCSPlayerController player);
}

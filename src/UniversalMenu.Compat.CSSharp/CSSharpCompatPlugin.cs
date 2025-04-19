using System;
using System.Collections.Generic;
using System.Linq;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Menu;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

using HarmonyLib;

using IUniversalMenu = CSSUniversalMenuAPI.IMenu;

namespace UniversalMenu.Compat.CSSharp;

public class CSSharpCompatPlugin : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Compat.CSSharp";
	public override string ModuleVersion => Verlite.Version.Full;

	public override void Load(bool hotReload)
	{
		Harmony = new Harmony("com.universalmenu.compat.cssharp");

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenCenterHtmlMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenCenterHtmlMenu(null!, null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenChatMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenChatMenu(null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenConsoleMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenConsoleMenu(null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.CloseActiveMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_CloseActiveMenu(null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}
	}

	public override void Unload(bool hotReload)
	{
		Harmony?.UnpatchAll();
	}

	private static Harmony? Harmony { get; set; }
	private static Dictionary<nint, IUniversalMenu> ActiveMenus { get; } = new();

	[GameEventHandler(HookMode.Pre)]
	public HookResult OnPlayerDisconnect(EventPlayerDisconnect e, GameEventInfo info)
	{
		if (e.Userid is null)
			return HookResult.Continue;

		ActiveMenus.Remove(e.Userid.Handle);
		return HookResult.Continue;
	}

	public static bool BaseMenu_Open(BasePlugin? plugin, CCSPlayerController player, BaseMenu menu)
	{
		// this API  assumes there may only be 1 menu at a time, so tell the implementation
		// that this menu is expected to be closed
		if (ActiveMenus.TryGetValue(player.Handle, out var activeMenu))
		{
			ActiveMenus.Remove(player.Handle);
			activeMenu.Close();
		}

		var api = CSSUniversalMenuAPI.UniversalMenu.DefaultDriver;

		if (api is null) // fall back to builtin menu
			return true;

		var newMenu = api.CreateMenu(player);
		newMenu.PlayerCanClose = menu.ExitButton;

		var useHtml = false;
		if (newMenu.TryGetExtension<IHtmlSupportMenuExtension>(out var htmlMenu))
			htmlMenu.UseHtml = useHtml = true;

		if (useHtml)
			newMenu.Title = menu.Title;
		else
			newMenu.Title = StripHtml(menu.Title);

		foreach (var item in menu.MenuOptions)
		{
			var newItem = newMenu.CreateItem();
			newItem.Enabled = !item.Disabled;

			if (useHtml)
				newItem.Title = item.Text;
			else
				newItem.Title = StripHtml(item.Text);

			if (item.OnSelect is not null)
				newItem.Selected += (selectedItem) =>
				{
					item.OnSelect(player, item);
					if (menu.PostSelectAction == PostSelectAction.Close)
					{
						if (ActiveMenus.TryGetValue(player.Handle, out var activeMenu) && activeMenu == newMenu)
							ActiveMenus.Remove(player.Handle);
						newMenu.Close();
					}
				};
		}

		ActiveMenus[player.Handle] = newMenu;
		newMenu.Display();
		return false;
	}

	public static bool MenuManager_CloseActiveMenu(CCSPlayerController player)
	{
		if (ActiveMenus.TryGetValue(player.Handle, out var activeMenu))
		{
			ActiveMenus.Remove(player.Handle);
			activeMenu.Close();
		}
		return true;
	}

	public static bool MenuManager_OpenCenterHtmlMenu(BasePlugin plugin, CCSPlayerController player, CenterHtmlMenu menu)
	{
		return BaseMenu_Open(plugin, player, menu);
	}

	public static bool MenuManager_OpenChatMenu(CCSPlayerController player, ChatMenu menu)
	{
		return BaseMenu_Open(null, player, menu);
	}

	public static bool MenuManager_OpenConsoleMenu(CCSPlayerController player, ConsoleMenu menu)
	{
		return BaseMenu_Open(null, player, menu);
	}

	private static readonly HtmlParser HtmlParser = new(new HtmlParserOptions() { IsStrictMode = false });
	private static string StripHtml(string input)
	{
		var doc = HtmlParser.ParseFragment(input, null!);
		return string.Join(string.Empty, doc.Select(x => x.Text()));
	}
}

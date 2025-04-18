using System.Linq;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

using HarmonyLib;

namespace UniversalMenu.Compat.CSSharp;

public static class CSSSharpCompatPluginShared
{
	private static Harmony? Harmony { get; set; }

	public static void Patch()
	{
		Harmony = new Harmony("com.universalmenu.cssharp");

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenCenterHtmlMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenCenterHtmlMenu(null!, null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenChatMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenChatMenu(null!, null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}

		{
			var original = AccessTools.Method(typeof(MenuManager), nameof(MenuManager.OpenConsoleMenu));
			var pre = SymbolExtensions.GetMethodInfo(() => MenuManager_OpenConsoleMenu(null!, null!, null!));
			Harmony.Patch(original, prefix: new HarmonyMethod(pre));
		}
	}

	public static void Unpatch()
	{
		Harmony?.UnpatchAll();
	}

	public static bool BaseMenu_Open(BasePlugin plugin, CCSPlayerController player, BaseMenu menu)
	{
		var api = IMenuAPI.PluginCapability.Get();

		if (api is null) // fall back to builtin menu
			return true;

		var newMenu = api.CreateMenu(player);
		newMenu.PlayerCanClose = menu.ExitButton;

		var useHtml = false;
		if (newMenu is IHtmlSupportMenuExtension htmlMenu)
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
					if (menu.PostSelectAction == PostSelectAction.Close)
						newMenu.Close();
					item.OnSelect(player, item);
				};
		}

		newMenu.Display();
		return false;
	}

	public static bool MenuManager_OpenCenterHtmlMenu(BasePlugin plugin, CCSPlayerController player, CenterHtmlMenu menu)
	{
		return BaseMenu_Open(plugin, player, menu);
	}

	public static bool MenuManager_OpenChatMenu(BasePlugin plugin, CCSPlayerController player, ChatMenu menu)
	{
		return BaseMenu_Open(plugin, player, menu);
	}

	public static bool MenuManager_OpenConsoleMenu(BasePlugin plugin, CCSPlayerController player, ConsoleMenu menu)
	{
		return BaseMenu_Open(plugin, player, menu);
	}

	private static readonly HtmlParser HtmlParser = new(new HtmlParserOptions() { IsStrictMode = false });
	private static string StripHtml(string input)
	{
		var doc = HtmlParser.ParseFragment(input, null!);
		return string.Join(string.Empty, doc.Select(x => x.Text()));
	}
}

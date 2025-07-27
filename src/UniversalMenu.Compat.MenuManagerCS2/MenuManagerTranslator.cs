using System;
using System.Collections.Generic;
using System.Linq;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

using CSSUniversalMenuAPI.Extensions;

using ICssMenu = CounterStrikeSharp.API.Modules.Menu.IMenu;
using IMenuManagerAPI = MenuManager.IMenuApi;
using IUniversalMenu = CSSUniversalMenuAPI.IMenu;
using MenuManagerMenuType = MenuManager.MenuType;

namespace UniversalMenu.Compat.MenuManagerCS2;

public sealed class MenuManagerTranslator : IMenuManagerAPI
{
	public MenuManagerCompat Plugin { get; }
	public MenuManagerTranslator(MenuManagerCompat plugin)
	{
		Plugin = plugin;
	}

	internal class PlayerState
	{
		public MenuInstanceTranslator? ActiveMenu { get; set; }
	}
	internal SortedDictionary<ulong, PlayerState> PlayerStates = new();

	internal PlayerState GetMenuState(CCSPlayerController player)
	{
		if (!PlayerStates.TryGetValue(player.SteamID, out var state))
			PlayerStates[player.SteamID] = state = new PlayerState();
		return state;
	}

	void IMenuManagerAPI.CloseMenu(CCSPlayerController player)
	{
		if (!PlayerStates.TryGetValue(player.SteamID, out var state))
			return;
		if (state.ActiveMenu is null)
			return;
		state.ActiveMenu.Close(player);
		state.ActiveMenu = null;
	}

	ICssMenu IMenuManagerAPI.GetMenu(string title, Action<CCSPlayerController>? backAction, Action<CCSPlayerController>? resetAction)
	{
		return new MenuInstanceTranslator(title, this, backAction, resetAction, MenuManagerMenuType.Default);
	}

	ICssMenu IMenuManagerAPI.GetMenuForcetype(string title, MenuManagerMenuType type, Action<CCSPlayerController>? backAction, Action<CCSPlayerController>? resetAction)
	{
		return new MenuInstanceTranslator(title, this, backAction, resetAction, type);
	}

	MenuManagerMenuType IMenuManagerAPI.GetMenuType(CCSPlayerController player)
	{
		return MenuManagerMenuType.Default;
	}

	bool IMenuManagerAPI.HasOpenedMenu(CCSPlayerController player)
	{
		return CSSUniversalMenuAPI.UniversalMenu.DefaultDriver?.IsMenuOpen(player) ?? false;
	}

	ICssMenu IMenuManagerAPI.NewMenu(string title, Action<CCSPlayerController>? backAction)
	{
		return new MenuInstanceTranslator(title, this, backAction, null, MenuManagerMenuType.Default);
	}

	ICssMenu IMenuManagerAPI.NewMenuForcetype(string title, MenuManagerMenuType type, Action<CCSPlayerController>? backAction)
	{
		return new MenuInstanceTranslator(title, this, backAction, null, type);
	}
}

internal sealed class MenuInstanceTranslator : ICssMenu
{
	public string Title { get; set; }
	public MenuManagerTranslator Translator { get; }
	public bool ExitButton { get; set; }

	public List<ChatMenuOption> MenuOptions { get; } = new();
	public Action<CCSPlayerController>? BackAction { get; }
	public Action<CCSPlayerController>? ResetAction { get; }
	public MenuManagerMenuType MenuType { get; set; }
	public PostSelectAction PostSelectAction { get; set; } = PostSelectAction.Nothing;

	public MenuInstanceTranslator(
		string title,
		MenuManagerTranslator translator,
		Action<CCSPlayerController>? backAction,
		Action<CCSPlayerController>? resetAction,
		MenuManagerMenuType menuType)
	{
		Title = title;
		Translator = translator;
		BackAction = backAction;
		ResetAction = resetAction;
		MenuType = menuType;
	}

	ChatMenuOption ICssMenu.AddMenuOption(string display, Action<CCSPlayerController, ChatMenuOption> onSelect, bool disabled)
	{
		var ret = new ChatMenuOption(display, disabled, onSelect);
		MenuOptions.Add(ret);
		return ret;
	}

	private Dictionary<ulong, IUniversalMenu> PlayerMenus = new();
	public void Open(CCSPlayerController player)
	{
		CloseActiveMenu(player);
		var menuState = Translator.GetMenuState(player);
		menuState.ActiveMenu = this;

		if (PlayerMenus.TryGetValue(player.SteamID, out var menu))
		{
			menu.Display();
			return;
		}

		menu = PlayerMenus[player.SteamID] = CSSUniversalMenuAPI.UniversalMenu.CreateMenu(player);
		menu.Title = Title;
		menu.PlayerCanClose = true;// ExitButton;

		var usingHtml = false;
		if (menu.TryGetExtension<IHtmlSupportMenuExtension>(out var htmlMenu))
			htmlMenu.UseHtml = usingHtml = true;

		if (BackAction is not null && menu.TryGetExtension<INavigateBackMenuExtension>(out var backableMenu))
			backableMenu.NavigateBack = (_) => BackAction.Invoke(player);

		foreach (var option in MenuOptions)
		{
			var item = menu.CreateItem();
			item.Enabled = !option.Disabled;

			if (usingHtml)
				item.Title = option.Text;
			else
			{
				var doc = HtmlParser.ParseFragment(option.Text, null!);
				item.Title = string.Join(string.Empty, doc.Select(x => x.Text()));
			}

			item.Context = option;
			if (option.OnSelect is not null)
				item.Selected += (menuItem) =>
				{
					var menuOption = menuItem.Context as ChatMenuOption;
					menuOption!.OnSelect(menuItem.Player, option);

					switch (PostSelectAction)
					{
						case PostSelectAction.Reset:
							ResetAction?.Invoke(menuItem.Player);
							break;
						case PostSelectAction.Nothing:
							break;
						default:
						case PostSelectAction.Close:
							menu.Close();
							if (menuState.ActiveMenu == this)
								menuState.ActiveMenu = null;
							break;
					}
				};
		}

		menu.Display();
	}
	private readonly HtmlParser HtmlParser = new(new HtmlParserOptions() { IsStrictMode = false });

	void ICssMenu.OpenToAll()
	{
		foreach (var player in Utilities.GetPlayers())
			Open(player);
	}

	private void CloseActiveMenu(CCSPlayerController player)
	{
		var menuState = Translator.GetMenuState(player);
		if (menuState.ActiveMenu is null)
			return;
		if (!menuState.ActiveMenu.PlayerMenus.TryGetValue(player.SteamID, out var universalMenu))
			return;

		menuState.ActiveMenu = null;
		universalMenu.Close();
	}

	internal void Close(CCSPlayerController player)
	{
		if (!PlayerMenus.TryGetValue(player.SteamID, out var menu))
			return;
		menu.Close();
	}
}

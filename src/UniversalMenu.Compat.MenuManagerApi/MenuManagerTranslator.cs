using System;
using System.Collections.Generic;
using System.Linq;

using AngleSharp.Dom;
using AngleSharp.Html.Parser;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;

using CSSUniversalMenuAPI;

using ICssMenu = CounterStrikeSharp.API.Modules.Menu.IMenu;
using IMenuManagerAPI = MenuManager.IMenuApi;
using IUniversalMenu = CSSUniversalMenuAPI.IMenu;
using MenuManagerMenuType = MenuManager.MenuType;

namespace UniversalMenu.Compat.MenuManagerApi;

public sealed class MenuManagerTranslator : IMenuManagerAPI
{
	public MenuManagerCompat Plugin { get; }
	public IMenuAPI UniversalAPI { get; }
	public MenuManagerTranslator(MenuManagerCompat plugin, IMenuAPI universalAPI)
	{
		Plugin = plugin;
		UniversalAPI = universalAPI;
	}

	internal class PlayerState
	{
		public MenuInstanceTranslator? ActiveMenu { get; set; }
		public IUniversalMenu? ExecutingMenu { get; set; }
	}
	internal SortedDictionary<ulong, PlayerState> PlayerStates = new();

	internal PlayerState GetMenuState(CCSPlayerController player)
	{
		if (!PlayerStates.TryGetValue(player.SteamID, out var state))
			PlayerStates[player.SteamID] = state = new PlayerState();
		return state;
	}

	internal MenuInstanceTranslator? CurrentParentMenu { get; set; }

	void IMenuManagerAPI.CloseMenu(CCSPlayerController player)
	{
		if (!PlayerStates.TryGetValue(player.SteamID, out var state))
			return;
		if (state.ActiveMenu is null)
			return;
		state.ActiveMenu.Close(player);
	}

	ICssMenu IMenuManagerAPI.GetMenu(string title, Action<CCSPlayerController> back_action, Action<CCSPlayerController> reset_action)
	{
		return new MenuInstanceTranslator(title, this, back_action, reset_action, MenuManagerMenuType.Default);
	}

	ICssMenu IMenuManagerAPI.GetMenuForcetype(string title, MenuManagerMenuType type, Action<CCSPlayerController> back_action, Action<CCSPlayerController> reset_action)
	{
		return new MenuInstanceTranslator(title, this, back_action, reset_action, type);
	}

	MenuManagerMenuType IMenuManagerAPI.GetMenuType(CCSPlayerController player)
	{
		// TODO: what is this function meant to get?
		//var state = GetMenuState(player);
		//if (state.ActiveMenu is not null)
		//	return state.ActiveMenu.MenuType;
		return MenuManagerMenuType.Default;
	}

	bool IMenuManagerAPI.HasOpenedMenu(CCSPlayerController player)
	{
		// does this return if a player already has a menu open, or what?
		return UniversalAPI.IsMenuOpen(player);
		//return GetMenuState(player).ActiveMenu is not null;
	}

	ICssMenu IMenuManagerAPI.NewMenu(string title, Action<CCSPlayerController> back_action)
	{
		return new MenuInstanceTranslator(title, this, back_action, null, MenuManagerMenuType.Default);
	}

	ICssMenu IMenuManagerAPI.NewMenuForcetype(string title, MenuManagerMenuType type, Action<CCSPlayerController> back_action)
	{
		return new MenuInstanceTranslator(title, this, back_action, null, type);
	}
}

internal sealed class MenuInstanceTranslator : ICssMenu
{
	public string Title { get; set; }
	public MenuManagerTranslator Translator { get; }
	public IMenuAPI UniversalAPI => Translator.UniversalAPI;
	public bool ExitButton { get; set; }

	public List<ChatMenuOption> MenuOptions { get; } = new();
	public Action<CCSPlayerController>? BackAction { get; }
	public Action<CCSPlayerController>? ResetAction { get; }
	public MenuManagerMenuType MenuType { get; set; }

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
	void ICssMenu.Open(CCSPlayerController player)
	{
		if (PlayerMenus.TryGetValue(player.SteamID, out var menu))
		{
			menu.Display();
			return;
		}

		var menuState = Translator.GetMenuState(player);

		// attempt to detect "reopening" the same menu
		var parent = menuState.ExecutingMenu;
		if (parent is not null && parent.Title == Title)
			parent = parent.Parent;

		if (parent is null) // TODO: add cancel token
			menu = PlayerMenus[player.SteamID] = UniversalAPI.CreateMenu(player);
		else
			menu = PlayerMenus[player.SteamID] = UniversalAPI.CreateMenu(parent);

		menu.Title = Title;
		menu.PlayerCanClose = true;// ExitButton;

		bool usingHtml = false;
		if (menu is CSSUniversalMenuAPI.Extensions.IHtmlSupportMenuExtension htmlMenu)
			htmlMenu.UseHtml = usingHtml = true;

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
					// MenuManager menus expect the menu to close on selection
					menuState.ExecutingMenu = menu;
					{
						menu.Close();
						menuOption!.OnSelect(menuItem.Player, option);
						ResetAction?.Invoke(menuItem.Player);
					}
					menuState.ExecutingMenu = null;
				};
		}

		menu.Display();
	}
	private readonly HtmlParser HtmlParser = new(new HtmlParserOptions() { IsStrictMode = false });

	void ICssMenu.OpenToAll()
	{
		throw new NotSupportedException("IMenu.OpenToAll() is not supported");
	}

	internal void Close(CCSPlayerController player)
	{
		if (!PlayerMenus.TryGetValue(player.SteamID, out var menu))
			return;
		menu.Close();
		// TODO: do we invoke?
		//BackAction?.Invoke(player);
	}
}

using System;
using System.Threading;
using System.Collections.Generic;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

using IMenuManagerApi = MenuManager.IMenuApi;
using IMenuManagerMenu = CounterStrikeSharp.API.Modules.Menu.IMenu;
using IMenu = CSSUniversalMenuAPI.IMenu;

namespace UniversalMenu.Driver.MenuManagerApi;

internal class PlayerMenuState
{
	public Menu? ActiveMenu { get; set; }
}

public sealed class MenuManagerApiDriver : IMenuAPI
{
	internal MenuManagerApiDriverPlugin Plugin { get; }
	internal IMenuManagerApi? MenuManagerApi { get; set; }

	public MenuManagerApiDriver(MenuManagerApiDriverPlugin plugin)
	{
		Plugin = plugin;
	}

	private Dictionary<ulong, PlayerMenuState> MenuStates = new();
	internal PlayerMenuState GetMenuState(CCSPlayerController player)
	{
		if (!MenuStates.TryGetValue(player.SteamID, out var menuState))
			MenuStates.Add(player.SteamID, menuState = new());
		return menuState;
	}
	internal void PlayerDisconnected(ulong steamId)
	{
		MenuStates.Remove(steamId);
	}

	IMenu IMenuAPI.CreateMenu(CCSPlayerController player, CancellationToken ct)
	{
		return new Menu()
		{
			MenuAPI = this,
			Player = player,
			Parent = null,
		};
	}

	IMenu IMenuAPI.CreateMenu(IMenu parent, CancellationToken ct)
	{
		if (parent is not Menu parentMenu)
			throw new ArgumentException("Menu given belongs to another menu implementation", nameof(parent));

		return new Menu()
		{
			MenuAPI = this,
			Player = parent.Player,
			Parent = parentMenu,
		};
	}

	bool IMenuAPI.IsExtensionSupported(Type extension)
	{
		if (extension == typeof(INavigateBackMenuExtension))
			return true;
		return false;
	}

	bool IMenuAPI.IsMenuOpen(CCSPlayerController player)
	{
		return MenuManagerApi?.HasOpenedMenu(player) ?? false;
	}
}

internal class Menu : IMenu, INavigateBackMenuExtension
{
	public required MenuManagerApiDriver MenuAPI { get; set; }

	// IMenu
	public required Menu? Parent { get; init; }
	IMenu? IMenu.Parent => Parent;
	public required CCSPlayerController Player { get; init; }
	public bool PlayerCanClose { get; set; } = true;
	public bool IsActive { get; private set; }
	public string Title { get; set; } = string.Empty;

	// ScreenMenuAPI
	internal IMenuManagerMenu? TheMenu { get; set; }
	private List<MenuItem> MenuItems { get; } = new();
	public Action<IMenu>? NavigateBack { get; set; }

	void IMenu.Close()
	{
		if (!IsActive)
			return;
		IsActive = false;

		var state = MenuAPI.GetMenuState(Player);
		if (this == state.ActiveMenu)
		{
			MenuAPI.MenuManagerApi.CloseMenu(Player);
			state.ActiveMenu = null;
		}
	}

	IMenuItem IMenu.CreateItem()
	{
		var ret = new MenuItem() { Menu = this };
		MenuItems.Add(ret);
		return ret;
	}

	void IMenu.Display()
	{
		if (MenuAPI.MenuManagerApi is null)
			throw new Exception("MenuManagerApi not found");

		IsActive = true;
		if (TheMenu is not null)
		{
			TheMenu.Open(Player);
			return;
		}

		Action<CCSPlayerController>? navBack = null;
		if (NavigateBack is not null)
			navBack = (player) => NavigateBack(this);

		TheMenu = MenuAPI.MenuManagerApi.GetMenu(Title, backAction: navBack!);

		foreach (var item in MenuItems)
			TheMenu.AddMenuOption(item.Title, (player, option) => item.RaiseSelected(), !item.Enabled);

		TheMenu.Open(Player);
	}
}

internal class MenuItem : IMenuItem
{
	public required IMenu Menu { get; init; }
	public string Title { get; set; } = string.Empty;
	public bool Enabled { get; set; } = true;
	public object? Context { get; set; }

	public event ItemSelectedAction? Selected;
	internal void RaiseSelected()
	{
		Selected?.Invoke(this);
	}
}

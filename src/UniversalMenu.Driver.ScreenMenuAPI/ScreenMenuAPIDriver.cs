using System;
using System.Threading;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;

using ScreenMenu = CS2ScreenMenuAPI.Menu;
using ScreenMenuAPI = CS2ScreenMenuAPI.MenuAPI;
using ScreenMenuPostSelect = CS2ScreenMenuAPI.PostSelect;
using ScreenMenuType = CS2ScreenMenuAPI.MenuType;
using System.Collections.Generic;

namespace UniversalMenu.ScreenMenuAPIAdapter;

public class PlayerMenuState
{
	public bool UsingKeyBinds { get; set; }
}

public sealed class ScreenMenuApiDriver : IMenuAPI
{
	public ScreenMenuAPIDriverPlugin Plugin { get; }

	public ScreenMenuApiDriver(ScreenMenuAPIDriverPlugin plugin)
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
		return false;
	}

	bool IMenuAPI.IsMenuOpen(CCSPlayerController player)
	{
		return ScreenMenuAPI.GetActiveMenu(player) is not null;
	}
}

internal class Menu : IMenu
{
	public required ScreenMenuApiDriver MenuAPI { get; set; }

	// IMenu
	public required Menu? Parent { get; init; }
	IMenu? IMenu.Parent => Parent;
	public required CCSPlayerController Player { get; init; }
	public bool PlayerCanClose { get; set; } = true;

	public bool _IsOpen = false;
	public bool IsActive
	{
		get
		{
			if (TheMenu is null)
				return false;
			if (!_IsOpen)
				return false;
			return ScreenMenuAPI.GetActiveMenu(Player) == TheMenu;
		}
	}
	public string Title { get; set; } = string.Empty;

	// ScreenMenuAPI
	internal ScreenMenu? TheMenu { get; set; }
	private List<MenuItem> MenuItems { get; } = new();

	void IMenu.Close()
	{
		_IsOpen = false;
		if (TheMenu is null)
			return;
		TheMenu.Close(Player);
	}

	IMenuItem IMenu.CreateItem()
	{
		var ret = new MenuItem() { Menu = this };
		MenuItems.Add(ret);
		return ret;
	}

	void IMenu.Display()
	{
		_IsOpen = true;
		if (TheMenu is not null)
		{
			ScreenMenuAPI.SetActiveMenu(Player, TheMenu);
			TheMenu.Display();
			return;
		}

		var menuState = MenuAPI.GetMenuState(Player);

		TheMenu = new ScreenMenu(Player, MenuAPI.Plugin)
		{
			Title = Title,
			PostSelect = ScreenMenuPostSelect.Nothing,
			PrevMenu = Parent?.TheMenu,
			IsSubMenu = Parent?.TheMenu is not null,
			MenuType = menuState.UsingKeyBinds ? ScreenMenuType.KeyPress : ScreenMenuType.Both,
			HasExitButon = PlayerCanClose,
		};

		foreach (var item in MenuItems)
			TheMenu.AddItem(item.Title, (player, option) => item.RaiseSelected(), !item.Enabled);

		TheMenu.Display();
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

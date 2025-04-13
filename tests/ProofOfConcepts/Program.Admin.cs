using System.Collections.Generic;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

namespace ProofOfConcepts;

public static partial class Program
{
	public static void AdminCommand(CCSPlayerController player)
	{
		var menu = API.CreateMenu(player);

		menu.Title = "Admin";
		if (menu is IMenuPriorityExtension menuPriorityExt)
			menuPriorityExt.Priority = 100.0; // higher priority, trumps other default menus

		{
			var item = menu.CreateItem();
			item.Title = "Players Manage";
			item.Selected += PlayersManage_Selected;
		}

		{
			var item = menu.CreateItem();
			item.Title = "Server Manage";
			item.Selected += ServersManage_Selected;
		}

		{
			var item = menu.CreateItem();
			item.Title = "Fun Commands";
			item.Selected += FunCommands_Selected;
		}

		{
			var item = menu.CreateItem();
			item.Title = "Admins Manage";
			item.Selected += AdminsManage_Selected;
		}

		menu.Display();
	}

	private static void PlayersManage_Selected(IMenuItem menuItem)
	{
		menuItem.Menu.Exit();
	}
	private static void ServersManage_Selected(IMenuItem menuItem)
	{
		menuItem.Menu.Exit();
	}

	private static void FunCommands_Selected(IMenuItem menuItem)
	{
		var menu = API.CreateMenu(menuItem.Menu);
		menu.Title = "Fun Commands";

		{
			var item = menu.CreateItem();
			item.Title = "God Mode";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "No Clip";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Respawn";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Give Weapon";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Give Weapons";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Freeze";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Set HP";
			item.Selected += FunCommand_Selected;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Set Speed";
			item.Enabled = false;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Set Gravity";
			item.Selected += FunCommand_Selected;
			item.Enabled = false;
		}
		{
			var item = menu.CreateItem();
			item.Title = "Set Money";
			item.Selected += FunCommand_Selected;
			item.Enabled = false;
		}

		menu.Display();
	}

	private static void FunCommand_Selected(IMenuItem menuItem)
	{
		var menu = API.CreateMenu(menuItem.Menu);
		menu.Title = menuItem.Title;

		foreach (var player in Players)
		{
			var item = menu.CreateItem();
			item.Title = player;
			item.Selected += static (menuItem) => menuItem.Menu.Exit();
		}

		menu.Display();
	}

	private static void AdminsManage_Selected(IMenuItem menuItem)
	{
		menuItem.Menu.Exit();
	}

	private static readonly string[] Players = [ "Me", "You", "Them", "Nobody", "Everybody" ];
}

using System.Collections;
using System.Collections.Generic;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

namespace ProofOfConcepts;

public static partial class Program
{
	public static void AdminCommand(CCSPlayerController player)
	{
		API.CreateAndShowMenu(player, new("Admin")
		{
			new("Players Manage")
			{
				new("Slap", players: Players)
				{
					new("0hp", exit: false),
					new("1hp", exit: false),
					new("5hp", exit: false),
					new("10hp", exit: false),
					new("25hp", exit: false),
					new("50hp", exit: false),
					new("100hp", exit: false),
				},
			},
			new("Server Manage"),
			new("Fun Commands")
			{
				new("God Mode", players: Players),
				new("No Clip", players: Players),
				new("Respawn", players: Players),
				new("Give Weapon", players: Players)
				{
					new("AK47"),
					new("M4"),
					new("Deagle"),
				},
				new("Give Weapons", players: Players),
				new("Freeze", players: Players),
				new("Set HP", players: Players)
				{
					new("1"),
					new("5"),
					new("10"),
					new("25"),
					new("50"),
					new("100"),
					new("200"),
					new("500"),
					new("1000"),
				},
				new("Set Speed", players: Players)
				{
					new("10%"),
					new("50%"),
					new("75%"),
					new("100%"),
					new("150%"),
					new("200%"),
					new("400%"),
				},
				new("Set Gravity", players: Players)
				{
					new("10%"),
					new("50%"),
					new("75%"),
					new("100%"),
					new("150%"),
					new("200%"),
				},
				new("Set Money", players: Players)
				{
					new("0"),
					new("800"),
					new("2000"),
					new("5000"),
					new("16000"),
				},
			},
			new("Admins Manage"),
		});
	}


	private static readonly string[] Players = [ "Me", "You", "Them", "Nobody", "Everybody" ];
}

file static class Extensions
{
	public static void CreateAndShowMenu(this IMenuAPI api, CCSPlayerController player, MenuItems items)
	{
		api.CreateAndShowMenuInternal(player, items, false);
	}

	private static void CreateAndShowMenuInternal(this IMenuAPI api, CCSPlayerController player, MenuItems items, bool donePlayers)
	{
		var menu = api.CreateMenu(player);
		menu.Title = items.Title;
		if (menu.TryGetExtension<IMenuPriorityExtension>(out var priorityExtension))
			priorityExtension.Priority = 100.0;
		menu.CreateItemsInternal(api, items, donePlayers);
		menu.Display();
	}
	private static void CreateAndShowMenuInternal(this IMenuAPI api, IMenu parent, MenuItems items, bool donePlayers)
	{
		var menu = api.CreateMenu(parent);
		menu.Title = items.Title;
		if (menu.TryGetExtension<IMenuPriorityExtension>(out var priorityExtension))
			priorityExtension.Priority = 100.0;
		menu.CreateItemsInternal(api, items, donePlayers);
		menu.Display();
	}

	private static void CreateItemsInternal(this IMenu menu, IMenuAPI api, MenuItems items, bool donePlayers)
	{
		if (items.Players is not null && !donePlayers)
		{
			foreach (var player in items.Players)
			{
				var item = menu.CreateItem();
				item.Title = player;

				if (items.Items.Count == 0)
				{
					if (items.Exit)
						item.Selected += (menuItem) => menuItem.Menu.Exit();
				}
				else
					item.Selected += (menuItem) => CreateAndShowMenuInternal(api, menuItem.Menu, items, true);
			}
			return;
		}

		foreach (var childItem in items.Items)
		{
			var item = menu.CreateItem();
			item.Title = childItem.Title;

			if (childItem.Players is null && childItem.Items.Count == 0)
			{
				if (childItem.Exit)
					item.Selected += (menuItem) => menuItem.Menu.Exit();
			}
			else
				item.Selected += (menuItem) => CreateAndShowMenuInternal(api, menuItem.Menu, childItem, false);
		}
	}

	public class MenuItems : ICollection<MenuItems>
	{
		public string Title { get; }
		public bool Exit { get; }
		public ItemSelectedAction? Action { get; }
		public List<MenuItems> Items { get; } = new List<MenuItems>();
		public IReadOnlyList<string>? Players { get; }

		public MenuItems(string title, bool exit = true, ItemSelectedAction? action = null, IReadOnlyList<string>? players = null)
		{
			Title = title;
			Exit = exit;
			Action = action;
			Players = players;
		}


		int ICollection<MenuItems>.Count => Items.Count;
		public bool IsReadOnly => false;
		public void Add(MenuItems item) => Items.Add(item);
		public void Clear() => Items.Clear();
		public bool Contains(MenuItems item) => Items.Contains(item);
		public void CopyTo(MenuItems[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
		public IEnumerator<MenuItems> GetEnumerator() => Items.GetEnumerator();
		public bool Remove(MenuItems item) => Items.Remove(item);
		IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
	}
}

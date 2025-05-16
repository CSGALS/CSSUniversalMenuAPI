using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

namespace ProofOfConcepts;

/// <summary>
/// This is a sample implementation using the console to act as a proof of concept or reference.
/// Mirrors SourceMod menus
/// </summary>
internal class NumberKeysMenuAPI : ISampleMenu
{
	internal Dictionary<CCSPlayerController, NumbersPlayerMenuState> PlayerMenuStates { get; } = new();

	IMenu IMenuAPI.CreateMenu(CCSPlayerController player, CancellationToken ct)
	{
		if (!PlayerMenuStates.TryGetValue(player, out var menuState))
			menuState = PlayerMenuStates[player] = new NumbersPlayerMenuState();

		return new NumberKeysMenu()
		{
			MenuState = menuState,
			Parents = [],
			Player = player,
			Ct = ct,
		};
	}

	IMenu IMenuAPI.CreateMenu(IMenu parent, CancellationToken ct)
	{
		if (parent is not NumberKeysMenu parentNumberMenu)
			throw new ArgumentException("Mismatched menu type", nameof(parent));

		if (!PlayerMenuStates.TryGetValue(parent.Player, out var menuState))
			menuState = PlayerMenuStates[parent.Player] = new NumbersPlayerMenuState();

		CancellationToken linkedToken;
		if (!ct.CanBeCanceled)
			linkedToken = parentNumberMenu.Ct;
		else if (!parentNumberMenu.Ct.CanBeCanceled)
			linkedToken = ct;
		else
			linkedToken = CancellationTokenSource.CreateLinkedTokenSource(ct, parentNumberMenu.Ct).Token;

		var parents = new List<NumberKeysMenu>(parentNumberMenu.Parents)
		{
			parentNumberMenu
		};

		return new NumberKeysMenu()
		{
			MenuState = menuState,
			Parents = parents,
			Player = parent.Player,
			Ct = linkedToken,
		};
	}

	bool IMenuAPI.IsExtensionSupported(Type extension)
	{
		if (extension == typeof(IMenuPriorityExtension))
			return true;
		if (extension == typeof(INavigateBackMenuExtension))
			return true;
		if (extension == typeof(IMenuItemSubtitleExtension))
			return true;
		return false;
	}

	bool IMenuAPI.IsMenuOpen(CCSPlayerController player)
	{
		if (!PlayerMenuStates.TryGetValue(player, out var menuState))
			return false;
		return menuState.FocusStack.Count > 0;
	}

	private const int ItemsPerPage = 7;
	void ISampleMenu.DrawMenus(CCSPlayerController player)
	{
		int linesWrote = 0;
		void writeLine(string line = "", bool background = false)
		{
			if (background)
				Console.ForegroundColor = ConsoleColor.Gray;
			else
				Console.ForegroundColor = ConsoleColor.DarkYellow;

			Console.WriteLine(line);
			Console.ResetColor();
			linesWrote++;
		}

		writeLine();
		writeLine();
		writeLine();

		if (PlayerMenuStates.TryGetValue(player, out var menuState))
		{
			if (menuState.FocusStack.Count > 0)
			{
				var focusedMenu = menuState.FocusStack[0];

				writeLine($"{focusedMenu.Title}:", background: true);

				var itemsStart = focusedMenu.CurrentPage * ItemsPerPage;
				var itemsInPage = Math.Min(focusedMenu.Items.Count, itemsStart + ItemsPerPage) - itemsStart;

				for (int i = 0; i < itemsInPage; i++)
				{
					var item = focusedMenu.Items[itemsStart + i];
					writeLine($"{i + 1}. {item.Title}", background: !item.Enabled);
					if (item.Subtitle is not null)
						writeLine($"  {item.Subtitle}", background: true);
				}

				bool showExitButton = focusedMenu.Parents.Where(x => !x.PlayerCanClose).Any() == false;
				bool showBackButton = focusedMenu switch
				{
					{ CurrentPage: not 0 } => false,
					{ NavigateBack: not null } => true,
					_ => showExitButton switch
					{
						true => focusedMenu.PlayerCanClose && focusedMenu.Parent is not null,
						false => focusedMenu.PlayerCanClose,
					},
				};
				bool showPrevButton = itemsStart > 0;
				bool showNextButton = itemsStart + itemsInPage < focusedMenu.Items.Count;
				bool showNavigation = showBackButton || showPrevButton || showNextButton;

				if (showNavigation || showExitButton)
				{
					int maxItemsPerPage = Math.Min(ItemsPerPage, focusedMenu.Items.Count);
					int blankLines = maxItemsPerPage - itemsInPage + 1;

					for (int i = 0; i < blankLines; i++)
						writeLine();

					if (showNavigation)
					{
						if (showPrevButton)
							writeLine("8. Previous");
						else if (showBackButton)
							writeLine("8. Back");
						else
							writeLine();

						if (showNextButton)
							writeLine("9. Next");
						else
							writeLine();
					}

					if (showExitButton)
						writeLine("0. Exit", background: true);
				}
			}
		}

		Console.ResetColor();
		for (int i = linesWrote; i < 18; i++)
			Console.WriteLine();
	}

	void ISampleMenu.HandleKey(CCSPlayerController player, ConsoleKeyInfo key)
	{
		if (!PlayerMenuStates.TryGetValue(player, out var menuState))
			return;
		if (menuState.FocusStack.Count <= 0)
			return;

		var focusedMenu = menuState.FocusStack[0];

		var itemsStart = focusedMenu.CurrentPage * ItemsPerPage;
		var itemsInPage = Math.Min(focusedMenu.Items.Count, itemsStart + ItemsPerPage) - itemsStart;

		bool showExitButton = focusedMenu.Parents.Where(x => !x.PlayerCanClose).Any() == false;
		bool showBackButton = focusedMenu switch
		{
			{ CurrentPage: not 0 } => false,
			{ NavigateBack: not null } => true,
			_ => showExitButton switch
			{
				true => focusedMenu.PlayerCanClose && focusedMenu.Parent is not null,
				false => focusedMenu.PlayerCanClose,
			},
		};
		bool showPrevButton = itemsStart > 0;
		bool showNextButton = itemsStart + itemsInPage < focusedMenu.Items.Count;
		bool showNavigation = showBackButton || showPrevButton || showNextButton;

		switch (key.Key)
		{
			case >= ConsoleKey.D1 and <= ConsoleKey.D7:
				int index = key.Key - ConsoleKey.D1;
				if (index < focusedMenu.Items.Count)
					focusedMenu.Items[itemsStart + index].RaiseSelected();
				break;
			case ConsoleKey.D8:
				if (showPrevButton)
					focusedMenu.CurrentPage--;
				else if (showBackButton)
				{
					if (focusedMenu.NavigateBack is not null)
						focusedMenu.NavigateBack(focusedMenu);
					else
						focusedMenu.Close();
				}
				break;
			case ConsoleKey.D9:
				if (showNextButton)
					focusedMenu.CurrentPage++;
				break;
			case ConsoleKey.D0:
				if (showExitButton)
					(focusedMenu as IMenu).Exit();
				break;
			default:
				break;
		}
	}
}

internal class NumbersPlayerMenuState
{
	public List<NumberKeysMenu> FocusStack { get; } = new();

	public void SortPriorities()
	{
		FocusStack.Sort((left, right) =>
		{
			int ret;
			int parentDepth = Math.Min(left.Parents.Count, right.Parents.Count);
			for (int i = 0; i < parentDepth; i++)
			{
				ret = right.Parents[i].Priority.CompareTo(left.Parents[i].Priority);
				if (ret != 0)
					return ret;
			}
			ret = right.Parents.Count.CompareTo(left.Parents.Count);
			if (ret != 0)
				return ret;
			ret = right.Priority.CompareTo(left.Priority); // highest priority comes first
			if (ret != 0)
				return ret;
			return right.OpenedAt.CompareTo(left.OpenedAt); // ok, select the newest menu first
		});
	}
}

internal class NumberKeysMenu : IMenu, IMenuPriorityExtension, INavigateBackMenuExtension
{
	public required NumbersPlayerMenuState MenuState { get; init; }
	public required List<NumberKeysMenu> Parents { get; init; }
	public required CancellationToken Ct { get; init; }
	public CancellationTokenRegistration Ctr { get; set; }
	public List<NumbersMenuItem> Items { get; } = [];
	public int CurrentPage { get; set; }
	public NumberKeysMenu? Parent => Parents.Count > 0 ? Parents[^1] : null;
	public DateTime OpenedAt { get; set; }

	// IMenu
	IMenu? IMenu.Parent => Parent;
	public required CCSPlayerController Player { get; internal init; }
	public bool IsActive => MenuState.FocusStack.Contains(this);
	public string Title { get; set; } = string.Empty;
	public bool PlayerCanClose { get; set; } = true;

	public IMenuItem CreateItem()
	{
		var ret = new NumbersMenuItem() { Menu = this };
		Items.Add(ret);
		return ret;
	}

	public void Display()
	{
		if (IsActive)
			return;
		OpenedAt = DateTime.UtcNow;
		MenuState.FocusStack.Add(this);
		MenuState.SortPriorities();
		if (Ct.CanBeCanceled)
			Ctr = Ct.Register(() => Close());
	}
	public void Close()
	{
		Ctr.Unregister();
		MenuState.FocusStack.Remove(this);
	}

	// IMenuPriorityExtension
	public double _Priority = 0.0;
	public double Priority
	{
		get => _Priority;
		set
		{
			if (value == _Priority)
				return;
			_Priority = value;
			MenuState.SortPriorities();
		}
	}
	public bool IsFocused => MenuState.FocusStack.Count > 0 && MenuState.FocusStack[0] == this;

	// INavigateBackMenuExtension
	public Action<IMenu>? NavigateBack { get; set; }
}

internal class NumbersMenuItem : IMenuItem, IMenuItemSubtitleExtension
{
	public void RaiseSelected()
	{
		Selected?.Invoke(this);
	}

	// IMenuItem
	public required IMenu Menu { get; init; }
	public string Title { get; set; } = string.Empty;
	public bool Enabled { get; set; } = true;
	public object? Context { get; set; }

	public event ItemSelectedAction? Selected;

	// IMenuItemSubtitleExtension
	public string? Subtitle { get; set; } = null;
}

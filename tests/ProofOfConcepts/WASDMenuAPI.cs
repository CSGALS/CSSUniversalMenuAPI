using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

namespace ProofOfConcepts;

/// <summary>
/// This is a sample implementation mirroring that of CenterHtmlMenu and the like
/// </summary>
internal class WASDMenuAPI : ISampleMenu
{
	internal Dictionary<CCSPlayerController, WASDPlayerMenuState> PlayerMenuStates { get; } = new();

	IMenu IMenuAPI.CreateMenu(CCSPlayerController player, CancellationToken ct)
	{
		if (!PlayerMenuStates.TryGetValue(player, out var menuState))
			menuState = PlayerMenuStates[player] = new WASDPlayerMenuState();

		return new WASDMenu()
		{
			MenuState = menuState,
			Parents = [],
			Player = player,
			Ct = ct,
		};
	}

	IMenu IMenuAPI.CreateMenu(IMenu parent, CancellationToken ct)
	{
		if (parent is not WASDMenu parentWasdMenu)
			throw new ArgumentException("Mismatched menu type", nameof(parent));

		if (!PlayerMenuStates.TryGetValue(parent.Player, out var menuState))
			menuState = PlayerMenuStates[parent.Player] = new WASDPlayerMenuState();

		CancellationToken linkedToken;
		if (!ct.CanBeCanceled)
			linkedToken = parentWasdMenu.Ct;
		else if (!parentWasdMenu.Ct.CanBeCanceled)
			linkedToken = ct;
		else
			linkedToken = CancellationTokenSource.CreateLinkedTokenSource(ct, parentWasdMenu.Ct).Token;

		var parents = new List<WASDMenu>(parentWasdMenu.Parents)
		{
			parentWasdMenu
		};

		return new WASDMenu()
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

	private const int ItemsVisible = 7;
	void ISampleMenu.DrawMenus(CCSPlayerController player)
	{
		int linesWrote = 0;
		void writeLine(string line = "", bool background = false, bool focused = false)
		{
			if (background)
				Console.ForegroundColor = ConsoleColor.DarkGray;
			if (focused)
				Console.BackgroundColor = ConsoleColor.DarkYellow;

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

				writeLine($"{focusedMenu.Title}:");

				if (focusedMenu.ScrollPosition <= focusedMenu.CurrentPosition - ItemsVisible)
					focusedMenu.ScrollPosition = focusedMenu.CurrentPosition - ItemsVisible + 1;
				else if (focusedMenu.ScrollPosition > focusedMenu.CurrentPosition)
					focusedMenu.ScrollPosition = focusedMenu.CurrentPosition;

				var itemsStart = focusedMenu.ScrollPosition;
				var itemsEnd = Math.Min(focusedMenu.ScrollPosition + ItemsVisible, focusedMenu.Items.Count);

				for (int i = itemsStart; i < itemsEnd; i++)
				{
					var item = focusedMenu.Items[i];
					writeLine(item.Title, background: !item.Enabled, focused: i == focusedMenu.CurrentPosition);
				}

				writeLine("A: Back W/S: Up/Down", background: true);
				writeLine("E&D: Select R: Exit", background: true);
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
		var canExit = focusedMenu.Parents.Where(x => !x.PlayerCanClose).Any() == false;

		switch (key.Key)
		{
			case ConsoleKey.D:
			case ConsoleKey.E:
				if (focusedMenu.CurrentPosition < focusedMenu.Items.Count)
					focusedMenu.Items[focusedMenu.CurrentPosition].RaiseSelected();
				break;
			case ConsoleKey.A:
				focusedMenu.Close();
				break;
			case ConsoleKey.W:
				focusedMenu.CurrentPosition = Math.Max(focusedMenu.CurrentPosition - 1, 0);
				break;
			case ConsoleKey.S:
				focusedMenu.CurrentPosition = Math.Min(focusedMenu.CurrentPosition + 1, focusedMenu.Items.Count - 1);
				break;
			case ConsoleKey.R:
				if (canExit)
					(focusedMenu as IMenu).Exit();
				break;
			default:
				break;
		}
	}
}

internal class WASDPlayerMenuState
{
	public List<WASDMenu> FocusStack { get; } = new();

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

internal class WASDMenu : IMenu, IMenuPriorityExtension
{
	public required WASDPlayerMenuState MenuState { get; init; }
	public required List<WASDMenu> Parents { get; init; }
	public required CancellationToken Ct { get; init; }
	public CancellationTokenRegistration Ctr { get; set; }
	public List<WASDMenuItem> Items { get; } = [];
	public int CurrentPosition { get; set; }
	public int ScrollPosition { get; set; }
	public WASDMenu? Parent => Parents.Count > 0 ? Parents[^1] : null;
	public DateTime OpenedAt { get; set; }

	// IMenu
	IMenu? IMenu.Parent => Parent;
	public required CCSPlayerController Player { get; internal init; }
	public bool IsActive => MenuState.FocusStack.Contains(this);
	public string Title { get; set; } = string.Empty;
	public bool PlayerCanClose { get; set; } = true;

	public IMenuItem CreateItem()
	{
		var ret = new WASDMenuItem() { Menu = this };
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
}

internal class WASDMenuItem : IMenuItem
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
}

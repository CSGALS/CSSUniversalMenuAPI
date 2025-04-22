using System.Diagnostics.CodeAnalysis;

using CounterStrikeSharp.API.Core;

namespace CSSUniversalMenuAPI;


public interface IMenuItem
{
	/// <summary>
	/// The menu that this item belongs to.
	/// </summary>
	IMenu Menu { get; }
	/// <summary>
	/// The player this menu item belongs to.
	/// </summary>
	CCSPlayerController Player => Menu.Player;
	/// <summary>
	/// The item text to show to the player.
	/// </summary>
	string Title { get; set; }
	/// <summary>
	/// Whether this item is enabled. The menu implementation decides how this is presented to the player.
	/// </summary>
	bool Enabled { get; set; }
	/// <summary>
	/// Event raised when this item has been selected.
	/// </summary>
	event ItemSelectedAction? Selected;
	object? Context { get; set; }

	/// <summary>
	/// Attempt to get an extension for the current menu item.<br/>
	///
	/// This is preferred over direct casting, as it allows menus to be wrapped,
	/// and switching the implementation at runtime for a single player.
	/// </summary>
	/// <typeparam name="TExtension">The extension's type.</typeparam>
	/// <param name="extension">The dynamic casted extension.</param>
	/// <returns>If the extension is supported, returns <c>true</c>, otherwise <c>false</c>.</returns>
	bool TryGetExtension<TExtension>([NotNullWhen(true)] out TExtension? extension)
		where TExtension : class
	{
		if (this is TExtension ext)
		{
			extension = ext;
			return true;
		}
		else
		{
			extension = null;
			return false;
		}
	}
}

public delegate void ItemSelectedAction(IMenuItem menuItem);

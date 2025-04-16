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
}

public delegate void ItemSelectedAction(IMenuItem menuItem);

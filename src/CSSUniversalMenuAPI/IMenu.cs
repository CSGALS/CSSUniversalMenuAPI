using System.Diagnostics.CodeAnalysis;

using CounterStrikeSharp.API.Core;

namespace CSSUniversalMenuAPI;

public interface IMenu
{
	/// <summary>
	/// The menu that this is a submenu of.
	/// </summary>
	IMenu? Parent { get; }
	/// <summary>
	/// The player that this menu is for.
	/// </summary>
	CCSPlayerController Player { get; }
	/// <summary>
	/// Whether the player can close this menu (not including parents)
	/// </summary>
	bool PlayerCanClose { get; set; }
	/// <summary>
	/// Whether the menu is open. May be occluded by another menu and not have focus.
	/// </summary>
	bool IsActive { get; }
	/// <summary>
	/// The text/heading/title of this option
	/// </summary>
	string Title { get; set; }
	/// <summary>
	/// Add an item to the menu.
	/// </summary>
	IMenuItem CreateItem();
	/// <summary>
	/// Present the menu to the player.
	/// </summary>
	void Display();
	/// <summary>
	/// Close the menu, yielding focus back to the <see cref="Parent"/> (or exiting).<br/>
	/// In SourceMod, this would be equivalent to pressing "Back" (7 or 8).
	/// </summary>
	void Close();
	/// <summary>
	/// Close the current menu, and all parents, yielding focus to some other root menu if one exists.<br/>
	/// In SourceMod, this would be equivalent to pressing "Exit" (9 or 0).
	/// </summary>
	void Exit()
	{
		IMenu? current = this;
		do
		{
			current.Close();
			current = current.Parent;
		} while (current is not null);
	}

	/// <summary>
	/// Attempt to get an extension for the current menu. <br/>
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

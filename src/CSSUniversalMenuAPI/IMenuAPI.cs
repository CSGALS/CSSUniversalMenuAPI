using System;
using System.Threading;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

namespace CSSUniversalMenuAPI;

public interface IMenuAPI
{
	/// <summary>
	/// Create a menu root menu with no parent.
	/// </summary>
	/// <param name="player">The player to present this menu to.</param>
	/// <param name="ct">A token to destroy this menu, such as on plugin unload.</param>
	/// <returns>A menu which can be further manipulated, then displayed to the player.</returns>
	IMenu CreateMenu(CCSPlayerController player, CancellationToken ct = default);
	/// <summary>
	/// Create a menu with a parent. This menu will be presented to the parent menu's root player.
	/// </summary>
	/// <param name="parent">The parent menu to this menu.</param>
	/// <param name="ct">A token to destroy this menu, such as on plugin unload.</param>
	/// <returns>A menu which can be further manipulated, then displayed to the player.</returns>
	IMenu CreateMenu(IMenu parent, CancellationToken ct = default);
	/// <summary>
	/// Allow the plugin to query the implementations capabilities prior to creating any menus.
	/// </summary>
	/// <param name="extension">The type of the interface desired.</param>
	/// <returns>True if the implementation supports the extension.</returns>
	bool IsExtensionSupported(Type extension);
	/// <summary>
	/// Query whether the player has a menu open already.
	/// Intended for use where we want to ask the player something, but we want to wait/not override
	/// the current menu, without using the priority extension or such. <br/>
	/// Note, providing access to the currently focused menu is intentionally not done. Plugins should not need to do this,
	/// and may clobber other plugin's menus.
	/// </summary>
	/// <param name="player">The player of which to query.</param>
	/// <returns>Whether the player has an active menu on their screen.</returns>
	bool IsMenuOpen(CCSPlayerController player);

	/// <summary>
	/// Standard helper to get get or provide this implementation
	/// </summary>
	static readonly PluginCapability<IMenuAPI> PluginCapability = new("universalmenuapi");
}

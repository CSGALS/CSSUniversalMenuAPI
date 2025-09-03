using System;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

using CSSUniversalMenuAPI;

namespace HelloMenu;

[MinimumApiVersion(314)]
public sealed class TeamDeathmatchPlugin : BasePlugin
{
	public override string ModuleName => "CsGals.TeamDeathmatch";
	public override string ModuleDescription => "Team Deathmatch gamemode plugin";
	public override string ModuleVersion => Verlite.Version.Full;


	[ConsoleCommand("css_hello")]
	public void HelloMenu(CCSPlayerController player, CommandInfo info)
	{
		var menu = UniversalMenu.CreateMenu(player);
		menu.Title = "Hello";

		var playerItem = menu.CreateItem();
		playerItem.Title = "Player";
		playerItem.Selected += PlayerItem_Selected;

		var aboutItem = menu.CreateItem();
		aboutItem.Title = "About";
		aboutItem.Selected += AboutItem_Selected;

		menu.Display();
	}

	private void PlayerItem_Selected(IMenuItem menuItem)
	{
		var player = menuItem.Menu.Player;
		player.PrintToChat($"Hello {player.PlayerName}");

		// close does not happen implicitly, you need to tell it to close
		menuItem.Menu.Close();
	}

	private void AboutItem_Selected(IMenuItem menuItem)
	{
		var childMenu = UniversalMenu.CreateMenu(menuItem.Menu);

		var authorItem = childMenu.CreateItem();
		authorItem.Title = $"Author: Ashleigh Adams";
		authorItem.Enabled = false;

		var versionItem = childMenu.CreateItem();
		versionItem.Title = $"Version: {Verlite.Version.Full}";
		versionItem.Enabled = false;

		var revItem = childMenu.CreateItem();
		revItem.Title = $"Revision: {Verlite.Version.Commit[..8]}";
		revItem.Enabled = false;

		childMenu.Display();
	}
}

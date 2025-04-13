using System.Collections.Generic;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;

namespace ProofOfConcepts;

public static partial class Program
{
	public static void AdminCommand(CCSPlayerController player)
	{
		var primaryMenu = API.CreateMenu(player);
		primaryMenu.Title = "Admin Menu";

		foreach (var primaryGun in PrimaryGuns)
		{
			var item = primaryMenu.CreateItem();
			item.Title = primaryGun;
			item.Selected += PrimaryGun_Selected;
		}

		primaryMenu.Display();
	}
}

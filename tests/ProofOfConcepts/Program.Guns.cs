using System.Collections.Generic;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;
using CSSUniversalMenuAPI.Extensions;

namespace ProofOfConcepts;

public static partial class Program
{
	public static string[] PrimaryGuns { get; } =
	{
		"Random",
		"M4A1",
		"AK47",
		"Galil",
		"M249",
		"Famas",
		"SG552",
		"AUG",
		"M3",
		"AWP",
		"SG550",
		"G3SG1",
		"XM1014",
		"MAC 10",
		"TMP",
		"MP5",
		"UMP45",
		"P90",
		"Scout",
	};

	public static string[] SecondaryGuns { get; } =
	{
		"USP",
		"Glock",
		"Deagle",
		"P228",
		"Elite",
		"Five Seven",
		"Random",
	};

	public static Dictionary<string, string?> DisabledGuns { get; } = new()
	{
		["AWP"] = "2/2",
		["SG550"] = null,
		["G3SG1"] = null,
	};

	public static void GunsCommand(CCSPlayerController player)
	{
		var primaryMenu = API.CreateMenu(player);
		primaryMenu.Title = "Primary Weapon";

		foreach (var primaryGun in PrimaryGuns)
		{
			var item = primaryMenu.CreateItem();
			item.Title = primaryGun;

			if (DisabledGuns.TryGetValue(primaryGun, out var disabledInfo))
			{
				item.Enabled = false;
				if (disabledInfo is not null)
					item.Title = $"{primaryGun} [{disabledInfo}]";
			}

			if (item.Enabled)
				item.Selected += PrimaryGun_Selected;
		}

		primaryMenu.Display();
	}

	private static string PrimaryWeapon { get; set; } = PrimaryGuns[2];
	private static string SecondaryWeapon { get; set; } = SecondaryGuns[2];
	private static void PrimaryGun_Selected(IMenuItem selectedItem)
	{
		PrimaryWeapon = selectedItem.Title; // should use .Context to find the real value

		var secondaryMenu = API.CreateMenu(selectedItem.Menu);
		secondaryMenu.Title = "Secondary Weapon";

		foreach (var secondaryGun in SecondaryGuns)
		{
			var item = secondaryMenu.CreateItem();
			item.Title = secondaryGun;

			if (DisabledGuns.TryGetValue(secondaryGun, out var disabledInfo))
			{
				item.Enabled = false;
				if (disabledInfo is not null)
					item.Title = $"{secondaryGun} [{disabledInfo}]";
			}

			if (item.Enabled)
				item.Selected += SecondaryGun_Selected;
		}

		secondaryMenu.Display();
	}

	private static void SecondaryGun_Selected(IMenuItem selectedItem)
	{
		SecondaryWeapon = selectedItem.Title; // should use .Context to find the real value
		selectedItem.Menu.Exit();
	}
}

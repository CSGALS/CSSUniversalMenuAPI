using System;

using CounterStrikeSharp.API.Core;

namespace ProofOfConcepts;

public static partial class Program
{
	private static ISampleMenu API { get; set; } = new NumberKeysMenuAPI();

	private static void MenuCommand(CCSPlayerController player)
	{
		var menu = API.CreateMenu(player);
		menu.Title = "Menu Type";

		var numbersItem = menu.CreateItem();
		numbersItem.Title = "Numbers";
		numbersItem.Enabled = API.GetType() != typeof(NumberKeysMenuAPI);
		if (numbersItem.Enabled)
			numbersItem.Selected += (menuItem) =>
			{
				menuItem.Menu.Exit();
				API = new NumberKeysMenuAPI();
			};

		var wasdItem = menu.CreateItem();
		wasdItem.Title = "WASD";
		wasdItem.Enabled = API.GetType() != typeof(WASDMenuAPI);
		if (wasdItem.Enabled)
			wasdItem.Selected += (menuItem) =>
			{
				menuItem.Menu.Exit();
				API = new WASDMenuAPI();
			};

		menu.Display();
	}

	//private static ISampleMenu API { get; set; } = new WASDMenuAPI();

	public static int Main()
	{
		string? currentCmd = null;
		CCSPlayerController player = new(nint.Zero);

		bool first = true;
		while (true)
		{
			if (!first)
			{
				var key = Console.ReadKey(true);

				Console.Clear();

				if (currentCmd is null)
				{
					if (key.Key == ConsoleKey.Y)
						currentCmd = "";
					else
						API.HandleKey(player, key);
				}
				else
				{
					switch (key.Key)
					{
						case ConsoleKey.Backspace:
							if (currentCmd.Length > 0)
								currentCmd = currentCmd[..^1];
							break;
						case ConsoleKey.Enter:
							switch (currentCmd)
							{
								case "!guns":
									GunsCommand(player);
									break;
								case "!admin":
									AdminCommand(player);
									break;
								case "!menu":
									MenuCommand(player);
									break;
								default:
									break;
							}
							currentCmd = null;
							break;
						default:
							currentCmd += key.KeyChar;
							break;
					}
				}
			}
			first = false;

			API.DrawMenus(player);
			Console.WriteLine($"Primary: {PrimaryWeapon}, Secondary: {SecondaryWeapon}");
			if (currentCmd is not null)
				Console.Write($"Command: {currentCmd}");
		}
	}
}

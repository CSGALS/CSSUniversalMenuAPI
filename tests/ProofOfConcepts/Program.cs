using System;

using CounterStrikeSharp.API.Core;

namespace ProofOfConcepts;

public static partial class Program
{
	private static ISampleMenu API { get; set; } = new NumberKeysMenuAPI();

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

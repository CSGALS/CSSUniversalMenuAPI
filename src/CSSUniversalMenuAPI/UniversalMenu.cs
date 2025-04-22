using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;

using CounterStrikeSharp.API.Core;

namespace CSSUniversalMenuAPI;

/// <summary>
/// Because menus may want access to other drivers, and PluginCapability&lt;&gt; is not expressive enough,
/// this helper class is included with the interface to aid in more complex scenarios, such as a switcher
/// that allows a player to use their preferred menu.
/// </summary>
public static class UniversalMenu
{
	private static string? DefaultDriverDesired { get; }
	private static string? DefaultDriverName { get; set; }
	public static IMenuAPI? DefaultDriver { get; private set; }

	private static Dictionary<string, IMenuAPI> RegisteredDrivers { get; } = new();
	public static IReadOnlyDictionary<string, IMenuAPI> Drivers => RegisteredDrivers;
	public static event EventHandler? DriversChanged;

	static UniversalMenu()
	{
		var configDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
		var configPath = Path.Join(configDir, "config.json");

		if (!File.Exists(configPath))
		{
			Console.WriteLine("UniversalMenu: No default driver configured, will use first registered driver");
			return;
		}

		var contents = File.ReadAllText(configPath);
		var doc = JsonDocument.Parse(contents);
		DefaultDriverDesired = doc.RootElement.GetProperty("driver").GetString();

		Console.WriteLine($"UniversalMenu: Desired default driver: {DefaultDriverDesired}");
	}

	public static void RegisterDriver(string name, IMenuAPI driver)
	{
		if (!RegisteredDrivers.TryAdd(name, driver))
			throw new InvalidOperationException($"RegisterDriver(): Conflicting named drivers configured: {name}");

		if (DefaultDriverDesired is null || name == DefaultDriverDesired)
		{
			if (DefaultDriver is not null)
				throw new InvalidOperationException($"RegisterDriver(): Conflicting default drivers configured: {name} and {DefaultDriverName}");
			DefaultDriver = driver;
			DefaultDriverName = name;
		}
		DriversChanged?.Invoke(null, EventArgs.Empty);
	}

	public static void UnregisterDriver(string name)
	{
		if (RegisteredDrivers.Remove(name, out var driver) && driver == DefaultDriver)
		{
			DefaultDriver = null;
			DefaultDriverName = null;
		}
		DriversChanged?.Invoke(null, EventArgs.Empty);
	}

	public static IMenu CreateMenu(CCSPlayerController player, CancellationToken ct = default)
	{
		if (DefaultDriver is null)
			throw new InvalidOperationException("No default driver has been registered. Do you have a driver installed and/or configured?");
		return DefaultDriver.CreateMenu(player, ct);
	}

	public static IMenu CreateMenu(IMenu parent, CancellationToken ct = default)
	{
		if (DefaultDriver is null)
			throw new InvalidOperationException("No default driver has been registered. Do you have a driver installed and/or configured?");
		return DefaultDriver.CreateMenu(parent, ct);
	}
}

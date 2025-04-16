using System.Threading;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Commands;

using CSSUniversalMenuAPI;

namespace UniversalMenu.ScreenMenuAPIAdapter;

[MinimumApiVersion(314)]
public class ScreenMenuAPIDriverPlugin : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Driver.ScreenMenuAPI";
	public override string ModuleDescription => "Implement CSSUniversalMenuAPI via ScreenMenuAPI";
	public override string ModuleVersion => Verlite.Version.Full;

	internal CancellationTokenSource Cts { get; set; } = null!;
	private ScreenMenuApiDriver? DriverInstance { get; set; }

	public override void Load(bool hotReload)
	{
		Cts = new CancellationTokenSource();

		Capabilities.RegisterPluginCapability(IMenuAPI.PluginCapability, () =>
		{
			DriverInstance ??= new ScreenMenuApiDriver(this);
			return DriverInstance;
		});
	}

	public override void Unload(bool hotReload)
	{
		Cts.Cancel();
	}

	[ConsoleCommand("css_0"), ConsoleCommand("css_1"), ConsoleCommand("css_2"), ConsoleCommand("css_3"), ConsoleCommand("css_4")]
	[ConsoleCommand("css_5"), ConsoleCommand("css_6"), ConsoleCommand("css_7"), ConsoleCommand("css_8"), ConsoleCommand("css_9")]
	[ConsoleCommand("css_screenmenu_bound_buttons")]
	public void RegisterKeyCommands(CCSPlayerController player, CommandInfo info)
	{
		if (player is null || !player.IsValid)
			return;

		var menuState = DriverInstance?.GetMenuState(player);
		if (menuState is null)
			return;
		menuState.UsingKeyBinds = info.CallingContext == CommandCallingContext.Console;
	}
}

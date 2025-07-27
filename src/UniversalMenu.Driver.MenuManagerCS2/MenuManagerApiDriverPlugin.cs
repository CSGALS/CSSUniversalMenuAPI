using System;
using System.Threading;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;

using IMenuManagerApi = MenuManager.IMenuApi;

namespace UniversalMenu.Driver.MenuManagerCS2;

[MinimumApiVersion(314)]
public class MenuManagerApiDriverPlugin : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Driver.MenuManagerCS2";
	public override string ModuleDescription => "Implement CSSUniversalMenuAPI via MenuManagerCS2";
	public override string ModuleVersion => Verlite.Version.Full;

	internal CancellationTokenSource Cts { get; set; } = null!;
	private MenuManagerApiDriver? DriverInstance { get; set; }
	private static readonly PluginCapability<IMenuManagerApi> MenuCapability = new("menu:nfcore");

	public override void Load(bool hotReload)
	{
		Cts = new CancellationTokenSource();
		DriverInstance = new MenuManagerApiDriver(this);
		CSSUniversalMenuAPI.UniversalMenu.RegisterDriver("MenuManagerCS2", DriverInstance);
	}

	public override void Unload(bool hotReload)
	{
		Cts.Cancel();
		CSSUniversalMenuAPI.UniversalMenu.UnregisterDriver("MenuManagerCS2");
	}

	public override void OnAllPluginsLoaded(bool hotReload)
	{
		DriverInstance!.MenuManagerApi = MenuCapability.Get();
	}

	[GameEventHandler(HookMode.Pre)]
	public HookResult OnPlayerDisconnect(EventPlayerDisconnect e, GameEventInfo info)
	{
		if (e.Userid is null)
			return HookResult.Continue;

		DriverInstance?.PlayerDisconnected(e.Userid.SteamID);
		return HookResult.Continue;
	}
}

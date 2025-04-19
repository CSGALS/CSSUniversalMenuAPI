using System;
using System.Threading;

using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Capabilities;

using CSSUniversalMenuAPI;

using IMenuManagerAPI = MenuManager.IMenuApi;

namespace UniversalMenu.Compat.MenuManagerApi;

[MinimumApiVersion(314)]
public class MenuManagerCompat : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Compat.MenuManagerApi";
	public override string ModuleDescription => "Translate MenuManagerApi to CSSUniversalMenuAPI";
	public override string ModuleVersion => Verlite.Version.Full;

	internal CancellationTokenSource Cts { get; set; } = null!;
	private static readonly PluginCapability<IMenuManagerAPI> MenuCapability = new("menu:nfcore");
	private MenuManagerTranslator? Instance { get; set; }

	public override void Load(bool hotReload)
	{
		Cts = new CancellationTokenSource();
		Capabilities.RegisterPluginCapability(MenuCapability, () =>
		{
			if (Instance is not null)
				return Instance;

			Instance = new MenuManagerTranslator(this);

			return Instance;
		});
	}

	public override void Unload(bool hotReload)
	{
		Cts.Cancel();
	}
}

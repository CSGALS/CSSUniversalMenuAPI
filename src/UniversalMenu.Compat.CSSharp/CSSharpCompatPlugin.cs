using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace UniversalMenu.Compat.CSSharp;

public class CSSharpCompatPlugin : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Compat.CSSharp";
	public override string ModuleVersion => Verlite.Version.Full;

	public override void Load(bool hotReload)
	{
		CSSharpCompatPluginShared.Patch();
	}

	public override void Unload(bool hotReload)
	{
		CSSharpCompatPluginShared.Unpatch();
	}

	[GameEventHandler(HookMode.Pre)]
	public HookResult OnPlayerDisconnect(EventPlayerDisconnect e, GameEventInfo info)
	{
		if (e.Userid is null)
			return HookResult.Continue;

		CSSharpCompatPluginShared.PlayerDisconnected(e.Userid);
		return HookResult.Continue;
	}
}

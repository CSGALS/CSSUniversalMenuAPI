using CounterStrikeSharp.API.Core;

namespace UniversalMenu.Compat.CSSharp;

public class CSSSharpCompatPlugin : BasePlugin
{
	public override string ModuleName => "UniversalMenu.Compat.CSSharp";
	public override string ModuleVersion => Verlite.Version.Full;

	public override void Load(bool hotReload)
	{
		CSSSharpCompatPluginShared.Patch();
	}

	public override void Unload(bool hotReload)
	{
		CSSSharpCompatPluginShared.Unpatch();
	}
}

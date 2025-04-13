using System;
using System.Threading;

using CounterStrikeSharp.API.Core;

namespace CSSUniversalMenuAPI;

public interface IMenuAPI
{
	IMenu CreateMenu(CCSPlayerController player, CancellationToken ct = default);
	IMenu CreateMenu(IMenu parent, CancellationToken ct = default);
	bool IsExtensionSupported(Type extension);
	bool IsMenuOpen(CCSPlayerController player);
}

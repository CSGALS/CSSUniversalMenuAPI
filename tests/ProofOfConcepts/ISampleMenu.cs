using System;

using CounterStrikeSharp.API.Core;

using CSSUniversalMenuAPI;

namespace ProofOfConcepts;

internal interface ISampleMenu : IMenuAPI
{
	void DrawMenus(CCSPlayerController player);
	void HandleKey(CCSPlayerController player, ConsoleKeyInfo key);
}


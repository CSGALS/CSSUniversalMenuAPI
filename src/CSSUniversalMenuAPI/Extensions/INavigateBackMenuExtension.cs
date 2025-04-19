using System;

namespace CSSUniversalMenuAPI.Extensions;

/// <summary>
/// Rather than navigating to the parent menu, this will be invoked instead.<br />
/// It is preferred you use <see cref="IMenuAPI.CreateMenu(IMenu, System.Threading.CancellationToken)"/>,
/// as this will retain any menu state such as scroll position, but this extension is provided to help the
/// compatibility layers get a more native feel.<br/>
/// </summary>
/// <remarks>
/// <b>When using this over <see cref="IMenu.Parent"/>, ensure the "parent" menu has been closed, else the menu
/// driver may give focus to the parent instead of the child.</b>
/// </remarks>
public interface INavigateBackMenuExtension
{
	Action<IMenu>? NavigateBack { get; set; }
}

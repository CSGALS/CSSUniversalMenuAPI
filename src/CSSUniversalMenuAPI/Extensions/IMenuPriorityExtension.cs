namespace CSSUniversalMenuAPI.Extensions;

/// <summary>
/// The implementation can handle concurrent menus. Provides hints and methods relating to this function.
/// </summary>
public interface IMenuPriorityExtension
{
	/// <summary>
	/// The priority of the menu
	/// </summary>
	double Priority { get; set; }

	/// <summary>
	/// Whether the menu is currently in the foreground.
	/// </summary>
	bool IsFocused { get; }
}

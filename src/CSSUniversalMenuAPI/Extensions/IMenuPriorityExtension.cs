namespace CSSUniversalMenuAPI.Extensions;

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

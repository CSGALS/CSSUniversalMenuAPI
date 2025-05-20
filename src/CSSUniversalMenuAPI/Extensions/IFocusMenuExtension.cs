namespace CSSUniversalMenuAPI.Extensions;

/// <summary>
/// Allow a menu or a notification to be presented to a player, but without taking focus from the player. <br/>
/// This should be used when you want to present a menu to a player who may not be expecting it, such as votes.
/// </summary>
public interface IFocusMenuExtension
{
	/// <summary>
	/// When <c>true</c>, the menu shall open in a state where the player can see it, or see a notification,
	/// but require further action to be manipulate. It is expected that this would be a key that requires pressing
	/// to toggle the menu.<br/>
	///<br/>
	/// Implementations should default this value to <c>true</c>.
	/// </summary>
	bool OpenWithFocus { get; set; }

	/// <summary>
	/// Returns whether or not the player's input is to be received by this menu if it were visible. <br/>
	/// Whether the menu is being occluded by another menu should have no bearing on this value.
	/// </summary>
	bool HasFocus { get; }

	/// <summary>
	/// Returns whether or not this menu's input has never been focused, enabling scenarios such as timing out a menu if
	/// it has received no interaction from the player.
	/// </summary>
	bool NeverHadFocus { get; }
}

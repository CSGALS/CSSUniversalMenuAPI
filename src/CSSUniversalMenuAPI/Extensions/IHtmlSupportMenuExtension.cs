namespace CSSUniversalMenuAPI.Extensions;

/// <summary>
/// An extension to expose HTML based title support.
/// </summary>
public interface IHtmlSupportMenuExtension
{
	/// <summary>
	/// Opt in rendering as HTML. Should default to false by the implementation. <br/>
	/// When set to false, the implementation should escape HTML tags and entities in titles.
	/// </summary>
	public bool UseHtml { get; set; }
}

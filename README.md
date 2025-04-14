# UniversalMenuAPI

A universal API for CounterStrikeSharp plugins to implement or use.

## Concerns & to do

- Fragmentation: https://xkcd.com/927/
  - Create adapters to and from existing interfaces
  - Push for native support in all existing menus
  - Check every usecase is supported via the API, or an extension
- Interface is agnostic to menu type
  - What use case is this not from a preference?
  - Is there any way we can have multiple implementations at the same time?
- Multiple menus at the same time?
- Why is there no "broadcast" API
  Complexities over navigation can be eliminated by forcing a menu to be created per player
  It also raises questions over expected behavior. Instead encode that behavior in the plugin as desired.
  Object pooling should be done if there are concerns over memory allocations and GC pauses


### API a little verbose?

By limiting surface area of the API, it makes it harder to abuse, and easier to amend or extend it in future.
It is expected that consumers of this API build their own internal extension methods if they want less verbosity,
rather than causing complexities in the interface.

You can tailor your extension methods to be as concise or verbose as is appropriate for you.
In the proof of concept sample, the `!admin` menu is set up with a very concise example.

```cs
API.CreateAndShowMenu(player, new("Admin")
{
	new("Players Manage")
	{
		new("Slap", players: Players)
		{
			new("0hp", exit: false),
			new("1hp", exit: false),
			new("10hp", exit: false),
			new("100hp", exit: false),
		},
	},
	new("Server Manage"),
	new("Fun Commands")
	{
		new("God Mode", players: Players),
		new("No Clip", players: Players),
		new("Respawn", players: Players),
		new("Give Weapon", players: Players)
		{
			new("AK47"),
			new("M4"),
			new("Deagle"),
		},
	},
	new("Admins Manage"),
});
```

But you could go as simple as this, if it just being a little less verbose suffices:

```cs
var menu = API.CreateMenu(player);
menu.AddItem("Test", (menuItem) => Console.WriteLine("Test"));
menu.Display();
```

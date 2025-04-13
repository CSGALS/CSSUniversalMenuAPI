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

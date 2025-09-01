# JSON RPG Maker

JSON RPG Maker is a versatile console-based RPG engine built in C#, designed to create and play customizable text-based adventure games defined in JSON files. The engine supports dynamic game worlds, items, and commands, powered by JSON for data and NLua for scripting. A demo game inspired by *Oregon Trail* showcases the engine’s ability to create engaging adventures with navigation, resource management, and interactive gameplay.

## Features
- **Data-Driven Design**: Define game worlds (rooms, items, rules, commands) in JSON for rapid customization.
- **Dynamic Command System**: Execute actions (e.g., `go`, `take`, `use`, `inventory`) using Lua scripts via NLua.
- **Flexible Gameplay**: Create any text-based adventure, from fantasy quests to historical journeys like Oregon Trail.
- **Save/Load System**: Persist game progress using JSON serialization.
- **Extensible Framework**: Add new features, commands, or game mechanics with minimal code changes.

## Tech Stack
- **C# & .NET**: Core language and framework for robust engine logic.
- **System.Text.Json**: JSON parsing for game data serialization/deserialization.
- **NLua**: Lua scripting for dynamic, extensible command execution.
- **Console UI**: Text-based interface for accessible gameplay.

## Folder Structure
```
JSON_RPG_Maker/
├── GameCommand.cs
├── GameData.cs
├── GameEngine.cs
├── GameEntity.cs
├── Player.cs
├── Program.cs
├── Room.cs
├── Games/
│   ├── games.JSON
│   └── games1.JSON
└── ...
```
## Setup Instructions
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/CodyPhelps93/JSON-RPG-Maker
2. **Install .NET SDK**: Ensure SDK 6.0 or later is installed
3. **Install NLua**: Add the NLua NuGet package:
   ```bash
   dotnet add package NLua
4. **Build and Run**:
   ```bash
   cd JSON_RPG_Maker
   dotnet build
   dotnet run

## Example Gameplay
```
You are in: Independence
You are in Independence, Missouri, the starting point of your journey.
The trail stretches forward to the west.
A trading post is nearby to the north.
Exits: forward, trading post
Items found in the room: food, water

Enter command: (go <direction>, take <item>, inventory, quit, save) take food
You took the food

Enter command: inventory
Inventory: food, water, wagon wheel
```

## How to Create a Game
1. **Create a JSON file** in the `Games` folder (e.g., `mygame.JSON`).
2. **Define your game structure** using the following format:

```json
{
  "StartingRoom": "Village",
  "GameRules": [
    "Navigate with 'go <direction>'.",
    "Collect items with 'take <item>'.",
    "Complete your quest!"
  ],
  "PlayerInventory": ["coin"],
  "Rooms": [
    {
      "Name": "Village",
      "Description": "A quiet village square.\nA path leads north to a forest.\nA shop is to the east.",
      "Exits": { "north": "Forest", "east": "Shop" },
      "Items": ["sword", "potion"]
    },
    {
      "Name": "Forest",
      "Description": "A dense forest with tall trees.\nThe village is back to the south.",
      "Exits": { "south": "Village" },
      "Items": ["herb"]
    },
    {
      "Name": "Shop",
      "Description": "A small shop with goods.\nThe village is back to the west.",
      "Exits": { "west": "Village" },
      "Items": ["shield"]
    }
  ],
  "PlayerCommands": [
    {
      "Name": "go",
      "Description": "Move to a new location.",
      "LScript": "if room.Exits[direction] then\n    player.CurrentRoom = room.Exits[direction]\n    print('You move ' .. direction .. ' to ' .. player.CurrentRoom)\nelse\n    print('You can\\'t go that way.')\nend"
    },
    {
      "Name": "take",
      "Description": "Pick up an item.",
      "LScript": "for i = 1, room.Items.Count do\n    local item = room.Items[i - 1]\n    if item == target then\n        room.Items:Remove(item)\n        player.Inventory:Add(item)\n        print('You took the ' .. item)\n        return\n    end\nend\nprint('Item not found in the room.')"
    },
    {
      "Name": "inventory",
      "Description": "View inventory.",
      "LScript": "local count = player.Inventory.Count\nif count == 0 then\n    print('Your inventory is empty.')\nelse\n    local items = {}\n    for i = 0, count - 1 do\n        items[i+1] = player.Inventory[i]\n    end\n    print('Inventory: ' .. table.concat(items, ', '))\nend"
    }
  ]
}
```
- **StartingRoom**: The name of the room where the player begins.
- **GameRules**: Optional list of rules or objectives.
- **Rooms**: Array of room objects, each with a name, description, exits, and items.
- **PlayerInventory**: Starting items.
- **PlayerCommands**: Lua scripts for game actions.

3. Run the engine and type in your game file name.

## Example Commands
```
go north
take torch
inventory
rules
quit
save
```

## Extending the Engine (To be added)
- Custom Commands: Add new Lua-based commands to PlayerCommands in JSON (e.g., use, trade).
- Advanced Mechanics: Modify `GameEngine.cs` to support health, random events, or quests.


## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a branch: `git checkout -b feature/new-feature`.
3. Add new games, commands, or engine features.
4. Submit a pull request with a clear description.

Report any issues via [GitHub Issues](https://github.com/CodyPhelps93/JSON_RPG_Maker/issues)




# JSON RPG Maker

JSON RPG Maker is a simple, extensible console-based RPG engine written in C#. It allows you to create, load, and play text-based adventure games defined in JSON format. The engine reads game data from JSON files, enabling rapid prototyping and easy customization of game worlds, rooms, items, and rules.

## Features
- Load and play RPG games defined in JSON files
- Explore rooms, collect items, and interact with the environment
- Customizable game rules and player inventory
- Simple command-based interface

## Folder Structure
```
JSON_RPG_Maker/
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

## How to Create a Game
1. **Create a JSON file** in the `Games` folder (e.g., `mygame.JSON`).
2. **Define your game structure** using the following format:

```json
{
  "StartingRoom": "Entrance",
  "GameRules": ["No cheating", "Find the treasure"],
  "Rooms": [
    {
      "Name": "Entrance",
      "Description": "You are at the entrance of a dark cave.",
      "Exits": {"north": "Hallway"},
      "Items": ["torch"]
    },
    {
      "Name": "Hallway",
      "Description": "A long, narrow hallway.",
      "Exits": {"south": "Entrance", "east": "Treasure Room"},
      "Items": []
    }
    // Add more rooms as needed
  ]
}
```
- **StartingRoom**: The name of the room where the player begins.
- **GameRules**: Optional list of rules or objectives.
- **Rooms**: Array of room objects, each with a name, description, exits, and items.

## How to Load a Game
1. **Run the application** (`JSON_RPG_Maker.exe`).
2. The engine will list all available JSON game files in the `Games` folder.
3. **Type the name of the file** you wish to load (e.g., `games.JSON`).
4. Play the game using commands:
   - `go <direction>`: Move to another room
   - `take <item>`: Pick up an item
   - `inventory`: Show your inventory
   - `rules`: Display game rules
   - `quit`: Exit the game

## Example Commands
```
go north
take torch
inventory
rules
quit
```

## Extending the Engine
- Add new properties to rooms, items, or player in the JSON and corresponding C# classes.
- Implement new commands in `GameEngine.cs` for more interactions.

## License
MIT License

using System.Collections.Generic;

namespace JSON_RPG_Maker
{
    // Game data structure for JSON deserialization
    public class GameData
    {
        public required string StartingRoom { get; set; }
        public List<string>? GameRules { get; set; }
        public required List<Room> Rooms { get; set; }
        public List<string>? PlayerInventory { get; set; }
        public required List<GameCommand> PlayerCommands { get; set; }
    }
}
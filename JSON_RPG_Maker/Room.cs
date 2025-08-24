using System.Collections.Generic;

namespace JSON_RPG_Maker
{
    public class Room : GameEntity
    {
        public Dictionary<string, string> Exits { get; set; }
        public List<string> Items { get; set; }

        public Room(string name, string description) : base(name, description)
        {
            Exits = new Dictionary<string, string>();
            Items = new List<string>();
        }
    }
}
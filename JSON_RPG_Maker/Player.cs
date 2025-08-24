using System.Collections.Generic;

namespace JSON_RPG_Maker
{
    // Player class to track current room and inventory
    public class Player
    {
        public string CurrentRoom { get; set; }
        public List<string> Inventory { get; set; }

        public Player(string StartingRoom)
        {
            CurrentRoom = StartingRoom;
            Inventory = new List<string>();
        }

    }
}
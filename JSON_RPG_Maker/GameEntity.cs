namespace JSON_RPG_Maker
{
    public abstract class GameEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public GameEntity(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
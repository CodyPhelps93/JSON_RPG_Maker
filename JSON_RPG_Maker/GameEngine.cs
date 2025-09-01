using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLua;

namespace JSON_RPG_Maker
{
    public class GameEngine
    {
        private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private Player player;
        private List<string> gameRules = new List<string>();
        private List<string> PlayerInventory = new List<string>();
        private List<GameCommand> PlayerCommands = new List<GameCommand>();

        public void Run()
        {
            Console.WriteLine("Welcome to the JSON RPG Maker! \n" +
            "Your new advbenture awaits...\n");
            if (!LoadGameData())
            {
                Console.WriteLine("Failed to load game data. Exiting...");
                return;
            }
            GameLoop();
        }

        public void GameLoop()
        {
            bool isRunning = true;
            DisplayGameRules();
            while (isRunning)
            {
                DisplayCurrentRoom();
                Console.Write("\nEnter command: (go <direction>, take <item>, inventory, quit, save) ");
                string input = Console.ReadLine().Trim().ToLower();
                if (string.IsNullOrEmpty(input)) continue;

                if (input == "quit")
                {
                    isRunning = false;
                    Console.WriteLine("Thanks for playing!");
                }
                else
                {
                    ProcessCommand(input);
                }
            }
        }

        private bool LoadGameData()
        {
            try
            {
                // sets the path to the Games Folder
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName ?? throw new DirectoryNotFoundException("could not determine root directory");
                string gamesFolder = Path.Combine(projectRoot, "Games");
                if (!Directory.Exists(gamesFolder))
                {
                    Console.WriteLine("Games folder not found.");
                    return false;
                }

                // List all JSON files in the Games Folder
                string[] gameFiles = Directory.GetFiles(gamesFolder, "*.json");
                if (gameFiles.Length == 0)
                {
                    Console.WriteLine("No game files found in the Games folder.");
                    return false;
                }
                Console.WriteLine("Available games:");
                for (int i = 0; i < gameFiles.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(gameFiles[i])}");
                }

                // Prompt user to select a game file
                Console.Write("Enter the name of the file you want to load (e.g., games.JSON): ");
                string selectedFile = Console.ReadLine().Trim();
                // Append .json if not provided
                if (!selectedFile.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    selectedFile += ".json";
                }
                string filePath = Path.Combine(gamesFolder, selectedFile);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    return false;
                }

                // Read and deserialize the JSON file
                string jsonData = File.ReadAllText(filePath);
                GameData gameData = JsonSerializer.Deserialize<GameData>(jsonData);
                gameRules = gameData.GameRules ?? new List<string>();
                PlayerInventory = gameData.PlayerInventory ?? new List<string>();
                PlayerCommands = gameData.PlayerCommands ?? new List<GameCommand>();
                
                if (gameData != null)
                {
                    if (gameData?.Rooms != null)
                    {
                        foreach (var room in gameData.Rooms)
                            rooms[room.Name] = room;
                    }
                    if (gameData.StartingRoom != null)
                    {
                        player = new Player(gameData.StartingRoom);
                        player.Inventory = PlayerInventory;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game data: {ex.Message}");
                return false;
            }
        }

        private void DisplayGameRules()
        {
            Console.WriteLine("Game Rules:");
            foreach (var rule in gameRules)
            {
                Console.WriteLine($"- {rule}");
            }
            Console.WriteLine();
        }

        private void DisplayCurrentRoom()
        {
            if (rooms.TryGetValue(player.CurrentRoom, out Room room))
            {
                Console.WriteLine($"\nYou are in: {room.Name}");
                Console.WriteLine(room.Description);
                Console.WriteLine("Exits: " + string.Join(", ", room.Exits.Keys));
                Console.WriteLine("Items found in the room: " + (room.Items.Count > 0 ? string.Join(", ", room.Items) : "None"));
            }
            else
            {
                Console.WriteLine("Error: Current room not found.");
            }
        }

        private void ProcessCommand(string input)
        {
            if (input == "save")
            {
                SaveGameData();
                return;
            }
            else if (input == "rules")
            {
                DisplayGameRules();
                return;
            }
            else
            {
                string[] parts = input.Split(' ', 2);
                string command = parts[0];
                string argument = parts.Length > 1 ? parts[1] : null;

                var cmd = PlayerCommands.FirstOrDefault(pcmd => pcmd.Name == command); // matches command to list of player commands
                if (cmd != null)
                {
                    using (var lua = new Lua())
                    {
                        lua.DoString("os = nil; io = nil; debug = nil; package = nil");
                        // Expose necessary objects to Lua
                        lua["player"] = player;
                        lua["room"] = rooms[player.CurrentRoom];
                        lua["rooms"] = rooms;
                        lua["inventory"] = player.Inventory;
                        lua["items"] = rooms[player.CurrentRoom].Items;
                        lua["direction"] = argument;
                        lua["target"] = argument;

                        lua.DoString(cmd.LScript);
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }
            
        }

        private void SaveGameData()
        {
            Console.WriteLine("Enter the name of the save file (e.g., savegame.json): ");
                    string fileName = Console.ReadLine().Trim();
                    if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".json";
                    }
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName ?? throw new DirectoryNotFoundException("could not determine root directory");
            string gamesFolder = Path.Combine(projectRoot, "Games");
            string filePath = Path.Combine(gamesFolder, fileName);
            PlayerInventory = new List<string>();
                    foreach (var item in player.Inventory)
                    {
                        PlayerInventory.Add(item);
                        
                    }
                    Console.WriteLine(string.Join(", ", PlayerInventory)); // remove after debugging
                
            try
            {
                GameData gameData = new GameData
                {
                    Rooms = new List<Room>(rooms.Values),
                    StartingRoom = player.CurrentRoom,
                    GameRules = gameRules,
                    PlayerInventory = PlayerInventory,
                    PlayerCommands = PlayerCommands


                };
                string jsonData = JsonSerializer.Serialize(gameData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Game data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game data: {ex.Message}");
            }
        }

    }
}
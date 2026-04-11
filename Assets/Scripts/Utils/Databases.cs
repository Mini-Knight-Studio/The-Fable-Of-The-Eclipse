using System;
using Loopie;
using System.IO;
using Newtonsoft.Json;

public abstract class LocalDatabase
{
    protected string DatabaseName;

    protected string FilePath => DatabaseName + ".json";

    protected LocalDatabase(string name)
    {
        DatabaseName = name;
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        Debug.Log("Saving file: " + FilePath);
        File.WriteAllText(FilePath, json);
    }

    public void Load()
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);
        JsonConvert.PopulateObject(json, this);
    }

    public bool Exists()
    {
        return File.Exists(FilePath);
    }
}

// Global Database
public static class GlobalDatabase
{
    public static GlobalData Data { get; } = new GlobalData();

    public class GlobalData : LocalDatabase
    {
        internal GlobalData() : base("globalDB") { }

        public static MainMenuDatabase mainMenuDB = new MainMenuDatabase();
        public static SettingsDatabase settingsDB = new SettingsDatabase();

        public static void SaveGlobalDatabase()
        {
            mainMenuDB.Save();
            settingsDB.Save();
        }

        public static void LoadGlobalDatabase()
        {
            mainMenuDB.Load();
            settingsDB.Load();
        }

        public static void SaveAll()
        {
            mainMenuDB.Save();
            settingsDB.Save();
            DatabaseRegistry.SaveAll();
        }

        public static void LoadAll()
        {
            mainMenuDB.Load();
            settingsDB.Load();
            DatabaseRegistry.LoadAll();
        }
    }
}

// Registry
public static class DatabaseRegistry
{
    public static PuzzlesDatabase puzzlesDB = new PuzzlesDatabase();
    public static PlayerDatabase playerDB = new PlayerDatabase();

    public static void SaveAll()
    {
        puzzlesDB.Save();
        playerDB.Save();
    }

    public static void LoadAll()
    {
        puzzlesDB.Load();
        playerDB.Load();
    }
}

// Local Databases - Menus
public class MainMenuDatabase : LocalDatabase
{
    public MainMenuDatabase() : base("mainMenuDB") { }

    public MainMenuData MainMenu { get; } = new MainMenuData();
}

public class SettingsDatabase : LocalDatabase
{
    public SettingsDatabase() : base("settingsDB") { }

    public SettingsData Settings { get; } = new SettingsData();
}

// Local Databases - InGame
public class PuzzlesDatabase : LocalDatabase
{
    public PuzzlesDatabase() : base("puzzlesDB") { }

    public PuzzlesData Puzzles { get; } = new PuzzlesData();
}

public class PlayerDatabase : LocalDatabase
{
    public PlayerDatabase() : base("playerDB") { }

    public PlayerData Player { get; } = new PlayerData();
}


//public class ExampleLocalDataBase : LocalDatabase
//{
//    public ExampleLocalDataBase(string name) : base(name) { }

//    public int PlayerLevel;
//    public string Loadout;
//    public int Health;
//}
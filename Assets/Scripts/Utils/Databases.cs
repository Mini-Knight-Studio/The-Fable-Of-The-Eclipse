using Loopie;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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

public static class PlayerPrefs
{
    public static PlayerPrefsData Data { get; } = new PlayerPrefsData();

    public class PlayerPrefsData : LocalDatabase
    {
        internal PlayerPrefsData() : base("playerPrefsDB") { Load(); }
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public void SetString(string key, string value)
        {
            Data[key] = value;
        }

        public void SetInt(string key, int value)
        {
            SetString(key, value.ToString());
        }

        public void SetFloat(string key, float value)
        {
            SetString(key, value.ToString());
        }

        public void SetBool(string key, bool value)
        {
            SetString(key, value.ToString());
        }

        public string GetString(string key, string defaultValue = "")
        {
            return Data.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return int.TryParse(GetString(key), out var result) ? result : defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return float.TryParse(GetString(key), out var result) ? result : defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return bool.TryParse(GetString(key), out var result) ? result : defaultValue;
        }

        public bool HasKey(string key)
        {
            return Data.ContainsKey(key);
        }

        public void DeleteKey(string key)
        {
            Data.Remove(key);
        }

        public void DeleteAll()
        {
            Data.Clear();
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

public class EnemiesDataBase : LocalDatabase
{
    public EnemiesDataBase() : base("enemiesDB") { }

    public EnemiesDataBase Enemies { get; } = new EnemiesData();  
}

//public class ExampleLocalDataBase : LocalDatabase
//{
//    public ExampleLocalDataBase(string name) : base(name) { }

//    public int PlayerLevel;
//    public string Loadout;
//    public int Health;
//}
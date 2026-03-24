using System;
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

public static class GlobalDatabase
{
    public static GlobalData Data { get; } = new GlobalData();

    public class GlobalData : LocalDatabase
    {
        internal GlobalData() : base("globalDB") { }

        // Global variables (Add as much as needed)

    }
}

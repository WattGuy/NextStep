using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Text;
using System;

[System.Serializable]
public class TargetData
{

    public string type = "";
    public string otype = "";
    public int target = 0;

    public TargetData(string type, string otype, int i)
    {

        this.type = type;
        this.otype = otype;
        target = i;

    }

    public HeroType? getType()
    {

        return TypeUtils.getType(type);

    }

    public OnType? getOType()
    {

        return TypeUtils.getOType(otype);

    }

}

[System.Serializable]
public class DotData
{

    public string position = "";
    public string type = "";
    public string dtype = "";
    public string otype = "";

    public DotData(HeroType ht, DLCType dt, OnType ot, string position) {

        this.position = position;
        type = ht.ToString();
        dtype = dt.ToString();
        otype = ot.ToString();

    }

    public HeroType? getType() {

        return TypeUtils.getType(type);

    }

    public DLCType? getDType()
    {

        return TypeUtils.getDType(dtype);

    }

    public OnType? getOType()
    {

        return TypeUtils.getOType(otype);

    }

}

[System.Serializable]
public class LevelData {

    public DotData[] dots;
    public TargetData[] targets;
    public int steps;
    public int energy = 1;

    public LevelData(DotData[] dots, TargetData[] targets, int steps, int energy) {

        this.dots = dots;
        this.targets = targets;
        this.steps = steps;
        this.energy = energy;

    }

}


[System.Serializable]
public class GameData {

    public string blue = "";
    public string green = "";
    public string lightgreen = "";
    public string orange = "";
    public string pink = "";
    public int last_level = 0;
    public List<String> purchases = new List<String>();
    public int keys = 0;
    public int coins = 0;
    public int energy = 0;
    public int energy_max = 10;
    public long time = 0;
    public long complete = 0;
    public int last_reward = 0;

}

public static class Saver{

    public static string file = "save.watt";

    public static void saveLevel(LevelData d, string path) {

        FileStream stream = new FileStream(path, FileMode.Create);

        byte[] contentBytes = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(d));
        stream.Write(contentBytes, 0, contentBytes.Length);
        stream.Close();
    }

    private static void fixProblems(GameData d) {

        if (d.energy_max < 10) {

            d.energy_max = 10;
            save(d);

        }

    }

    public static LevelData loadLevel(string s)
    {
        LevelData d = null;

        try
        {
            d = (LevelData) JsonUtility.FromJson(s, typeof(LevelData));
        }
        catch (Exception e) { }

        if (d != null && d.energy <= 0) d.energy = 1;

        return d;
    }

    public static void save(GameData d) {

        BinaryFormatter f = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + file;
        FileStream stream = new FileStream(path, FileMode.Create);

        f.Serialize(stream, d);
        stream.Close();
    }

    public static GameData load() {
        string path = Application.persistentDataPath + "/" + file;

        if (File.Exists(path))
        {

            BinaryFormatter f = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData d = f.Deserialize(stream) as GameData;
            stream.Close();

            fixProblems(d);
           
            return d;

        }
        else {

            GameData d = new GameData();
            fixProblems(d);
            d.energy = d.energy_max;

            return d;

        }

    }
    
}

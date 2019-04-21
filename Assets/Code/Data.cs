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
    public int target = 0;

    public TargetData(HeroType ht, int i)
    {

        type = ht.ToString();
        target = i;

    }

    public HeroType? getType()
    {

        return TypeUtils.getType(type);

    }

}

[System.Serializable]
public class DotData
{

    public string position = "";
    public string type = "";
    public string dtype = "";

    public DotData(HeroType ht, DLCType dt, string position) {

        this.position = position;
        type = ht.ToString();
        dtype = dt.ToString();

    }

    public HeroType? getType() {

        return TypeUtils.getType(type);

    }

    public DLCType? getDType()
    {

        return TypeUtils.getDType(dtype);

    }

}

[System.Serializable]
public class LevelData {

    public DotData[] dots;
    public TargetData[] targets;
    public int steps;

    public LevelData(DotData[] dots, TargetData[] targets, int steps) {

        this.dots = dots;
        this.targets = targets;
        this.steps = steps;

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

}

public static class Saver{

    public static string file = "save.watt";

    public static void saveLevel(LevelData d, string path) {

        FileStream stream = new FileStream(path, FileMode.Create);

        byte[] contentBytes = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(d));
        stream.Write(contentBytes, 0, contentBytes.Length);
        stream.Close();
    }

    public static LevelData loadLevel(string s)
    {
        LevelData d = null;

        try
        {
            d = (LevelData) JsonUtility.FromJson(s, typeof(LevelData));
        }
        catch (Exception e) { }

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

            return d;

        }
        else {

            return new GameData();

        }

    }
    
}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameData {

    public string blue = DLCType.VERTICAL.ToString();
    public string green = DLCType.BOMB.ToString();
    public string lightgreen = DLCType.BOMB.ToString();
    public string orange = DLCType.BOMB.ToString();
    public string pink = DLCType.BOMB.ToString();

}

public static class Saver{

    public static string file = "save.watt";

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

            Debug.LogError("Save file not found in " + path);
            return new GameData();

        }

    }
    
}

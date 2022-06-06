using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerData player)
    {
        BinaryFormatter bf = new BinaryFormatter();
#if UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/" + player.Name + ".save";
#else
        string path = Application.persistentDataPath + "/" + player.Name + ".save";
#endif

        FileStream file = new FileStream(path, FileMode.Create);
        bf.Serialize(file, player);
        file.Close();
    }

    public static PlayerData LoadPlayer(string name)
    {
#if UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/" + name + ".save";
#else
        string path = Application.persistentDataPath + "/" + name + ".save";
#endif
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Debug.Log("File doesn't exist!");
        }
        return null;
    }

    public static void DeletePlayer(string name)
    {
#if UNITY_EDITOR
        string path = Application.streamingAssetsPath + "/" + name + ".save";
#else
        string path = Application.persistentDataPath + "/" + name + ".save";
#endif
        if(File.Exists(path))
        {
            File.Delete(path);

#if UNITY_EDITOR
            if (File.Exists(path + ".meta"))
            {
                File.Delete(path + ".meta");
            }
#endif
        }
        else
        {
            Debug.Log("File doesn't exist!");
        }
    }
}


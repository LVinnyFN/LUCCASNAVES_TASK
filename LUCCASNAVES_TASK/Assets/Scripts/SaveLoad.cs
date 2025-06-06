using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    private static string saveFolderPath = Application.persistentDataPath;
    private const string path = "/Save/";
    private const string extension = ".sav";

    public static void Save<T>(T data, string name) 
    {
        FileStream fileStream = null;
        string directory = saveFolderPath + path;
        string fullPath = saveFolderPath + path + name + extension;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(fullPath))
        {
            fileStream = File.OpenWrite(fullPath);
        }
        else
        {
            fileStream = File.Create(fullPath);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static bool Load<T>(string name, out T load)
    {
        FileStream fileStream = null;
        string directory = saveFolderPath + path;
        string fullPath = saveFolderPath + path + name + extension;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(fullPath))
        {
            fileStream = File.OpenRead(fullPath);
        }
        else
        {
            fileStream = File.Create(fullPath);
        }

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            object deserialized = bf.Deserialize(fileStream);
            if ((T)deserialized != null)
            {
                fileStream.Close();
                load = (T)deserialized;
                return true;
            }
            else
            {
                Debug.LogError("There was an error trying to read the save file.");
                load = default;
                return false;
            }
        }
        catch
        {
            Debug.LogError("There was an error trying to read the save file.");
            load = default;
            return false;
        }
    }

    public static void Delete(string name)
    {
        string directory = saveFolderPath + path;
        string fullPath = saveFolderPath + path + name + extension;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
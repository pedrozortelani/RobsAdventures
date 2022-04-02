using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    static string playerPath = $"{Application.persistentDataPath}/data.rob";
    static string worldPath = $"{Application.persistentDataPath}/data.world";

    public static void RestartSaveFiles()
    {
        File.Delete(playerPath);
        File.Delete(worldPath);
        SaveWorld(1, 0);
    }

    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (var stream = new FileStream(playerPath, FileMode.Create))
        {
            PlayerData data = new PlayerData(player);
            formatter.Serialize(stream, data);
        }
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(playerPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var stream = new FileStream(playerPath, FileMode.Open))
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }
        }
        else
            return null;
    }

    public static void SaveWorld(int worldIndex, int stageIndex)
    {
        if (File.Exists(worldPath))
        {
            BinaryFormatter loadformatter = new BinaryFormatter();

            using (var loadstream = new FileStream(worldPath, FileMode.Open))
            {
                WorldData currentdata = loadformatter.Deserialize(loadstream) as WorldData;

                if (currentdata.worldIndex < worldIndex && currentdata.stageIndex < stageIndex)
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    using (var stream = new FileStream(worldPath, FileMode.Create))
                    {
                        WorldData data = new WorldData(worldIndex, stageIndex);
                        formatter.Serialize(stream, data);
                    }
                }
            }
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var stream = new FileStream(worldPath, FileMode.Create))
            {
                WorldData data = new WorldData(1, 1);
                formatter.Serialize(stream, data);
            }
        }

    }

    public static WorldData LoadWorld()
    {
        if (File.Exists(worldPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var stream = new FileStream(worldPath, FileMode.Open))
            {
                WorldData data = formatter.Deserialize(stream) as WorldData;
                return data;
            }
        }
        else
            return null;

    }


}

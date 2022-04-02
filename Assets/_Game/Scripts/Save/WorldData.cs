using System;

[Serializable]
public class WorldData
{
    public DateTime timestamp;
    public int worldIndex;
    public int stageIndex;


    public WorldData(int worldIndex, int stageIndex)
    {
        timestamp = DateTime.Now;

        this.worldIndex = worldIndex;
        this.stageIndex = stageIndex;
    }
}

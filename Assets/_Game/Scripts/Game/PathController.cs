using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] GameObject[] lockedSpots = null;
    [SerializeField] int worldIndex = 0;
    
    void Start()
    {
        int index;

        if(worldIndex == 0)
        {
            WorldData data = SaveSystem.LoadWorld();
            index = data.stageIndex;
        }
        else
        {
            WorldData data = SaveSystem.LoadWorld();
            if (data.worldIndex > worldIndex)
            {
                index = 8;
            }
            else
                index = data.stageIndex;
        }
        
        for (int i = 1; i < lockedSpots.Length; i++)
        {
            if(i <= index && lockedSpots[i] != null)
            {
                lockedSpots[i].GetComponent<StageSpot>().isActive = true;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehtseStudio.SimpleWaveSystem.SO
{

    [System.Serializable]
    public class WaveSetUp
    {

        public int objectID;
        public GameObject gameObjectToSpawn;
        public int totalCountToSpawn;

    }

    [CreateAssetMenu(menuName = "Rehtse Studio Simple Wave System/Create New Wave")]
    public class WaveSystemSO : ScriptableObject
    {

        [Header("Type of object on the wave")]
        public WaveSetUp[] objectToSpawnOnThisWave;

        [Header("Amount of objects to spawn on this wave")]
        public int amountToSpawnOnThisWave;

        int totalCount;

        private void OnValidate()
        {
            TotalEnemiesCount();
        }
        public int ReturnObjectType(int i)
        {

            var nextObjectType = objectToSpawnOnThisWave[i].objectID;
            return nextObjectType;

        }

        public void TotalEnemiesCount()
        {
            totalCount = 0;

            foreach(var i in objectToSpawnOnThisWave)
            {
                totalCount += i.totalCountToSpawn;
            }
            amountToSpawnOnThisWave = totalCount;
        }

    }

}


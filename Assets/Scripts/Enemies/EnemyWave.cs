using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WaveData", menuName ="EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public float WaveTime;
    public float postWaveTime;
    public List<EnemyTypeWaveData> enemyWaves = new List<EnemyTypeWaveData>();

    public float spawnRate {get; private set;}

    [System.NonSerialized]
    int numEnemies;

    public void InitWave()
    {
        numEnemies = 0;
        foreach(EnemyTypeWaveData type in enemyWaves)
        {
            numEnemies += type.numInWave;
            type.numLeft = type.numInWave;
        }

        spawnRate = WaveTime / (float)numEnemies;
    }

    public GameObject GetNextEnemy()
    {
        if(numEnemies == 0) return null;

        float rand = Random.Range(Mathf.Epsilon, numEnemies);

        int i = Mathf.CeilToInt(rand) - 1;

        int currIdx = 0;
        foreach (EnemyTypeWaveData type in enemyWaves)
        {
            currIdx += type.numLeft;

            if(currIdx > i)
            {
                --type.numLeft;
                --numEnemies;
                return type.enemyPrefab;
            }
        }

        return null;
    }

}

[System.Serializable]
public class EnemyTypeWaveData
{
    public GameObject enemyPrefab;
    public int numInWave;

    [System.NonSerialized]
    public int numLeft;
}

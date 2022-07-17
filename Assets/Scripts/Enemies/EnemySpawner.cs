using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public GameObject End;

    public float SpawnDelay = 1f;

    public List<EnemyWave> waves;

    public int preWaveTime = 5;
    int waveTimeLeft;
    public int wave { get; private set;}

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnDelay);


        while(wave < waves.Count)
        {

            waveTimeLeft = preWaveTime;

            while (waveTimeLeft > 0)
            {
                GameManager.gameManager.uiManager.WaveIncoming(waveTimeLeft);
                yield return new WaitForSeconds(1f);
                --waveTimeLeft;
            }

            GameManager.gameManager.uiManager.WaveIncoming(waveTimeLeft);

            waves[wave].InitWave();

            GameObject prefab = waves[wave].GetNextEnemy();
            while (prefab)
            {
                GameObject newEnemyGO = Instantiate(prefab, transform.position, Quaternion.identity);
                Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
                newEnemy.navAgent.SetDestination(End.transform.position);
                newEnemy.End = End;

                yield return new WaitForSeconds(waves[wave].spawnRate);

                prefab = waves[wave].GetNextEnemy();
            }

            ++wave;

            yield return new WaitForSeconds(waves[wave-1].postWaveTime);
        }
    }
}

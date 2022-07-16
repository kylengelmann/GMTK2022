using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public GameObject End;

    public float SpawnDelay = 1f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnDelay);

        while(true)
        {
            GameObject newEnemyGO = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
            Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
            newEnemy.navAgent.SetDestination(End.transform.position);
            newEnemy.End = End;

            yield return wait;
        }
    }
}

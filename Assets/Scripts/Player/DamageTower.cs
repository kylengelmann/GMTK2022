using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTower : MonoBehaviour
{
    public float MaxRange = 2f;
    public float Cooldown = 1f;

    public Transform bulletOrigin;
    public GameObject bulletPrefab;

    private void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    IEnumerator UpdateLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(Cooldown);

        bool bDidShoot = false;
        while(true)
        {
            float minDist = float.PositiveInfinity;
            Enemy closestEnemy = null;
            foreach(Enemy enemy in FindObjectsOfType<Enemy>())
            {
                // ignore active rigidbodies because doing damage to them is weird so i don't wanna think about it
                if(!enemy.rig.isKinematic)
                {
                    continue;
                }

                float enemyDist = Vector3.ProjectOnPlane(enemy.rig.transform.position - transform.position, Vector3.up).sqrMagnitude;
                if(enemyDist < minDist && enemyDist <= MaxRange)
                {
                    minDist = enemyDist;
                    closestEnemy = enemy;
                }
            }

            if(closestEnemy)
            {
                bDidShoot = true;

                Vector3 toEnemy = closestEnemy.transform.position - bulletOrigin.position;

                Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.FromToRotation(Vector3.up, toEnemy));
            }

            yield return bDidShoot ? wait : null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTower : MonoBehaviour
{
    public float MaxRange = 2f;
    public float Cooldown = .3f;
    public float BurstCooldown = 1.5f;
    public int BurstSize = 2;

    public Transform bulletOrigin;
    public GameObject bulletPrefab;

    float hover;

    public bool bIsHovered;
    public bool bIsSelected;

    public Color hoverColor;
    public Color selectedColor;
    public float hoverSpeed = 4f;

    public Renderer renderer;

    public List<AudioClip> pews;

    private void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    private void Update()
    {
        if(bIsSelected)
        {
            renderer.material.SetColor("_EmissionColor", selectedColor);
        }
        else if((bIsHovered && hover < 1f) || (!bIsHovered && hover > 0f))
        {
            hover = Mathf.Clamp01(hover + Time.deltaTime * hoverSpeed * (bIsHovered ? 1f : -1f));
            renderer.material.SetColor("_EmissionColor", hoverColor * hover);
        }
    }

    IEnumerator UpdateLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(BurstCooldown);
        WaitForSeconds waitShort = new WaitForSeconds(Cooldown);

        int burstNum = BurstSize;

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
                if(pews.Count > 0) GameManager.gameManager.SpawnSFXAtLocation(pews[Random.Range(0, pews.Count)], bulletOrigin.position, .5f);
            }

            if(bDidShoot)
            {
                --burstNum;
                if(burstNum <= 0)
                {
                    yield return wait;
                    burstNum = BurstSize;
                }
                else
                {
                    yield return waitShort;
                }
            }
            yield return null;
        }
    }
}

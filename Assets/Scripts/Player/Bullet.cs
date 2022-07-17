using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;
    public int Damage = 1;

    private void Update()
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Enemy hitEnemy = other.GetComponentInParent<Enemy>();
        if (hitEnemy)
        {
            hitEnemy.Damage(Damage);
        }

        Destroy(gameObject);
    }
}

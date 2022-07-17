using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    public float ExplosionRange = .75f;
    public float CenterForce = 2f;
    public float upImpulseRatio = .5f;
    public float CenterMaxTorque = 4f;

    protected override void OnTriggerEnter(Collider other)
    {
        foreach(Collider collider in Physics.OverlapSphere(transform.position, ExplosionRange, 1 << LayerMask.NameToLayer("Enemy")))
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if(enemy)
            {
                Vector3 toEnemy = enemy.rig.transform.position - transform.position;

                float impulseStrength = Mathf.Lerp(CenterForce, 0f, Mathf.Clamp01(Mathf.Pow(toEnemy.magnitude / ExplosionRange, 2f)));
                Vector3 Impulse = Vector3.ProjectOnPlane(toEnemy.normalized + Vector3.ProjectOnPlane(transform.up, Vector3.up).normalized, Vector3.up).normalized * impulseStrength + Vector3.up * upImpulseRatio * impulseStrength;

                Debug.Log(Impulse);

                Vector3 AngularImpulse = Random.onUnitSphere * (1 - Mathf.Pow(Random.value, 2)) * Mathf.Lerp(CenterMaxTorque, 0f, toEnemy.magnitude / ExplosionRange);
                
                enemy.Launch(Impulse, AngularImpulse);
            }
        }

        Destroy(gameObject);
    }
}

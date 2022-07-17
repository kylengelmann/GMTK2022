using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navAgent {get; private set;}

    public Rigidbody rig {get; private set; }
    DieBase die;

    [System.NonSerialized]
    public GameObject End;

    int Health;

    public float rollDuration = .5f;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;

        rig = GetComponentInChildren<Rigidbody>();

        die = GetComponentInChildren<DieBase>();
        die.onCollision += OnCollision;
    }

    private void Start()
    {
        Health = Random.Range(1, die.GetMaxFace() + 1);

        die.transform.rotation = die.GetRotationForFace(Health);
    }

    private void Update()
    {
        if ((navAgent.isStopped) && (rig.IsSleeping() || rig.isKinematic))
        {
            rig.isKinematic = true;
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;

            transform.position = rig.transform.position;
            rig.transform.localPosition = Vector3.zero;

            navAgent.isStopped = false;
            navAgent.SetDestination(End.transform.position);

            Health = die.GetValue();

            die.RollToFace(Health, rollDuration);
        }
    }

    private void OnCollision(Collision collision)
    {
        if (rig.isKinematic)
        {
            navAgent.isStopped = true;
            rig.isKinematic = false;
            rig.AddForceAtPosition(-collision.impulse, collision.GetContact(0).point, ForceMode.Impulse);
        }
    }

    public void Launch(Vector3 Impulse, Vector3 AngularImpulse)
    {
        navAgent.isStopped = true;
        rig.isKinematic = false;
        rig.AddTorque(Impulse, ForceMode.Impulse);
        rig.AddForce(AngularImpulse, ForceMode.Impulse);
    }

    public void Damage(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        die.RollToFace(Health, rollDuration);
    }
}

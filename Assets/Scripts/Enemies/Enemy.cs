using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navAgent {get; private set;}

    Rigidbody rig;

    [System.NonSerialized]
    public GameObject End;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;

        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if((!navAgent.enabled) && (rig.IsSleeping() || rig.isKinematic))
        {
            rig.isKinematic = true;
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
            navAgent.enabled = true;
            navAgent.isStopped = false;
            navAgent.SetDestination(End.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rig.IsSleeping() || rig.isKinematic)
        {
            navAgent.enabled = false;
            rig.isKinematic = false;
            rig.AddForceAtPosition(-collision.impulse, collision.GetContact(0).point, ForceMode.Impulse);
        }
    }
}

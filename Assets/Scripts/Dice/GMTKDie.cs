using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMTKDie : MonoBehaviour
{
    public GameObject damageTowerPrefab;
    public GameObject rocketTowerPrefab;

    Rigidbody rigidbody;
    DieBase die;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        die = GetComponent<DieBase>();
    }

    void Update()
    {
        if (rigidbody.IsSleeping())
        {

            int value = die.GetValue();

            bool bRocket = value == 1 || value == 6;

            GameObject towerPrefab = bRocket ? rocketTowerPrefab : damageTowerPrefab;

            if (towerPrefab)
            {
                Instantiate(towerPrefab, Vector3.ProjectOnPlane(transform.position, Vector3.up), Quaternion.AngleAxis(Random.Range(Mathf.Epsilon, 360f), Vector3.up));
            }

            Destroy(gameObject);
        }
    }
}

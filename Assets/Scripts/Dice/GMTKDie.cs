using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMTKDie : MonoBehaviour
{
    public GameObject towerPrefab;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rigidbody.IsSleeping())
        {
            if (towerPrefab)
            {
                Instantiate(towerPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}

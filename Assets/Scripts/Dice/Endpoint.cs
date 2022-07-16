using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    DieBase die;

    int face = 20;

    float rollTime = .5f;

    float idleRotationSpeed = 36f;

    Coroutine currentRollRoutine;
    void Start()
    {
        die = GetComponent<DieBase>();
        transform.rotation = die.GetRotationForFace(face);
    }

    private void Update()
    {
        transform.rotation = Quaternion.AngleAxis(Time.deltaTime * idleRotationSpeed, Vector3.up) * transform.rotation;    
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if(!enemy)
        {
            return;
        }

        --face;
        OnFaceChanged();

        Destroy(enemy.gameObject);
    }

    void OnFaceChanged()
    {
        if(face <= 0)
        {
            return;
        }
            
        if(currentRollRoutine != null)
        {
            StopCoroutine(currentRollRoutine);
        }
        currentRollRoutine = StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        Quaternion fromRot = transform.rotation;
        Quaternion toRot = die.GetRotationForFace(face);
        float currentRollTime = 0f;
        while(currentRollTime < rollTime)
        {
            transform.rotation = Quaternion.AngleAxis(Time.deltaTime * currentRollTime, Vector3.up) * Quaternion.Slerp(fromRot, toRot, currentRollTime/rollTime);
            currentRollTime += Time.deltaTime;
            yield return null;
        }
    }
}

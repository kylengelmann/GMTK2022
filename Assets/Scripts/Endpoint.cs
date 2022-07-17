using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    DieBase die;

    public int face { get; private set;}

    float rollTime = .5f;

    float idleRotationSpeed = 36f;

    Coroutine currentRollRoutine;

    public List<AudioClip> eatClips;

    private void Awake()
    {
        face = 20;
    }

    void Start()
    {
        die = GetComponentInChildren<DieBase>();
        die.transform.rotation = die.GetRotationForFace(face);
    }

    private void Update()
    {
        transform.rotation = Quaternion.AngleAxis(Time.deltaTime * idleRotationSpeed, Vector3.up) * transform.rotation;    
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        Enemy enemy = parent.gameObject.GetComponent<Enemy>();
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
            GameManager.gameManager.GameOver();
            return;
        }

        GameManager.gameManager.SpawnSFXAtLocation(eatClips[Random.Range(0, eatClips.Count)], transform.position);
        die.RollToFace(face, rollTime);
    }
}

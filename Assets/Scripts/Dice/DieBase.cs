using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBase : MonoBehaviour
{
    List<Vector3> faces = new List<Vector3>();

    public delegate void OnCollision(Collision collision);
    public OnCollision onCollision;

    Coroutine currentRoll;

    Rigidbody rig;

    [System.NonSerialized]
    readonly float sfxCooldownTime = .5f;

    float lastSFXTime = -1f;

    private void Awake()
    {
        int i = 1;
        Transform face = null;
        do
        {
            face = transform.Find(i.ToString());
            if(face)
            {
                faces.Add(transform.InverseTransformPoint(face.position).normalized);
            }
            ++i;
        } while(face);

        rig = GetComponent<Rigidbody>();
    }

    public int GetValue()
    {
        float bestFaceDot = float.NegativeInfinity;
        int bestFace = 0;
        for(int i = 0; i < faces.Count; ++i)
        {
            float faceDot = Vector3.Dot(transform.TransformDirection(faces[i]), Vector3.up);
            if(faceDot > bestFaceDot)
            {
                bestFaceDot = faceDot;
                bestFace = i + 1;
            }
        }

        return bestFace;
    }

    public int GetMaxFace() { return faces.Count; }

    public Quaternion GetRotationForFace(int face)
    {
        Vector3 faceDir = transform.TransformDirection(faces[face-1]);
        
        return Quaternion.FromToRotation(faceDir, Vector3.up) * transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rig && !rig.isKinematic && Time.time - lastSFXTime >= sfxCooldownTime)
        {
            GameManager.gameManager.SpawnSFXAtLocation(GameManager.gameManager.dieSound, collision.GetContact(0).point);
        }

        if(onCollision != null)
        {
            onCollision(collision);
        }
    }

    public void RollToFace(int face, float rollDuration)
    {
        if(currentRoll != null)
        {
            StopCoroutine(currentRoll);
        }
        currentRoll = StartCoroutine(Roll(face, rollDuration));
    }

    IEnumerator Roll(int face, float duration)
    {
        Quaternion fromRot = transform.rotation;
        Quaternion toRot = GetRotationForFace(face);
        float currentRollTime = 0f;
        while (currentRollTime < duration)
        {
            transform.rotation = Quaternion.Slerp(fromRot, toRot, currentRollTime / duration);
            currentRollTime += Time.deltaTime;
            yield return null;
        }
    }
}

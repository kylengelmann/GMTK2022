using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBase : MonoBehaviour
{
    List<Vector3> faces = new List<Vector3>();

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

    public Quaternion GetRotationForFace(int face)
    {
        Vector3 faceDir = transform.TransformDirection(faces[face-1]);
        
        return Quaternion.FromToRotation(faceDir, Vector3.up) * transform.rotation;
    }
}

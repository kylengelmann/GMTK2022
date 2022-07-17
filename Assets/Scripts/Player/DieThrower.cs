using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieThrower : MonoBehaviour
{
    public GameObject cursorPrefab;
    public GameObject towerDiePrefab;

    Cursor cursor;

    public int diceToThrow = 4;
    public float launchFudgeFactor = .3f;
    
    void Update()
    {
        if(diceToThrow > 0 && Input.GetMouseButtonDown(0))
        {
            Vector3 cursorRelativePos = cursor.transform.localPosition;
            
            Vector3 cursor2DDirection = Vector3.ProjectOnPlane(cursorRelativePos, Vector3.up);

            float throwDist = cursor2DDirection.magnitude;

            Vector3 cursor2DRightVec = Vector3.Cross(cursor2DDirection, Vector3.up).normalized;

            Quaternion throwDirRotation = Quaternion.AngleAxis(45, cursor2DRightVec);
            Vector3 throwDir = (throwDirRotation * cursor2DDirection).normalized;

            float heightDif = cursorRelativePos.y;

            float launchSpeed = Mathf.Sqrt((-Physics.gravity.y*throwDist*throwDist)/(2*(throwDist - heightDif))) + throwDist * launchFudgeFactor;

            Debug.Log("Distance: " + throwDist.ToString() + " | speed: " + launchSpeed.ToString());

            GameObject newDieGO = Instantiate(towerDiePrefab, transform.position, Random.rotation);
            Rigidbody newDieRB = newDieGO.GetComponent<Rigidbody>();
            newDieRB.AddForce(throwDir * launchSpeed, ForceMode.VelocityChange);
            newDieRB.AddTorque(Random.onUnitSphere * Random.Range(3f, 12f), ForceMode.VelocityChange);

            --diceToThrow;
            if(diceToThrow <= 0)
            {
                GameManager.gameManager.SetIsRolling(false);
            }
        }
    }

    private void OnEnable()
    {
        GameObject cursorGO = Instantiate(cursorPrefab, transform);
        cursor = cursorGO.GetComponent<Cursor>();
    }

    private void OnDisable()
    {
        Destroy(cursor.gameObject);
        cursor = null;
    }
}
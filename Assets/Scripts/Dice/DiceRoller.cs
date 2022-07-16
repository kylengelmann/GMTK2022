using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public GameObject DiePrefab;

    public float RollHeight = 1f;

    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 mouseLocation = mouseRay.origin + mouseRay.direction * (RollHeight - mouseRay.origin.y) / mouseRay.direction.y;

        transform.position = mouseLocation;

        if(Input.GetMouseButtonDown(0) && DiePrefab)
        {

            for (int i = 0; i < 4; ++i)
            {
                GameObject die = Instantiate<GameObject>(DiePrefab, transform.position + transform.lossyScale.z * Vector3.forward + Random.insideUnitSphere, Random.rotation);
                die.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(2f, 6f) + Random.insideUnitSphere * .5f, ForceMode.VelocityChange);
                die.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(3f, 12f), ForceMode.VelocityChange);
            }
        }
    }
}

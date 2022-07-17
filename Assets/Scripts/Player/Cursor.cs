using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public float cursorHeight = 0f;

    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 mouseLocation = mouseRay.origin + mouseRay.direction * (cursorHeight-mouseRay.origin.y) / mouseRay.direction.y;

        if(Mathf.Abs(mouseLocation.x) < 10f && Mathf.Abs(mouseLocation.z) < 10f)
        {
            transform.position = mouseLocation;
        }
    }
}

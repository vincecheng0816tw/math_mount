using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragObjectRot2 : MonoBehaviour
{
    private float rotSpeed = 10;
    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        //transform.position = GetMouseWorldPos() + mOffset;
        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.up, -rotY); ;
        Debug.Log("OnMouseDrag");
    }
}

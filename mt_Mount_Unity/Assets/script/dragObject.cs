using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragObject : MonoBehaviour
{
    private Vector3 mOffset;
    
    private float mZcoord;

    void OnMouseDown()
    {

        mZcoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        Debug.Log("OnMouseDown"+ mZcoord);
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinate (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZcoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
        Debug.Log("OnMouseDrag");
    }
}

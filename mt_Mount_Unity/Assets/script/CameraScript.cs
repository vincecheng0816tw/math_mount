using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool enable = true;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;

    public float panLimitx = 50;
    public float panLimitz = 250;
    public float minY;
    public float maxY;

    private Vector3 initialPosition;
    private Vector3 offset;
    private Vector3 screenPoint;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        if (enable == false)
            return;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness )
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <=  panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            transform.eulerAngles += scrollSpeed * new Vector3(x: 11, y: Input.GetAxis("Mouse X"), z:0);

        }
        if (Input.GetMouseButtonUp(1))
        {

        }
        

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            // rot.x += scroll * scrollSpeed * .1f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, 0, panLimitx);
        pos.z = Mathf.Clamp(pos.z, 0, panLimitz);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
        transform.rotation = rot;

    }
   
    
}

using UnityEngine;

public class FollowForMouse : MonoBehaviour
{
    Ray ray;
    RaycastHit raycastHit;
    Camera mainCamera;
    bool isSelected = false;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (isSelected)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 10000) && raycastHit.collider.tag == "MoveZone")
                transform.position = new Vector3(raycastHit.point.x, -transform.localScale.y / 2, raycastHit.point.z);
        }
    }

    private void OnMouseDown() { isSelected = true; }
    private void OnMouseUp() { isSelected = false; }
}

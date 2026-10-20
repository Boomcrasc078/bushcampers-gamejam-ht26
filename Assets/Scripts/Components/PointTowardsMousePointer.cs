using UnityEngine;
using UnityEngine.InputSystem;

public class PointTowardsMousePointer : MonoBehaviour {
    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Camera camera = Camera.main;
        if (camera == null)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float cameraDistance = Vector3.Dot(transform.position - camera.transform.position, camera.transform.forward);
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, cameraDistance));
        Vector2 direction = mouseWorldPosition - transform.position;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
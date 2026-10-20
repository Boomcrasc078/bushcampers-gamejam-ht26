using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lookAheadDistance = 1.5f;
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target == null)
            return;

        transform.position = new Vector3(target.position.x, target.position.y, -10f);
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector2 lookAhead = Vector2.zero;

        if(Mouse.current != null && Camera.main != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector3 viewport = Camera.main.ScreenToViewportPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

            Vector2 offsetFromCenter = new Vector2((viewport.x - 0.5f) * 2f, (viewport.y - 0.5f) * 2f);

            lookAhead = Vector2.ClampMagnitude(offsetFromCenter, 1f) * lookAheadDistance;

            Vector3 targetPosition = new Vector3(target.position.x +lookAhead.x, target.position.y+lookAhead.y,-10f);


            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        }
    }
}

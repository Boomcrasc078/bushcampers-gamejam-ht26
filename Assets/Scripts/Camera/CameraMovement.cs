using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lookAheadDistance = 1.5f;
    [SerializeField] private float ticksPerSecond = 1f;
    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CameraTicker());
    }

    IEnumerator CameraTicker()
    {
        while (true)
        {
            var getTarget = FindAnyObjectByType<CameraTarget>();
            if (getTarget != null)
            {
                target = getTarget.transform;
            }
            yield return new WaitForSeconds(1 / ticksPerSecond);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        var lookAhead = Vector2.zero;
        if (Mouse.current != null && Camera.main != null)
        {
            var camera = Camera.main;
            var mousePosition = Mouse.current.position.ReadValue();
            var cameraDistance = Vector3.Dot(
                target.position - camera.transform.position,
                camera.transform.forward);
            var mouseWorldPosition = camera.ScreenToWorldPoint(
                new Vector3(mousePosition.x, mousePosition.y, cameraDistance));
            var directionToMouse = (Vector2)(mouseWorldPosition - target.position);
            if (directionToMouse.sqrMagnitude > 0f)
            {
                lookAhead = directionToMouse.normalized * lookAheadDistance;
            }
        }

        var targetPosition = new Vector3(
            target.position.x + lookAhead.x,
            target.position.y + lookAhead.y,
            -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }
}

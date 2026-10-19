using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lookAheadDistance = 1.5f;
    [SerializeField] private float ticksPerSecond = 1f;
    private Transform target;
    private Vector3 previousTargetPosition;
    private bool hasPreviousTargetPosition;
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

        var movement = hasPreviousTargetPosition
            ? (Vector2)(target.position - previousTargetPosition)
            : Vector2.zero;
        previousTargetPosition = target.position;
        hasPreviousTargetPosition = true;

        var lookAhead = movement.sqrMagnitude > 0f
            ? movement.normalized * lookAheadDistance
            : Vector2.zero;
        var targetPosition = new Vector3(
            target.position.x + lookAhead.x,
            target.position.y + lookAhead.y,
            -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }
}

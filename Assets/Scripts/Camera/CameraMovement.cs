using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
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
        transform.position = Vector3.Lerp(new Vector3(target.position.x, target.position.y, -10), transform.position, Time.deltaTime * speed / Vector2.Distance(target.position, transform.position));
    }
}

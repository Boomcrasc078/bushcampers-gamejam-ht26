using UnityEngine;

public class PointTowardsVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;

    // Update is called once per frame
    void Update()
    {
        if (rigidbody2D.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rigidbody2D.linearVelocity.y, rigidbody2D.linearVelocity.x) * Mathf.Rad2Deg + -90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}

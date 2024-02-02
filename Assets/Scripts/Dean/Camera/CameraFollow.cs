using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public float smoothSpeed = 0.125f; // How quickly the camera follows the target
    public Vector3 offset; // The offset from the target

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.position = new Vector3(transform.position.x, transform.position.y, -10); // Ensure the camera is at a consistent z-depth
        }
    }
}

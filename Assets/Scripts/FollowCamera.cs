using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // This should be the player's unit transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 targetRotation; // Specify the target rotation in Euler angles

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Calculate the target rotation as a Quaternion
            Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, smoothSpeed);
        }
    }
}

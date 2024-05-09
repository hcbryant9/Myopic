using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform pivotPoint;  // The point around which the camera will rotate
    public float rotationSpeed = 1.0f;
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the pivot point

    private void Update()
    {
        if (pivotPoint != null)
        {
            // Calculate the desired position based on the pivot point and offset
            Vector3 desiredPosition = pivotPoint.position + offset;

            // Set the camera's position to the desired position
            transform.position = desiredPosition;

            // Rotate the camera around the pivot point
            transform.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Pivot point not assigned to CameraRotationAroundPoint script!");
        }
    }
}

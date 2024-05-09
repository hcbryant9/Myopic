using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FollowCamera followCamera;                     // Reference to the grid-based camera script
    public RotateCamera rotateCamera;     // Reference to the rotation camera script

    private void Start()
    {
        // Ensure only one camera is active at start (choose default camera)
        followCamera.enabled = true;
        rotateCamera.enabled = false;
    }

    private void Update()
    {
        // Toggle between cameras based on player input (e.g., UI button press)
        if (Input.GetKeyDown(KeyCode.C)) // Example key (replace with your UI button input)
        {
            ToggleCamera();
        }
    }

    public void ToggleCamera()
    {
        // Toggle the active state of cameras
        followCamera.enabled = !followCamera.enabled;
        rotateCamera.enabled = !rotateCamera.enabled;
    }
}

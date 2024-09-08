using UnityEngine;

public class PlaceCamera : MonoBehaviour
{
    public Transform target;
    public float viewAngle = 45f;
    public float cameraHeightMultiplier = 1.2f;
    public float rotationSpeed = 50f;
    public float heightFactor = 1.5f;
    public float zoomSpeed = 10f;  // Speed at which the camera zooms in and out
    public float minZoomDistance = 5f;  // Minimum distance of the camera from the target
    public float maxZoomDistance = 50f;  // Maximum distance of the camera from the target

    private Vector3 offset;
    private float currentZoomDistance;  // Current distance from the camera to the target

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
            currentZoomDistance = offset.magnitude;  // Initialize the zoom distance
        }
    }


    void LateUpdate()
    {
        if (target != null)
        {
            Bounds bounds = CalculateBounds(target);
            RotateCamera(bounds);
            HandleZoomInput();
            HandleAngleInput();  // Handle angle adjustments
            AdjustCameraPosition(bounds);
        }
    }


    void HandleZoomInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentZoomDistance = Mathf.Max(minZoomDistance, currentZoomDistance - zoomSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentZoomDistance = Mathf.Min(maxZoomDistance, currentZoomDistance + zoomSpeed * Time.deltaTime);
        }
    }

    void HandleAngleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            viewAngle = Mathf.Max(10f, viewAngle - rotationSpeed * Time.deltaTime);  // Prevent the angle from going too low
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            viewAngle = Mathf.Min(80f, viewAngle + rotationSpeed * Time.deltaTime);  // Prevent the angle from going too high
        }
    }



    Bounds CalculateBounds(Transform target)
    {
        var bounds = new Bounds(target.position, Vector3.zero);
        // Include child object bounds
        foreach (Renderer renderer in target.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    void RotateCamera(Bounds bounds)
    {
        // Handle input to rotate left or right
        float rotationInput = Input.GetAxis("Horizontal");  // A/D keys or Left/Right arrows
        if (rotationInput != 0)
        {
            transform.RotateAround(bounds.center, Vector3.up, rotationSpeed * rotationInput * Time.deltaTime);
            // Update the offset after rotation
            offset = transform.position - bounds.center;
        }
    }

    void AdjustCameraPosition(Bounds bounds)
    {
        float height = currentZoomDistance * heightFactor;  // Calculate height based on zoom distance

        // Adjust the camera position maintaining the calculated distance
        transform.position = bounds.center + offset.normalized * currentZoomDistance;
        // Increase the Y position by the height factor calculation
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        // Maintain a fixed viewing angle
        transform.rotation = Quaternion.Euler(viewAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

}

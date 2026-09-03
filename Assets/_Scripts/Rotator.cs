using UnityEngine;
using UnityEngine.InputSystem;

public class Rotator : MonoBehaviour
{
    public InputActionProperty rotateButton; // Assign XR button in Inspector
    public float rotationSpeed = 90f;       // Degrees per second
    public Vector3 rotationAxis = Vector3.forward; // Z-axis spin by default

    private bool isRotating = false;

    void OnEnable()
    {
        rotateButton.action.Enable();
    }

    void OnDisable()
    {
        rotateButton.action.Disable();
    }

    void Update()
    {
        // Toggle rotation when button is pressed
        if (rotateButton.action.WasPressedThisFrame())
        {
            isRotating = !isRotating; // Start/stop rotation
        }

        if (isRotating)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}

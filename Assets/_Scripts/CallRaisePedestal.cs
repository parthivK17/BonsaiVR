using UnityEngine;
using UnityEngine.InputSystem;

public class CallRaisePedestal : MonoBehaviour
{
    public RaisePedestal raiseScript;       // Reference to your grow script
    public InputActionProperty growButton;  // Assign the VR button action

    void OnEnable()
    {
        growButton.action.Enable();
    }

    void OnDisable()
    {
        growButton.action.Disable();
    }

    void Update()
    {
        // Check if the VR button was pressed this frame
        if (growButton.action.WasPressedThisFrame())
        {
            if (raiseScript != null)
                raiseScript.Grow();
        }
    }
}
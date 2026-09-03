using UnityEngine;
using UnityEngine.InputSystem;

public class ResetPedestal : MonoBehaviour
{
    public RaisePedestal raiseScript;       // Assign your script
    public InputActionProperty resetButton; // Assign VR button action

    void OnEnable()
    {
        resetButton.action.Enable();
    }

    void OnDisable()
    {
        resetButton.action.Disable();
    }

    void Update()
    {
        if (resetButton.action.WasPressedThisFrame())
        {
            if (raiseScript != null)
                raiseScript.ResetSize();
        }
    }
}
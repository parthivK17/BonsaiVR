using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SitInChair : MonoBehaviour
{
    public InputActionProperty sitButton;  // Assign in Inspector
    public Transform chairSeatAnchor;      // SeatAnchor empty object
    public GameObject menuToHide;          // Menu to hide when sitting
    public GameObject menuToShow;          // Menu to show when sitting
    public XRController leftController;    // Optional, disable movement
    public XRController rightController;   // Optional, disable movement

    private bool isSeated = false;
    private bool hasSeatedOnce = false;    // Tracks if seated at least once

    void OnEnable()
    {
        sitButton.action.Enable();
    }

    void OnDisable()
    {
        sitButton.action.Disable();
    }

    void Update()
    {
        if (sitButton.action.WasPressedThisFrame())
        {
            ToggleSit();
        }
    }

    void ToggleSit()
    {
        isSeated = !isSeated;

        if (isSeated)
        {
            // Move XR Rig to chair
            Transform xrOrigin = Camera.main.transform.parent; // assumes Camera inside Camera Offset
            xrOrigin.position = chairSeatAnchor.position;
            xrOrigin.rotation = chairSeatAnchor.rotation;

            // Run menu swap only on the first sit
            if (!hasSeatedOnce)
            {
                if (menuToHide != null) menuToHide.SetActive(false);
                if (menuToShow != null) menuToShow.SetActive(true);

                hasSeatedOnce = true;
            }

            // Optionally disable movement
            //if (leftController) leftController.enableInputActions = false;
            //if (rightController) rightController.enableInputActions = false;
        }
        else
        {
            // Re-enable movement
            //if (leftController) leftController.enableInputActions = true;
            //if (rightController) rightController.enableInputActions = true;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleControlMenu : MonoBehaviour
{
    public GameObject menu;             // Assign your menu in the Inspector
    public InputActionProperty toggleButton; // Assign the input action for the button

    void OnEnable()
    {
        toggleButton.action.Enable();
    }

    void OnDisable()
    {
        toggleButton.action.Disable();
    }

    void Update()
    {
        if (toggleButton.action.WasPressedThisFrame())
        {
            if (menu != null)
            {
                // Toggle the menu visibility
                menu.SetActive(!menu.activeSelf);
            }
        }
    }
}
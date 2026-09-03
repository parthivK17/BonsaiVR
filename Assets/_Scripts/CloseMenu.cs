using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    [Header("Menu to Close")]
    public GameObject menuToClose;

    public void Close()
    {
        if (menuToClose != null)
        {
            menuToClose.SetActive(false);
        }
    }
}
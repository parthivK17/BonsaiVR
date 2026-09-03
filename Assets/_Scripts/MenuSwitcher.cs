using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [Header("Menu to Hide")]
    public GameObject menuToHide;
    [Header("Menu to Show")]
    public GameObject menuToShow;

    public void SwitchMenus()
    {
        if (menuToHide != null) menuToHide.SetActive(false);
        if (menuToShow != null) menuToShow.SetActive(true);
    }
}
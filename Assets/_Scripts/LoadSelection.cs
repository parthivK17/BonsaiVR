using UnityEngine;

public class LoadSelection : MonoBehaviour
{
    [Header("Parent Container")]
    public GameObject container; // The parent object that holds all the children

    [Header("Child To Show")]
    public GameObject childToShow; // The one child you want visible

    public void ShowSelectedChild()
    {
        if (container == null || childToShow == null) return;

        // Hide all children
        foreach (Transform child in container.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Show the desired child
        childToShow.SetActive(true);
    }
}
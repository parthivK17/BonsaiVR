using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject currentVisibleObject; // public, not static
    public GameObject objectA;
    public GameObject objectB;

    public void ShowObject(GameObject objToShow)
    {
        if (currentVisibleObject != null)
            currentVisibleObject.SetActive(false);

        objToShow.SetActive(true);
        currentVisibleObject = objToShow;
    }
}
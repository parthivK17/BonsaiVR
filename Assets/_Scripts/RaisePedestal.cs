using UnityEngine;
using System.Collections;

public class RaisePedestal : MonoBehaviour
{
    [Header("References")]
    public Transform objectToGrow;       // Base object
    public Transform objectOnTop;        // Object sitting on top

    [Header("Growth Settings")]
    public Vector3 growthAmount = new Vector3(0f, 0.5f, 0f); // Growth per button press
    public float growSpeed = 0.5f;         // Units per second
    public float maxHeight = 1.5f;         // Maximum Y-scale of the base

    private bool isAnimating = false;
    private Vector3 initialScale;
    private Vector3 initialTopPosition;

    void Start()
    {
        if (objectToGrow != null)
            initialScale = objectToGrow.localScale;

        if (objectOnTop != null)
            initialTopPosition = objectOnTop.position;
    }

    public void Grow()
    {
        if (!isAnimating && objectToGrow != null && objectOnTop != null)
        {
            float allowedY = Mathf.Min(growthAmount.y, maxHeight - objectToGrow.localScale.y);
            if (allowedY > 0f)
            {
                Vector3 allowedGrowth = new Vector3(growthAmount.x, allowedY, growthAmount.z);
                StartCoroutine(SmoothAnimate(objectToGrow.localScale + allowedGrowth, objectOnTop.position + new Vector3(0f, allowedY, 0f)));
            }
        }
    }

    public void ResetSize()
    {
        if (!isAnimating && objectToGrow != null && objectOnTop != null)
        {
            StartCoroutine(SmoothAnimate(initialScale, initialTopPosition));
        }
    }

    private IEnumerator SmoothAnimate(Vector3 targetScale, Vector3 targetTopPosition)
    {
        isAnimating = true;

        while ((objectToGrow.localScale - targetScale).magnitude > 0.001f)
        {
            objectToGrow.localScale = Vector3.MoveTowards(objectToGrow.localScale, targetScale, growSpeed * Time.deltaTime);
            objectOnTop.position = Vector3.MoveTowards(objectOnTop.position, targetTopPosition, growSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure final exact values
        objectToGrow.localScale = targetScale;
        objectOnTop.position = targetTopPosition;

        isAnimating = false;
    }
}
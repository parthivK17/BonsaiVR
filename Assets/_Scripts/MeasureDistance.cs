using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // For TextMeshPro
using System.Collections; // Needed for IEnumerator

public class MeasureDistance : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;

    [Header("Input")]
    public InputActionProperty selectPointButton;

    [Header("Visualization")]
    public GameObject pointMarkerPrefab;
    private GameObject firstMarker;
    private GameObject secondMarker;
    private LineRenderer lineRenderer;

    [Header("Distance Label")]
    public TextMeshPro distanceLabelPrefab; // Assign a TextMeshPro prefab here
    private TextMeshPro distanceLabelInstance;

    private Vector3? firstPoint = null;
    private Vector3? secondPoint = null;

    private Coroutine clearCoroutine;

    void Awake()
    {
        // Line Renderer setup
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void OnEnable()
    {
        selectPointButton.action.Enable();
    }

    void OnDisable()
    {
        selectPointButton.action.Disable();
    }

    void Update()
    {
        if (selectPointButton.action.WasPressedThisFrame())
        {
            Transform activeController = rightController != null ? rightController : leftController;
            if (activeController == null) return;

            Vector3 pointPosition = activeController.position;

            if (firstPoint == null)
            {
                firstPoint = pointPosition;
                if (firstMarker != null) Destroy(firstMarker);
                firstMarker = SpawnMarker(pointPosition);
            }
            else if (secondPoint == null)
            {
                secondPoint = pointPosition;
                if (secondMarker != null) Destroy(secondMarker);
                secondMarker = SpawnMarker(pointPosition);

                float distance = Vector3.Distance(firstPoint.Value, secondPoint.Value);
                Debug.Log("Distance between points: " + distance + " meters");

                DrawLine(distance);

                // Start auto-clear countdown
                if (clearCoroutine != null) StopCoroutine(clearCoroutine);
                clearCoroutine = StartCoroutine(ClearAfterDelay(10f)); // 10 seconds
            }
            else
            {
                // Reset manually if a third point is placed
                ResetMeasurement();
                firstPoint = pointPosition;
                firstMarker = SpawnMarker(pointPosition);
            }
        }

        // Keep label facing the camera
        if (distanceLabelInstance != null)
        {
            distanceLabelInstance.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }

    GameObject SpawnMarker(Vector3 position)
    {
        if (pointMarkerPrefab != null)
        {
            return Instantiate(pointMarkerPrefab, position, Quaternion.identity);
        }
        return null;
    }

    void DrawLine(float distance)
    {
        if (firstPoint != null && secondPoint != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, firstPoint.Value);
            lineRenderer.SetPosition(1, secondPoint.Value);

            // Position label at midpoint
            Vector3 midPoint = (firstPoint.Value + secondPoint.Value) / 2f;

            if (distanceLabelInstance == null)
            {
                distanceLabelInstance = Instantiate(distanceLabelPrefab, midPoint, Quaternion.identity);
            }

            //Divide by 3 to account for scaling of tree models
            float distanceScaled = distance / 3;

            // Convert distance to millimeters
            float distanceMM = distanceScaled * 1000f;
            distanceLabelInstance.text = distanceMM.ToString("F0") + " mm";
            //distanceLabelInstance.text = distance.ToString("F2") + " m";
            distanceLabelInstance.transform.position = midPoint;
        }
    }

    IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetMeasurement();
    }

    void ResetMeasurement()
    {
        firstPoint = null;
        secondPoint = null;

        if (firstMarker != null) Destroy(firstMarker);
        if (secondMarker != null) Destroy(secondMarker);
        if (distanceLabelInstance != null) Destroy(distanceLabelInstance.gameObject);

        lineRenderer.enabled = false;
    }
}
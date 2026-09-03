using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class RayMeasureDistance : MonoBehaviour
{
    [Header("XR Ray Interactor")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRayInteractor;

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
            Vector3? hitPoint = GetRightRayHit();
            if (hitPoint == null) return;

            Vector3 pointPosition = hitPoint.Value;

            if (firstPoint == null)
            {
                // First point
                firstPoint = pointPosition;
                if (firstMarker != null) Destroy(firstMarker);
                firstMarker = SpawnMarker(pointPosition);
            }
            else if (secondPoint == null)
            {
                // Second point
                secondPoint = pointPosition;
                if (secondMarker != null) Destroy(secondMarker);
                secondMarker = SpawnMarker(pointPosition);

                float distance = Vector3.Distance(firstPoint.Value, secondPoint.Value);
                Debug.Log("Distance between points: " + distance + " meters");

                DrawLine(distance);
            }
            else
            {
                // Reset
                firstPoint = pointPosition;
                secondPoint = null;

                if (firstMarker != null) Destroy(firstMarker);
                if (secondMarker != null) Destroy(secondMarker);
                if (distanceLabelInstance != null) Destroy(distanceLabelInstance.gameObject);

                lineRenderer.enabled = false;
                firstMarker = SpawnMarker(pointPosition);
            }
        }

        // Keep label facing the camera
        if (distanceLabelInstance != null && Camera.main != null)
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

            // Divide by 3 to account for scaling of tree models (optional)
            float distanceScaled = distance / 3;

            // Convert to millimeters
            float distanceMM = distanceScaled * 1000f;
            distanceLabelInstance.text = distanceMM.ToString("F0") + " mm";
            distanceLabelInstance.transform.position = midPoint;
        }
    }

    Vector3? GetRightRayHit()
    {
        if (rightRayInteractor != null)
        {
            if (rightRayInteractor.TryGetHitInfo(out Vector3 pos, out _, out _, out bool valid) && valid)
            {
                return pos;
            }
        }
        return null;
    }
}
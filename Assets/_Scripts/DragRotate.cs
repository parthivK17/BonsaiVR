using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DragRotate : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public float rotationSpeed = 200f;
    public Vector3 rotationAxis = Vector3.forward;

    private Transform interactorTransform;
    private Vector3 lastInteractorPosition;
    private bool isGrabbed = false;

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(StartGrab);
        grabInteractable.selectExited.AddListener(EndGrab);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(StartGrab);
        grabInteractable.selectExited.RemoveListener(EndGrab);
    }

    void StartGrab(SelectEnterEventArgs args)
    {
        interactorTransform = (args.interactorObject as MonoBehaviour)?.transform;
        if (interactorTransform != null)
        {
            lastInteractorPosition = interactorTransform.position;
            isGrabbed = true;
        }
    }

    void EndGrab(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactorTransform = null;
    }

    void Update()
    {
        if (isGrabbed && interactorTransform != null)
        {
            Vector3 delta = interactorTransform.position - lastInteractorPosition;

            // Convert delta into local space for more predictable rotation
            Vector3 localDelta = transform.InverseTransformDirection(delta);

            float rotateAmount = localDelta.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis, rotateAmount, Space.Self);

            lastInteractorPosition = interactorTransform.position;
        }
    }
}
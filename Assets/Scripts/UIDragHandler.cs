using UnityEngine;

/// <summary>
/// Enables dragging a UI image (or any RectTransform) using the legacy Input system.
/// Attach this to the Image you want to make draggable.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIDragHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera canvasCamera;
    private RectTransform referenceRect;
    private Vector2 pointerOffset;
    private bool isDragging;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas == null)
        {
            Debug.LogWarning($"{nameof(UIDragHandler)} requires the object to be inside a Canvas.", this);
            return;
        }

        referenceRect = rectTransform.parent as RectTransform;
        if (referenceRect == null)
        {
            referenceRect = parentCanvas.transform as RectTransform;
        }

        // Only ScreenSpace-Camera needs an explicit camera for the screen-to-world conversion.
        canvasCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceCamera ? parentCanvas.worldCamera : null;
    }

    private void Update()
    {
        if (parentCanvas == null || referenceRect == null)
        {
            return;
        }

        if (!isDragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryBeginDrag(Input.mousePosition);
            }

            return;
        }

        if (!Input.GetMouseButton(0))
        {
            isDragging = false;
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                referenceRect,
                Input.mousePosition,
                canvasCamera,
                out var parentLocalPoint))
        {
            rectTransform.anchoredPosition = parentLocalPoint + pointerOffset;
        }
    }

    private void TryBeginDrag(Vector3 pointerPosition)
    {
        if (referenceRect == null)
        {
            return;
        }

        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pointerPosition, canvasCamera))
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                referenceRect,
                pointerPosition,
                canvasCamera,
                out var parentLocalPoint))
        {
            pointerOffset = rectTransform.anchoredPosition - parentLocalPoint;
            isDragging = true;
        }
    }
}

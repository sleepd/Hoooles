using UnityEngine;

/// <summary>
/// Syncs this GameObject's position with a UI RectTransform by projecting
/// the UI's screen position onto a world plane (default z = 0).
/// </summary>
[RequireComponent(typeof(Transform))]
public class UIPositionFollower : MonoBehaviour
{
    [SerializeField]
    private RectTransform sourceRect;

    [SerializeField]
    private Canvas sourceCanvas;

    [SerializeField]
    private float worldZ = 0f;

    private void Reset()
    {
        if (sourceCanvas == null)
        {
            sourceCanvas = GetComponentInParent<Canvas>();
        }
    }

    private void LateUpdate()
    {
        if (sourceRect == null)
        {
            return;
        }

        if (sourceCanvas == null)
        {
            sourceCanvas = sourceRect.GetComponentInParent<Canvas>();
            if (sourceCanvas == null)
            {
                return;
            }
        }

        Vector3 worldPoint = GetProjectedWorldPoint(sourceRect, sourceCanvas);
        worldPoint.z = worldZ;
        transform.position = worldPoint;
    }

    private static Vector3 GetProjectedWorldPoint(RectTransform rectTransform, Canvas canvas)
    {
        Camera eventCamera = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            eventCamera = canvas.worldCamera;
        }

        if (eventCamera == null)
        {
            eventCamera = Camera.main;
        }

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(eventCamera, rectTransform.position);

        if (eventCamera == null)
        {
            Vector3 worldPointOnPlane = ScreenToWorldOnPlane(screenPoint);
            return worldPointOnPlane;
        }

        var ray = eventCamera.ScreenPointToRay(screenPoint);
        if (Mathf.Approximately(ray.direction.z, 0f))
        {
            return ScreenToWorldOnPlane(screenPoint);
        }

        var plane = new Plane(Vector3.forward, Vector3.zero);
        if (plane.Raycast(ray, out var enter))
        {
            return ray.GetPoint(enter);
        }

        return ScreenToWorldOnPlane(screenPoint);
    }

    private static Vector3 ScreenToWorldOnPlane(Vector2 screenPoint)
    {
        var plane = new Plane(Vector3.forward, Vector3.zero);
        var ray = new Ray(new Vector3(screenPoint.x, screenPoint.y, -1000f), Vector3.forward);
        if (plane.Raycast(ray, out var enter))
        {
            return ray.GetPoint(enter);
        }

        return new Vector3(screenPoint.x, screenPoint.y, 0f);
    }
}

using UnityEngine;
using UnityEngine.UI;
public class WallController : MonoBehaviour
{
    [SerializeField] Image _hitpoint;
    [SerializeField] WallMovement _wallMovement;

    void Start()
    {
        if (_hitpoint != null)
        {
            _hitpoint.enabled = false;
        }
    }

    void Update()
    {
        if (transform.localPosition.z < -1f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        _wallMovement.Stop();
        MoveHitpointToCollision(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        _wallMovement.Stop();
        MoveHitpointToTrigger(other);
    }




    private void MoveHitpointToCollision(Collision collision)
    {
        if (_hitpoint == null || collision.contactCount == 0)
        {
            return;
        }

        var contact = collision.GetContact(0);
        var canvas = _hitpoint.canvas;
        if (canvas == null)
        {
            return;
        }

        var canvasRect = canvas.transform as RectTransform;
        if (canvasRect == null)
        {
            return;
        }

        var worldCamera = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        if (worldCamera == null)
        {
            return;
        }

        var screenPoint = worldCamera.WorldToScreenPoint(contact.point);
        var eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                eventCamera,
                out var localPoint))
        {
            _hitpoint.rectTransform.anchoredPosition = localPoint;
            _hitpoint.enabled = true;
        }
    }

    private void MoveHitpointToTrigger(Collider other)
    {
        if (_hitpoint == null || other == null)
        {
            return;
        }

        var canvas = _hitpoint.canvas;
        if (canvas == null)
        {
            return;
        }

        var canvasRect = canvas.transform as RectTransform;
        if (canvasRect == null)
        {
            return;
        }

        var worldCamera = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        if (worldCamera == null)
        {
            return;
        }

        var contactPoint = other.ClosestPoint(transform.position);
        var screenPoint = worldCamera.WorldToScreenPoint(contactPoint);
        var eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                eventCamera,
                out var localPoint))
        {
            _hitpoint.rectTransform.anchoredPosition = localPoint;
            _hitpoint.enabled = true;
        }
    }
}

using UnityEngine;

/// <summary>
/// Rotates a UI RectTransform when pressing Q and E (legacy input system).
/// Attach this script to the UI element you want to rotate.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIRotateHandler : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 120f; // Degrees per second.

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float direction = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            direction += 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            direction -= 1f;
        }

        if (Mathf.Approximately(direction, 0f))
        {
            return;
        }

        float deltaRotation = direction * rotationSpeed * Time.unscaledDeltaTime;
        rectTransform.Rotate(0f, 0f, deltaRotation);
    }
}

using UnityEngine;

/// <summary>
/// Synchronizes this GameObject's rotation with a referenced UI RectTransform.
/// Use the axis mask to control which axes are copied (defaults to Z only).
/// </summary>
[RequireComponent(typeof(Transform))]
public class UIRotationFollower : MonoBehaviour
{
    [SerializeField]
    private RectTransform sourceRect;

    [SerializeField]
    private bool readLocalRotation;

    [SerializeField]
    private bool applyAsLocalRotation;

    [SerializeField]
    private Vector3 axisMask = new Vector3(0f, 0f, 1f);

    [SerializeField]
    private Vector3 rotationOffset;

    private void LateUpdate()
    {
        if (sourceRect == null)
        {
            return;
        }

        Vector3 sourceEuler = readLocalRotation ? sourceRect.localEulerAngles : sourceRect.eulerAngles;
        Vector3 maskedEuler = Vector3.Scale(sourceEuler, axisMask);
        Vector3 targetEuler = maskedEuler + rotationOffset;
        Quaternion targetRotation = Quaternion.Euler(targetEuler);

        if (applyAsLocalRotation)
        {
            transform.localRotation = targetRotation;
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }
}

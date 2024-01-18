using UnityEngine;

public class RunPointLine : MonoBehaviour
{
    internal enum ScreenPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField] ScreenPosition _screenPosition;
    [SerializeField] float _distanceFromScreenEdge = 1f;

    void Start()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        float halfWidth = transform.localScale.x / 2.0f;
        float halfHeight = transform.localScale.y / 2.0f;

        Vector3 newPosition = transform.position;

        switch (_screenPosition)
        {
            case ScreenPosition.Top:
                newPosition.y = mainCamera.pixelHeight - _distanceFromScreenEdge;
                break;
            case ScreenPosition.Bottom:
                newPosition.y = _distanceFromScreenEdge;
                break;
            case ScreenPosition.Left:
                newPosition.x = _distanceFromScreenEdge;
                break;
            case ScreenPosition.Right:
                newPosition.x = mainCamera.pixelWidth - _distanceFromScreenEdge;
                break;
        }

        newPosition = mainCamera.ScreenToWorldPoint(new Vector3(newPosition.x, newPosition.y, mainCamera.nearClipPlane));
        transform.position = new Vector3(newPosition.x + halfWidth, (_screenPosition == ScreenPosition.Left || _screenPosition == ScreenPosition.Right) ? 0 : newPosition.y + halfHeight, transform.position.z);
    }
}

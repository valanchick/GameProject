using UnityEngine;

public class CameraBoundsController : MonoBehaviour
{
    [Header("Границы уровня")]
    public Vector2 minBounds;
    public Vector2 maxBounds; 

    [Header("Цель слежения")]
    public Transform target;

    private void LateUpdate()
    {
        if (target == null) return;

        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float clampedX = Mathf.Clamp(
            target.position.x,
            minBounds.x + cameraWidth,  
            maxBounds.x - cameraWidth
        );

        float clampedY = Mathf.Clamp(
            target.position.y,
            minBounds.y + cameraHeight, 
            maxBounds.y - cameraHeight
        );

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
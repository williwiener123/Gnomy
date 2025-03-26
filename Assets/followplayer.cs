using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign your player here
    public float smoothSpeed = 5f;  // Adjust for smoothness
    public float yOffset = 0f;  // Adjust if you want to offset the Y position

    private void LateUpdate()
    {
        if (player == null)
            return;

        // Keep the camera at a fixed Y position while following X
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y + yOffset, transform.position.z);

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}

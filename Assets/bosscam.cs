using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    public float zoneRadius = 5f;
    public float zoomOutSize = 7f;
    public float zoomOutFOV = 80f;
    public float transitionSpeed = 2f;
    public BossHealthBar bossHealthBar; // Hälsobaren

    private Camera mainCamera;
    private Transform player;
    private float originalSize;
    private float originalFOV;
    private bool playerInZone = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera.orthographic)
            originalSize = mainCamera.orthographicSize;
        else
            originalFOV = mainCamera.fieldOfView;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            bool shouldZoom = distance <= zoneRadius;

            if (shouldZoom && !playerInZone)
            {
                playerInZone = true;
                bossHealthBar.ShowHealthBar(true); // Visa hälsobaren
            }
            else if (!shouldZoom && playerInZone)
            {
                playerInZone = false;
                bossHealthBar.ShowHealthBar(false); // Dölj hälsobaren om spelaren lämnar zonen
            }

            float targetSize = playerInZone ? zoomOutSize : originalSize;
            float targetFOV = playerInZone ? zoomOutFOV : originalFOV;

            if (mainCamera.orthographic)
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * transitionSpeed);
            else
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * transitionSpeed);
        }
    }
}

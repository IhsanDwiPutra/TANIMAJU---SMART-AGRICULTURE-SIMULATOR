using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform playerTransform; // Taruh objek Player kamu di sini

    [Header("Camera Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -5f); // Jarak kamera dari Player
    [SerializeField] private float smoothSpeed = 5f; // Kelembutan gerakan kamera

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // Tentukan posisi target kamera yang diinginkan
        Vector3 targetPosition = playerTransform.position + offset;

        // Gerakkan kamera ke posisi target secara lembut (smooth interpolation)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Paksa kamera untuk selalu melirik ke arah karakter Player
        transform.LookAt(playerTransform.position + Vector3.up * 0.5f);
    }
}
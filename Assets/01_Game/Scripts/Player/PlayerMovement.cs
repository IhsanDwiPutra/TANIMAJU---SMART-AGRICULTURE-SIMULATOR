using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 720f; // Kecepatan berputar karakter

    private CharacterController characterController;
    private Vector3 moveDirection;

    void Start()
    {
        // Ambil komponen Character Controller yang ada di objek ini
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Ambil Input dari Keyboard (WASD / Arrow Keys)
        float horizontal = Input.GetAxisRaw("Horizontal"); // Tombol A/D atau Kiri/Kanan
        float vertical = Input.GetAxisRaw("Vertical");     // Tombol W/S atau Atas/Bawah

        // 2. Kalkulasikan arah gerakan berdasarkan sumbu X dan Z
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Jika ada tombol yang dipencet, gerakkan dan putar karakter
        if (moveDirection.magnitude >= 0.1f)
        {
            // Hitung sudut rotasi agar karakter menghadap ke arah jalan
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            // Gerakkan karakter menggunakan Character Controller
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // 4. Efek Gravitasi Sederhana agar karakter tidak melayang
        Vector3 gravityMove = new Vector3(0f, -9.81f * Time.deltaTime, 0f);
        characterController.Move(gravityMove);
    }
}
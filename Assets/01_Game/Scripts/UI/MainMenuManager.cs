using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Target Name")]
    [SerializeField] private string gameplaySceneName = "level";

    // Fungsi yang akan dipanggil oleh Tombol Mulai
    public void StartGame()
    {
        Debug.Log("[Menu] Memulai petualangan pertanian pintar...");
        // Membuka scene pertanian bersasarkan namanya
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Fungsi yang akan dipanggil oleh Tombol Keluar
    public void QuitGame()
    {
        Debug.Log("[Menu] Menutup aplikasi game. Sampai jumpa!");
        // Perintah untuk menutup game (fungsi ini bekerja setelah game di-build menjadi .exe)
        Application.Quit();
    }
}

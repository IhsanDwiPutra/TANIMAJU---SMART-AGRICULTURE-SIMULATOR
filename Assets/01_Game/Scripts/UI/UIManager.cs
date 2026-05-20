using UnityEngine;
using TMPro; // Untuk mengontrol TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Text References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi untuk memperbarui tampilan teks uang
    public void UpdateMoneyUI(int currentMoney)
    {
        if (moneyText != null)
        {
            moneyText.text = "Uang: " + currentMoney + " Koin";
        }
    }

    // Fungsi untuk memperbarui tampilan teks waktu
    public void UpdateTimeGUI(int day, int hour)
    {
        if (timeText != null)
        {
            // Format jam agar terlihat rapi
            string formattedHour = hour.ToString("00") + ":00";
            timeText.text = "Hari: " + day + " | Jam: " + formattedHour;
        }
    }
}

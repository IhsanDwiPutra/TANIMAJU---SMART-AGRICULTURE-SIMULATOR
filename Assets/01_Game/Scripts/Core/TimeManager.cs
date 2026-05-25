using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public static event Action OnHourChanged; // Event penanda jam berubah

    public float timeSpeed = 60f; // Kecepatan waktu virtual
    private float currentSecond;
    public int currentHour = 6; // Mulai game jam 6 pagi
    public int currentDay = 1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Menghitung detik virtual berdasarkan waktu nyata
        currentSecond += Time.deltaTime * timeSpeed;
        UIManager.Instance.UpdateTimeGUI(currentDay, currentHour);

        if (currentSecond >= 60f)
        {
            currentSecond = 0;
            currentHour++;

            // Picu event OnHourChanged agar semua petak tanah tahu jam sudah berganti
            OnHourChanged.Invoke();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateTimeGUI(currentDay, currentHour);
            }

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay ++;

                // Panggil acak cuaca saat hari baru dimulai
                if (WeatherManager.Instance != null) WeatherManager.Instance.RandomizeWeather();

                // Acak harga pasar saat hari baru dimulai!
                if (MarketManager.Instance != null) MarketManager.Instance.UpdateMarketPrices();

                // Otomatis save progress setiap hari baru dimulai!
                if (SaveManager.Instance != null) SaveManager.Instance.SaveGameProgress();


            }
        }
    }
}

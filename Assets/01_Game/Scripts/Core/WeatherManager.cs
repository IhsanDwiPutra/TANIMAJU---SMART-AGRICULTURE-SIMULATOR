using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public enum WeatherType { Sunny, Rainy, Heatwave }
    [Header("Current Weather")]
    public WeatherType currentWeather = WeatherType.Sunny;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Daftarkan ke event ganti hari di TimeManager (jika ada event ganti hari, atau panggil manual)
        RandomizeWeather();
    }

    // Fungsi untuk mengacak cuaca baru
    public void RandomizeWeather()
    {
        // Mengacak angka antara 0 sampai 2
        int randomIndex = Random.Range(0, 3);
        currentWeather = (WeatherType)randomIndex;

        Debug.Log("[BMKG Virtual] Cuaca hari ini berganti menjadi: " + currentWeather.ToString());

        // Update tampilan teks cuaca di UI layar
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateWeatherUI(currentWeather.ToString());
        }
    }

    // Fungsi pembantu untuk memodifikasi penguapan air di FarmingPlot
    public float GetWeatherEvaporationMultiplier()
    {
        switch (currentWeather)
        {
            case WeatherType.Rainy:
                return -10f; // Nilai minus artinya malah menambah air (hujan otomatis)
            case WeatherType.Heatwave:
                return 3f; // Penguapan 3x lipat lebih gila
            case WeatherType.Sunny:
            default:
                return 1f; // Penguapan normal
        }
    }
}

using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi utama untuk menyimpan progress game
    public void SaveGameProgress()
    {
        // Ambil data dari masing-masing manager dan simpan ke PlayerPrefs
        if (EconomyManager.Instance != null)
        {
            PlayerPrefs.SetInt("SavedMoney", EconomyManager.Instance.GetCurrentMoney());
        }

        if (TimeManager.Instance != null)
        {
            PlayerPrefs.SetInt("SavedDay", TimeManager.Instance.currentDay);
            PlayerPrefs.SetInt("SavedHour", TimeManager.Instance.currentHour);
        }

        if (UpgradeManager.Instance != null)
        {
            PlayerPrefs.SetInt("SavedGrowthLevel", UpgradeManager.Instance.growthSpeedLevel);
            PlayerPrefs.SetInt("SavedWaterLevel", UpgradeManager.Instance.waterEcoLevel);
        }

        // Menyimpan stok benih di inventory
        if (InventoryManager.Instance != null)
        {
            // Ambil referensi benih dari MarketManager
            if (MarketManager.Instance.padiData != null) PlayerPrefs.SetInt("Inventory_Padi", InventoryManager.Instance.GetSeedCount(MarketManager.Instance.padiData));
            if (MarketManager.Instance.gandumData != null) PlayerPrefs.SetInt("Inventory_Gandum", InventoryManager.Instance.GetSeedCount(MarketManager.Instance.gandumData));
        }

        // Menyimpan status setiap petak lahan di grid
        // FarmingPlot[] allPlots = FindObjectOfType<FarmingPlot>();
        // foreach (FarmingPlot plot in allPlots)
        // {
        //     // Simpan status lahan 
        // }

        // Tulis data ke harddisk komputer/perangkat
        PlayerPrefs.Save();
        Debug.Log("[Save System] Progress pertanian pintar kamu berhasil disimpan otomatis!");
    }
    
    // Fungsi utama untuk memuat kembali progres game
    public void LoadGameProgress()
    {
        // Cek dulu apakah pernah ada data yang disimpan sebelumnya
        if (PlayerPrefs.HasKey("SavedMoney"))
        {
            // Muat data Uang
            int loadedMoney = PlayerPrefs.GetInt("SavedMoney");
            EconomyManager.Instance.AddMoney(loadedMoney - EconomyManager.Instance.GetCurrentMoney());

            // Muat data waktu
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.currentDay = PlayerPrefs.GetInt("SavedDay");
                TimeManager.Instance.currentHour = PlayerPrefs.GetInt("SavedHour");
            }

            // Muat data Upgrade Level
            if (UpgradeManager.Instance != null)
            {
                UpgradeManager.Instance.growthSpeedLevel = PlayerPrefs.GetInt("SavedGrowthLevel");
                UpgradeManager.Instance.waterEcoLevel = PlayerPrefs.GetInt("SavedWaterLevel");
            }

            Debug.Log("[Save System] Progres lama berhasil dimuat! Selamat datang kembali di ladang cerdasmu.");
        }
        else
        {
            Debug.Log("[Save System] Tidak ditemukan data lama. Memulai petualangan tani dari awal!");
        }
    }

    // Fitur tambahan: Hapus save data
    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("[Save System] Semua data progres telah dihapus bersih!");
    }
}

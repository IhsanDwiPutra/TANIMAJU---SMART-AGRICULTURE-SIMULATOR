using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Upgrade Levels")]
    public int growthSpeedLevel = 0; // Level 0 artinya normal
    public int waterEcoLevel = 0;

    [Header("Upgrade Costs")]
    public int upgradeCost = 1500; // Harga sekali upgrade 1500 koin

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi untuk meningkatkan kecepatan tumbuh tanaman
    public void UpgradeGrowthSpeed()
    {
        if (EconomyManager.Instance.RemoveMoney(upgradeCost))
        {
            growthSpeedLevel++;
            Debug.Log("[Upgrade] Berhasil meningkatkan Level Pupuk Genetik! Level saat ini: " + growthSpeedLevel);
        }
    }

    // Fungsi untuk menghemat air tanah
    public void UpgradeWaterEco()
    {
        if (EconomyManager.Instance.RemoveMoney(upgradeCost))
        {
            waterEcoLevel++;
            Debug.Log("[Upgrade] Berhasil meningkatkan Level Eko-Irigasi! Level saat ini: " + waterEcoLevel);
        }
    }

    // Fungsi pembantu untuk memotong waktu tumbuh di FarmingPlot
    public float GetGrowthMultiplier()
    {
        // Setiap level upgrade memotong waktu tumbuh sebesar 15%
        return Mathf.Max(0.4f, 1f - (growthSpeedLevel * 0.15f));
    }

    // Fungsi pembantu untuk memotong tingkat penguapan air di FarmingPlot
    public float GetEvaporationReduction()
    {
        // Setiap level upgrade mengurangi penguapan sebesar 1 poin
        return Mathf.Max(1f, 5f - waterEcoLevel);
    }
}

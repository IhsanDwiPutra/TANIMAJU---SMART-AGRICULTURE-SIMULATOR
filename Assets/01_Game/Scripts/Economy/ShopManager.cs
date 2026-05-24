using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Reference")]
    public GameObject shopPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi untuk membuka atau menutup toko (Toggle)
    public void ToggleShop()
    {
        if (shopPanel != null)
        {
            // Jika aktif maka nonaktifkan, jika nonaktif maka aktifkan
            bool currentState = shopPanel.activeSelf;
            shopPanel.SetActive(!currentState);
            Debug.Log("Status Toko diunah menjadi: " + !currentState);
        }
    }

    // Fungsi utama yang nanti akan ditempelkan ke Tombol UI Toko
    public void BuySeed(CropData crop)
    {
        if (crop == null) return;

        // Cek apakah uang pemain cukup berdasarkan purchasePrice di CropData
        if (EconomyManager.Instance.RemoveMoney(crop.purchasePrice))
        {
            // Jika uang cukup, tambahkan 1 benih ke inventory
            InventoryManager.Instance.AddSeed(crop, 1);
            Debug.Log("Berhasil membeli 1 Benih " + crop.cropName);
        }
        else
        {
            Debug.LogWarning("Gagal beli benih! Uangmu seret.");
        }
    }
}

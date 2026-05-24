using UnityEngine;
using System;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance;

    [Header("Crop Data References")]
    public CropData padiData;
    public CropData gandumData;

    // Menyimpan harga dasar (default) agar acakannya tetap realistis
    private int basePadiPrice;
    private int baseGandumPrice;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Simpan harga asli dari ScriptableObject di awal game
        if (padiData != null) basePadiPrice = padiData.sellPrice;
        if (gandumData != null) baseGandumPrice = gandumData.sellPrice;

        // Daftarkan fungsi ke event setiap kali hari berganti (bisa pakai logika jam/hari)
        // Untuk tes awal, kita buat fungsinya mengacak harga saat start game
        UpdateMarketPrices();
    }

    // Fungsi utama untuk mengacak harga jual tanaman
    public void UpdateMarketPrices()
    {
        if (padiData != null)
        {
            // Acak harga padi: harga dasar + atau - kisaran random (misal -50 sampai + 100)
            padiData.sellPrice = basePadiPrice + UnityEngine.Random.Range(-50, 101);
            // Batasi agar harga gak sampai minus atau 0 koin
            if (padiData.sellPrice < 50) padiData.sellPrice = 50;

            Debug.Log("[Pasar] Hari Baru! Harga jual Padi hari ini: " + padiData.sellPrice + " Koin");
        }

        if (gandumData != null)
        {
            gandumData.sellPrice = baseGandumPrice + UnityEngine.Random.Range(-70, 121);
            if (gandumData.sellPrice < 70) gandumData.sellPrice = 70;

            Debug.Log("[Pasar] Hari Baru! Harga jual Gandum hari ini: " + gandumData.sellPrice + " Koin");
        }

        // Kabari UIManager untuk memperbarui teks papan harga (nanti kita buat di step bawah)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMarketBoardUI(padiData.sellPrice, gandumData.sellPrice);
        }
    }
}

using UnityEngine;
using System.Collections.Generic; // Untuk menggunakan Dictionary

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Dictionary untuk menyimpan data: <Jenis Tanaman, Jumlah Benih yang Dimiliki>
    private Dictionary<CropData, int> seedInventory = new Dictionary<CropData, int>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public CropData testCrop;
    void Start()
    {
        AddSeed(testCrop, 5);
    }

    // Fungsi untuk menambah benih (saat beli di toko)
    public void AddSeed(CropData crop, int amount)
    {
        if (seedInventory.ContainsKey(crop))
        {
            seedInventory[crop] += amount;
        }
        else
        {
            seedInventory.Add(crop, amount);
        }
        Debug.Log("Benih " + crop.cropName + " bertambah. Total sekarang: " + seedInventory[crop]);
    }

    // Fungsi untuk mengurangi benih (saat ditanam di ladang)
    public bool TryUseSeed(CropData crop)
    {
        if (seedInventory.ContainsKey(crop) && seedInventory[crop] > 0)
        {
            seedInventory[crop]--;
            Debug.Log("Benih " + crop.cropName + " digunakan. Sisa: " + seedInventory[crop]);
            return true; // Berhasil digunakan
        }

        Debug.LogWarning("Kamu tidak punya benih " + crop.cropName + "!");
        return false; // Gagal karena benih habis
    }

    // Fungsi pembantu toko untuk mengecek jumlah benih saat ini
    public int GetSeedCount(CropData crop)
    {
        if (seedInventory.ContainsKey(crop)) return seedInventory[crop];
        return 0;
    }
}

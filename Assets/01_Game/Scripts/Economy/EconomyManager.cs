using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // Pola Singleton agar gampang dipanggil dari script mana saja
    public static EconomyManager Instance;

    [Header("Player Wallet")]
    [SerializeField] private int currentMoney = 1000; // Modal awal pemain 1000 koin

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi untuk menambah uang (saat panen atau jual hasil)
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Uang bertambah! Total Dompet saat ini: " + currentMoney + " Koin");
    }

    // Fungsi untuk mengurangi uang (saat beli benih atau alat IoT)
    public bool RemoveMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log("Uang berkurang! Total Dompet saat ini: " + currentMoney + " Koin");
            return true; // Transaksi sukses
        }
        else
        {
            Debug.LogWarning("Uang tidak cukup untuk melakukan transaksi!");
            return false; // Transaksi gagal karena miskin
        }
    }

    // Fungsi pembantu untuk mengecek jumlah uang saat ini
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}

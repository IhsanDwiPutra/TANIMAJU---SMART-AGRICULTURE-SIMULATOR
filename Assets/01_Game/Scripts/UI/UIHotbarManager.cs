using UnityEngine;
using UnityEngine.UI; // Untuk memanipulasi komponen Image UI

public class UIHotbarManager : MonoBehaviour
{
    public static UIHotbarManager Instance;

    [Header("Hotbar Slots Image References")]
    public Image[] slots; // Masukkan 4 objek gambar slot tadi

    [Header("Selection Colors")]
    [SerializeField] private Color activeColor = Color.yellow; // Warna saat slot aktif dipilih
    [SerializeField] private Color inactiveColor = Color.white; // Warna normal saat slot pasif

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Di awal game, set default slot 1 (Cangkul) yang menyala aktif
        UpdateHotbarVisual(0);
    }

    // Fungsi utama untuk mengubah highlight kotak slot berdasarkan index (0 sampai 3)
    public void UpdateHotbarVisual(int activeIndex)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                // Jika index-nya cocok dengan alat yang aktif, ganti warnanya jadi kuning emas
                if (i == activeIndex)
                {
                    slots[i].color = activeColor;
                }
                else
                {
                    slots[i].color = inactiveColor;
                }
            }
        }
    }
}

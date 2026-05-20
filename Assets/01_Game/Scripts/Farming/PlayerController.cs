using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Kita buat sistem sederhana untuk memilih alat apa yang lagi dipegang pemain
    public enum ActiveTool { Hoe, Seed, Hand, WaterCan, Sprinkler }
    [Header("Tools Settings")]
    public ActiveTool currentTool = ActiveTool.Hoe;

    public CropData seedToPlant; // Data benih yang mau ditanam kalau lagi megang alat 'Seed'
    public Camera mainCamera;    // Referensi ke kamera utama

    void Update()
    {
        // Cek kalau pemain klik kiri mouse
        if (Input.GetMouseButtonDown(0))
        {
            HandleFarmingInput();
        }

        // Fitur Tambahan: Tombol Shortcut Keyboard buat ganti alat (Biar gampang ngetes)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentTool = ActiveTool.Hoe; Debug.Log("Alat aktif: Cangkul (Hoe)"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentTool = ActiveTool.Seed; Debug.Log("Alat aktif: Benih (Seed)"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentTool = ActiveTool.Hand; Debug.Log("Alat aktif: Tangan Kosong (Hand)"); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { currentTool = ActiveTool.WaterCan; Debug.Log("Alat aktif: Gembor Air (WaterCan)");}
        if (Input.GetKeyDown(KeyCode.Alpha5)) { currentTool = ActiveTool.Sprinkler; Debug.Log("Alat aktif: Pasang Smart Sprinkler IoT");}
    }

    void HandleFarmingInput()
    {
        // Mengubah posisi mouse di layar menjadi struktur Sinar (Ray)
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Tembakkan raycast-nya (jarak di-set 100 meter)
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Cek apakah objek yang ditabrak punya script FarmingPlot
            FarmingPlot targetPlot = hit.collider.GetComponent<FarmingPlot>();

            if (targetPlot != null)
            {
                // Eksekusi aksi berdasarkan alat yang lagi dipegang pemain
                switch (currentTool)
                {
                    case ActiveTool.Hoe:
                        targetPlot.TillPlot(); // Panggil fungsi cangkul
                        break;

                    case ActiveTool.Seed:
                        if (seedToPlant != null)
                        {
                            targetPlot.PlantSeed(seedToPlant); // Panggil fungsi tanam
                        }
                        else
                        {
                            Debug.LogWarning("Kamu milih alat benih, tapi belum masukin data benihnya di Inspector!");
                        }
                        break;

                    case ActiveTool.Hand:
                        targetPlot.HarvestPlot(); // Panggil fungsi panen
                        Debug.Log("Lagi panen bang");
                        break;
                    
                    case ActiveTool.WaterCan:
                        targetPlot.WaterPlot(); // Siram manual
                        Debug.Log("Tanah berhasil disiram manual!");
                        break;
                    
                    case ActiveTool.Sprinkler:
                        // Biar realistis, pasang sprinkler butuh modal. Misal harga pasang 500 koin
                        if (EconomyManager.Instance.RemoveMoney(500))
                        {
                            targetPlot.EquipSmartSprinkler(); // Pasang alat IoT otomatis
                            Debug.Log("Pasang alat IoT berhasil!");
                        }
                        break;
                }
            }
        }
    }
}
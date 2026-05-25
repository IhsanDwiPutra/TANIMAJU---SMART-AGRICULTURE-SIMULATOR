using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    // Kita buat sistem sederhana untuk memilih alat apa yang lagi dipegang pemain
    public enum ActiveTool { Hoe, Seed, Hand, WaterCan, Sprinkler }
    [Header("Tools Settings")]
    public ActiveTool currentTool = ActiveTool.Hoe;

    public CropData seedToPlant; // Data benih yang mau ditanam kalau lagi megang alat 'Seed'
    public Camera mainCamera;    // Referensi ke kamera utama

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 1.5f; // Jarak maksimal jangkauan tangan karakter
    [SerializeField] private LayerMask plotPlayer; // Untuk optimasi physics

    // Visaul genggaman alat
    [Header("Visual Hand Settings")]
    public Transform handPivot; // Taruh objek "Tangan" di sini

    [Header("Tool Prefabs")]
    public GameObject hoeModelPrefab; // Prefab 3D model Cangkul
    public GameObject seedModePrefab; // Prefab 3D model Kantong Benih
    public GameObject waterCanPrefab; // Prefab 3D model Ketel Air

    private GameObject currentHeldObject; // Menyimpan objek alat yang sedang aktif di tangan

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Di awal game, langsung munculkan alat default (Cangkul)
        UpdateHeldToolVisual();
    }

    void Update()
    {
        // Fitur Tambahan: Tombol Shortcut Keyboard buat ganti alat (Biar gampang ngetes)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentTool = ActiveTool.Hoe;
            if (UIHotbarManager.Instance != null) UIHotbarManager.Instance.UpdateHotbarVisual(0); // Slot 1 aktif
            UpdateHeldToolVisual();
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTool = ActiveTool.Seed;
            if (UIHotbarManager.Instance != null) UIHotbarManager.Instance.UpdateHotbarVisual(1);
            UpdateHeldToolVisual();
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentTool = ActiveTool.Hand;
            if (UIHotbarManager.Instance != null) UIHotbarManager.Instance.UpdateHotbarVisual(2);
            UpdateHeldToolVisual();
        } 
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentTool = ActiveTool.WaterCan;
            if (UIHotbarManager.Instance != null) UIHotbarManager.Instance.UpdateHotbarVisual(3);
            UpdateHeldToolVisual();
        } 
        if (Input.GetKeyDown(KeyCode.Alpha5)) { currentTool = ActiveTool.Sprinkler; Debug.Log("Alat aktif: Pasang Smart Sprinkler IoT");}
        if (Input.GetKeyDown(KeyCode.B)) ShopManager.Instance.ToggleShop();

        // Tombol aksi: Sekarang pakai tombil E atau klik mouse kiri
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            CheckAndInteractWithPlot();
        }
    }

    // Fungsi untuk mengganti model 3D di tangan karakter secara real-time
    void UpdateHeldToolVisual()
    {
        // Hancurkan dulu alat lama yang sedang dipegang agar tidak menumpuk
        if (currentHeldObject != null)
        {
            Destroy(currentHeldObject);
        }

        GameObject prefabToSpawn = null;

        // Tentukan prefab mana yang mau dimunculkan di tangan berdasarkan alat aktif
        switch (currentTool)
        {
            case ActiveTool.Hoe:
                prefabToSpawn = hoeModelPrefab;
                break;
            case ActiveTool.Seed:
                prefabToSpawn = seedModePrefab;
                break;
            case ActiveTool.WaterCan:
                prefabToSpawn = waterCanPrefab;
                break;
            case ActiveTool.Hand:
                // Untuk panen bisa dikosongkan (tangan kosong) atau pakai model sabit
                prefabToSpawn = null;
                break;
        }

        // Spawn model alat baru di koordinat objek "Tangan"
        if (prefabToSpawn != null && handPivot != null)
        {
            currentHeldObject = Instantiate(prefabToSpawn, handPivot.position, handPivot.rotation);
            // Jadikan child dari objek Tangan agar ikut bergerak seirama tubuh player
            currentHeldObject.transform.parent = handPivot;
        }
    }

    // Fungsi untuk mencari petak tanah terdekat dari posisi karakter
    void CheckAndInteractWithPlot()
    {
        // Cari semua collider dalam radius di sekitar Player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);
        FarmingPlot closestPlot = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in hitColliders)
        {
            // Cek apakah objek tersebut adalah petak lahan
            if (col.CompareTag("FarmingPlot"))
            {
                FarmingPlot plot = col.GetComponent<FarmingPlot>();
                if (plot != null)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    // Ambil yang jaraknya paling nempel dengan karakter
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlot = plot;
                    }
                }
            }
        }

        // Jika ketemu petak terdekat, eksekusi alat yang sedang aktif
        if (closestPlot != null)
        {
            ExecuteToolAction(closestPlot);
        }
    }

    // Mengeksekusi aksi berdasarkan alat yang dipilih
    void ExecuteToolAction(FarmingPlot targetPlot)
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
                    // Cek dulu: Apakah benihnya ada di kantong inventory?
                    if (InventoryManager.Instance.GetSeedCount(seedToPlant) > 0)
                    {
                        // Jika petak tanah berhasil ditanam (statusnya berubah)
                        if (targetPlot.currentState == FarmingPlot.PlotState.Tilled)
                        {
                            targetPlot.PlantSeed(seedToPlant);
                            InventoryManager.Instance.TryUseSeed(seedToPlant); // Kurangi benihnya di inventory
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Gagal menanam! Benih " + seedToPlant.cropName + " kamu habis. Beli dulu di toko!");
                    }
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

    // Untuk visualisasikan jarak jangkauan di jendela Scene editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
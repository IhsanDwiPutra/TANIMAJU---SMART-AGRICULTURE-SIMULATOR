using UnityEngine;

public class FarmingPlot : MonoBehaviour
{
    // Mengatur fase status tanah
    public enum PlotState { Empty, Tilled, Planted }
    public PlotState currentState = PlotState.Empty;

    [Header("Water & IoT Settings")]
    public float waterLevel = 100f; // Indikator air (0% - 100%)
    public float evaporationRate = 5f; // Berapa % air berkurang setiap jam virtual
    public bool hasSmartSprinkler = false; // Apakah alat IoT otomatus sudah terpasang?
    public GameObject sprinklerVisual; // Referensi objek 3D sprinkler pintar

    [Header("Crop Settings")]
    public CropData currentCropData; // Menyimpan data tanaman yang sedang ditanam
    private float growthTimer = 0f;
    private bool isMaxGrown = false;

    [Header("Visual References")]
    public Renderer plotRenderer;
    public Color emptyColor = new Color(0.5f, 0.35f, 0.2f); // Cokelat muda
    public Color tilledColor = new Color(0.3f, 0.2f, 0.1f);  // Cokelat tua (subur)
    public Color dryColor = new Color(0.7f, 0.55f, 0.4f); // Warna tanah kalau sering kerontang
    
    private GameObject spawnedCropModel;

    void Start()
    {
        // Set warna awal tanah jadi tanah biasa
        UpdatePlotVisual();

        // Daftarkan fungsi penguapan air ke sistem jam TimeManager
        TimeManager.OnHourChanged += HandleEvaporation;
    }

    void OnDestroy()
    {
        // Unsubscribe agar tidak menyebabkan memory leak
        TimeManager.OnHourChanged -= HandleEvaporation;
    }

    void Update()
    {
        // Logika Otomasi IoT Smart Sprinkler
        if (hasSmartSprinkler && waterLevel < 30f)
        {
            WaterPlot(); // Otomatis menyiram kalau air di bawah 30%
            Debug.Log(gameObject.name + ": [IoT Alert] Sensor mendeteksi tanah kering, Smart Sprinkler otomatis menyiram!");
        }


        // Jika ada tanaman yang tertanam dan belum tumbuh maksimal
        if (currentState == PlotState.Planted && !isMaxGrown && waterLevel > 0)
        {
            growthTimer += Time.deltaTime;
            float growthPercentage = growthTimer / currentCropData.timeToGrow;

            // Mengatur ukuran model 3D secara bertahap berdasarkan waktu virtual
            if (spawnedCropModel != null)
            {
                // Fase 1: Baru ditanam (0% - 30% progres)
                if (growthPercentage < 0.3f)
                {
                    // Bisa scaling model 3D-nya biar mulai dari ukuran kecil
                    spawnedCropModel.transform.localScale = Vector3.one * 0.2f;
                }
                // Fase 2: Sedang tumbuh (30% - 70% progres)
                else if (growthPercentage >= 0.3f && growthPercentage < 0.7f)
                {
                    // Ukuran model 3D otomatis membesar di layar jadi setengah matang
                    spawnedCropModel.transform.localScale = Vector3.one * 0.6f;
                }
                // Fase 3: Matang (Diatas 70% progres sampai target waktu selesai)
                else
                {
                    spawnedCropModel.transform.localScale = Vector3.one * 1.0f;
                }
                
            }


            // Cek apakah timer pertumbuhan sudah melewati waktu target di CropData
            if (growthTimer >= (currentCropData.timeToGrow * UpgradeManager.Instance.GetGrowthMultiplier()))
            {
                isMaxGrown = true;

                // Saat Matang: Ganti model proses menjadi model matang!
                SwapToFullGrownModel();

                Debug.Log(currentCropData.cropName + " sudah siap dipanen!");
            }
        }
    }

    // Fungsi yang otomatis dipanggil setiap jam virtual berubah
    void HandleEvaporation()
    {
        if (currentState != PlotState.Empty)
        {
            waterLevel -= UpgradeManager.Instance.GetEvaporationReduction();
            if (waterLevel < 0) waterLevel = 0;

            UpdatePlotVisual();
        }
    }

    // Fungsi untuk menyiram tanah (bisa dipanggil manual atau lewat IoT)
    public void WaterPlot()
    {
        waterLevel = 100f;
        UpdatePlotVisual();
    }

    // Fungsi untuk memasang alat pintar IoT (Beli dari toko)
    public void EquipSmartSprinkler()
    {
        if (!hasSmartSprinkler)
        {
            hasSmartSprinkler = true;
            if (sprinklerVisual != null) sprinklerVisual.SetActive(true); // Munculkan visual alatnya
            Debug.Log("Smart Sprinkler IoT berhasil dipasang di petak ini!");
        }
    }

    // Fungsi untuk mencangkul tanah
    public void TillPlot()
    {
        if (currentState == PlotState.Empty)
        {
            currentState = PlotState.Tilled;
            waterLevel = 100f; // Otomatis basah setelah dicangkul awal
            UpdatePlotVisual();
            Debug.Log("Tanah berhasil dicangkul!");
        }
    }

    // Fungsi untuk menanam benih
    public void PlantSeed(CropData crop)
    {
        if (currentState == PlotState.Tilled)
        {
            currentCropData = crop;
            currentState = PlotState.Planted;
            growthTimer = 0f;
            isMaxGrown = false;
            Debug.Log("Menanam benih: " + crop.cropName);

            // Langsung spawn model di sini: Muncul saat klik tanam dengan ukuran awal yang kecil
            if (currentCropData.growingModel != null)
            {
                spawnedCropModel = Instantiate(currentCropData.growingModel, transform.position + Vector3.up * 0.2f, Quaternion.identity);
                spawnedCropModel.transform.parent = this.transform;
            }
        }
    }

    // Fungsi untuk memanen
    public void HarvestPlot()
    {
        if (currentState == PlotState.Planted && isMaxGrown)
        {
            // Panggil EconomyManager untuk menambah uang berdasarkan harga jual tanaman
            if (currentCropData != null)
            {
                EconomyManager.Instance.AddMoney(currentCropData.sellPrice);
            }

            // Reset status petak tanah kembali ke awal
            currentState = PlotState.Empty;
            currentCropData = null;
            growthTimer = 0f;
            isMaxGrown = false;

            if (spawnedCropModel != null) Destroy(spawnedCropModel);

            UpdatePlotVisual();
            Debug.Log("Tanaman berhasil dipanen! Uang bertambah (Logika uang nanti di step berikutnya).");
        }
    }

    // Mengubah warna tanah berdasarkan statusnya
    void UpdatePlotVisual()
    {
        if (waterLevel <= 0) plotRenderer.material.color = dryColor; // Tanah mati/kering
        else if (currentState == PlotState.Empty) plotRenderer.material.color = emptyColor;
        else if (currentState == PlotState.Tilled || currentState == PlotState.Planted)
        {
            // Semakin kering tanahnya, warnanya bakal memudar dari cokelat tua ke cokelar muda
            plotRenderer.material.color = Color.Lerp(emptyColor, tilledColor, waterLevel / 100f);
        }
    }

    // Memunculkan aset 3D tumbuhan yang sudah matang
    void SpawnFullGrownModel()
    {
        if (currentCropData.fullGrownModel != null)
        {
            // Munculkan model 3D tanaman tepat di atas petak tanah
            spawnedCropModel = Instantiate(currentCropData.fullGrownModel, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    // Fungsi untuk mengahancurkan model lama dan menggantinya dengan model matang
    void SwapToFullGrownModel()
    {
        // Hancurkan dulu model proses tumbuh yang lama
        if (spawnedCropModel != null)
        {
            Destroy(spawnedCropModel);
        }

        // Spawn model baru yang sudah matang/masak
        if (currentCropData.fullGrownModel != null)
        {
            spawnedCropModel = Instantiate(currentCropData.fullGrownModel, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            spawnedCropModel.transform.parent = this.transform;
        }
    }
}
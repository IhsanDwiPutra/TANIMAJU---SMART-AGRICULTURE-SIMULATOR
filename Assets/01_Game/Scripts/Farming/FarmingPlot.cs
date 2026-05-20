using UnityEngine;

public class FarmingPlot : MonoBehaviour
{
    // Mengatur fase status tanah
    public enum PlotState { Empty, Tilled, Planted }
    public PlotState currentState = PlotState.Empty;

    [Header("Crop Settings")]
    public CropData currentCropData; // Menyimpan data tanaman yang sedang ditanam
    private float growthTimer = 0f;
    private bool isMaxGrown = false;

    [Header("Visual References")]
    public Renderer plotRenderer;
    public Color emptyColor = new Color(0.5f, 0.35f, 0.2f); // Cokelat muda
    public Color tilledColor = new Color(0.3f, 0.2f, 0.1f);  // Cokelat tua (subur)
    
    private GameObject spawnedCropModel;

    void Start()
    {
        // Set warna awal tanah jadi tanah biasa
        UpdatePlotVisual();
    }

    void Update()
    {
        // Jika ada tanaman yang tertanam dan belum tumbuh maksimal
        if (currentState == PlotState.Planted && !isMaxGrown)
        {
            growthTimer += Time.deltaTime;

            // Cek apakah timer pertumbuhan sudah melewati waktu target di CropData
            if (growthTimer >= currentCropData.timeToGrow)
            {
                isMaxGrown = true;
                SpawnFullGrownModel();
                Debug.Log(currentCropData.cropName + " sudah siap dipanen!");
            }
        }
    }

    // Fungsi untuk mencangkul tanah
    public void TillPlot()
    {
        if (currentState == PlotState.Empty)
        {
            currentState = PlotState.Tilled;
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
        }
    }

    // Fungsi untuk memanen
    public void HarvestPlot()
    {
        if (currentState == PlotState.Planted && isMaxGrown)
        {
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
        if (currentState == PlotState.Empty) plotRenderer.material.color = emptyColor;
        else if (currentState == PlotState.Tilled) plotRenderer.material.color = tilledColor;
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
}
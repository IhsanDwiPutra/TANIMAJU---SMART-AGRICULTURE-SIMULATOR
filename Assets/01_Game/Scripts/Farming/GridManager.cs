using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject plotPrefab; // Masukkan Prefab Petak_Lahan di sini
    public int width = 3; // Jumlah kolom (ke samping X)
    public int height = 3; // Jumlah baris (ke belakang Z)
    public float spacing = 2.2f; // Jarak antar petak lahan

    void Start()
    {
        GenerateFarmGrid();

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadGameProgress();
        }
    }

    void GenerateFarmGrid()
    {
        if (plotPrefab == null)
        {
            Debug.LogError("Plot Prefab belum dimasukkan di GridManager!");
            return;
        }

        // Perulangan bersarang untuk membuat baris dan kolom
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Hitung posisi kalkulasi koordinat untuk setiap petak lahan
                Vector3 spawnPosition = new Vector3(x * spacing, 0, z * spacing);

                // Spawn objek petak lahan baru ke dunia virtual
                GameObject newPlot = Instantiate(plotPrefab, spawnPosition, Quaternion.identity);

                // Masukkan ke dalam parent GameObject ini agar Hierarki tetap rapi
                newPlot.transform.parent = this.transform;

                // Beri nama unik di Hierarki berdasarkan koordinat petaknya
                newPlot.name = "Petak_Lahan_" + x + "_" + z;
            }
        }

        Debug.Log("Ladang berhasil dibuat dengan total " + (width * height) + " petak!");
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Agriculture/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName; // Nama tanaman (Contoh: Padi)
    public float timeToGrow; // Total waktu yang dibutuhkan sampai panen (dalam detik)
    public int purchasePrice; // Harga beli benih
    public int sellPrice; // Harga jual hasil panen

    // FITUR MODEL TERPISAH
    public GameObject growingModel;
    public GameObject fullGrownModel; // Aset 3D ketika tanaman sudah siap panen

    public Sprite cropIcon; // Ikon untuk tombol UI
}

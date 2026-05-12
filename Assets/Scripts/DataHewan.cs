using UnityEngine;

[CreateAssetMenu(fileName = "DataSatwaBaru", menuName = "Ensiklopedia AR/Buat Data Hewan")]
public class DataHewan : ScriptableObject
{
    [Header("Identitas Satwa")]
    public string namaHewan;

    [Header("Statistik Fisik")]
    public string tinggi;
    public string berat;
    public string habitat;
    public string asal;
    public string makanan;

    [Header("Informasi Lanjut")]
    [TextArea(3, 10)]
    public string deskripsi;

    [Header("Suara")]
    public AudioClip suaraHewan;
}
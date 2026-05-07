using UnityEngine;
using TMPro;

public class InfoSatwaManager : MonoBehaviour
{
    [Header("Data Nilai (Panel Kanan)")]
    public TextMeshProUGUI valNama;
    public TextMeshProUGUI valTinggi;
    public TextMeshProUGUI valBerat;
    public TextMeshProUGUI valHabitat;
    public TextMeshProUGUI valAsal; 
    public TextMeshProUGUI valMakanan;

    [Header("Data Deskripsi (Panel Bawah)")]
    public TextMeshProUGUI txtDesc;

    public void TampilkanInfo(DataHewan data)
    {
        if (data == null) return;

        valNama.text = data.namaHewan;
        valTinggi.text = data.tinggi;
        valBerat.text = data.berat;
        valHabitat.text = data.habitat;
        valAsal.text = data.asal; 
        valMakanan.text = data.makanan;

        txtDesc.text = data.deskripsi;
    }
}
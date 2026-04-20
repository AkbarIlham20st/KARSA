using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Pengaturan Scene")]
    [Tooltip("Pastikan nama ini sama persis dengan nama file Scene AR Anda")]
    public string namaSceneAR = "ARDisplay";

    [Header("Pengaturan UI")]
    [Tooltip("Masukkan objek Panel Panduan ke sini")]
    public GameObject panelPanduan;

    void Start()
    {
        // Memastikan Panel Panduan dalam keadaan tersembunyi saat aplikasi baru dibuka
        if (panelPanduan != null)
        {
            panelPanduan.SetActive(false);
        }
    }

    // --- FUNGSI TOMBOL START ---
    public void MulaiAR()
    {
        Debug.Log("Memasuki Mode AR...");
        SceneManager.LoadScene(namaSceneAR);
    }

    // --- FUNGSI TOMBOL PANDUAN ---
    public void BukaPanduan()
    {
        if (panelPanduan != null)
        {
            panelPanduan.SetActive(true); // Memunculkan panel
        }
    }

    // --- FUNGSI TOMBOL TUTUP PANDUAN (X) ---
    public void TutupPanduan()
    {
        if (panelPanduan != null)
        {
            panelPanduan.SetActive(false); // Menyembunyikan panel kembali
        }
    }

    // --- FUNGSI TOMBOL KELUAR ---
    public void KeluarAplikasi()
    {
        Debug.Log("Aplikasi Ditutup");
        Application.Quit(); // Catatan: Ini hanya akan menutup aplikasi jika sudah di-build ke HP
    }
}
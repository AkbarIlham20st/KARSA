using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class ARManager : MonoBehaviour
{
    [Header("=== PENGATURAN AR ===")]
    public ARRaycastManager raycastManager;
    private GameObject spawnedObject; // Mengingat hewan apa yang sedang muncul di layar

    [Header("=== PENGATURAN UI ===")]
    public GameObject panelMenuHewan; // Masukkan MenuPilihHewan (Scroll View 33 Hewan) ke sini


    // -------------------------------------------------------------------
    // FUNGSI 1: BUKA-TUTUP MENU HEWAN (Dipakai di Tombol Pojok Kanan Atas)
    // -------------------------------------------------------------------
    public void ToggleDaftarHewan()
    {
        // Memastikan Anda tidak lupa memasukkan objek di Inspector
        if (panelMenuHewan != null)
        {
            bool sedangAktif = panelMenuHewan.activeSelf;
            panelMenuHewan.SetActive(!sedangAktif); // Balikkan keadaannya
        }
        else
        {
            Debug.LogWarning("Perhatian: Panel Menu Hewan belum diisi di Inspector!");
        }
    }


    // -------------------------------------------------------------------
    // FUNGSI 2: MENGELUARKAN HEWAN (Dipakai di 33 Tombol dalam Menu)
    // -------------------------------------------------------------------
    public void PilihHewan(string namaHewan)
    {
        // 1. Bersihkan layar dulu (Hapus hewan sebelumnya jika ada)
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }

        // 2. Ambil cetakan hewan (Prefab) dari dalam folder 'Resources'
        GameObject prefab = Resources.Load<GameObject>(namaHewan);

        // 3. Jika cetakannya ketemu, jadikan nyata!
        if (prefab != null)
        {
            // Memunculkan hewan ke layar (sementara di titik 0,0,0)
            spawnedObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Sukses memunculkan: " + namaHewan);

            // 4. Otomatis sembunyikan menu 33 hewan agar layar AR kembali lega
            if (panelMenuHewan != null)
            {
                panelMenuHewan.SetActive(false);
            }
        }
        else
        {
            // Munculkan pesan error warna merah jika nama tombol beda dengan nama prefab
            Debug.LogError("Gagal! Tidak menemukan Prefab bernama [" + namaHewan + "] di folder Resources. Cek huruf besar/kecilnya.");
        }
    }


    // -------------------------------------------------------------------
    // FUNGSI 3: KEMBALI KE BERANDA (Dipakai di Tombol Pojok Kiri Atas)
    // -------------------------------------------------------------------
    public void TombolBack()
    {
        // Tutup AR dan muat halaman utama. (Pastikan MainMenu ada di Build Settings)
        SceneManager.LoadScene("MainMenu");
    }

    // -------------------------------------------------------------------
    // FUNGSI 4: KONTROL ANIMASI HEWAN (Dipakai di Tombol Kanan)
    // -------------------------------------------------------------------
    public void AnimasiJalan()
    {
        // Pastikan ada hewan yang sedang tampil di layar
        if (spawnedObject != null)
        {
            // Ambil "otak animator" dari hewan yang sedang tampil tersebut
            Animator anim = spawnedObject.GetComponent<Animator>();

            if (anim != null)
            {
                // Nyalakan sakelar jalan
                anim.SetBool("isWalking", true);
            }
        }
    }

    public void AnimasiDiam()
    {
        if (spawnedObject != null)
        {
            Animator anim = spawnedObject.GetComponent<Animator>();

            if (anim != null)
            {
                // Matikan sakelar jalan
                anim.SetBool("isWalking", false);
            }
        }
    }
}
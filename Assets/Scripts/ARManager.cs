using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // ✅ WAJIB: Untuk mendeteksi jari menyentuh tombol UI

public class ARManager : MonoBehaviour
{
    [Header("=== PENGATURAN AR ===")]
    public ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private GameObject selectedPrefab; // Menyimpan memori hewan apa yang dipilih dari menu

    [Header("=== PENGATURAN UI ===")]
    public GameObject panelMenuHewan;
    public GameObject panelInfoSatwa;

    // Tempat menyimpan data titik benturan laser (raycast) ke lantai
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // -------------------------------------------------------------------
    // FUNGSI UTAMA AR: DETEKSI KETUKAN KE LANTAI (Tap to Place)
    // -------------------------------------------------------------------
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Layar disentuh!"); // Pesan 1: Jari terdeteksi

                if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
                {
                    Debug.Log("Lantai kena tembak!"); // Pesan 2: Lantai terdeteksi laser

                    if (selectedPrefab != null)
                    {
                        Debug.Log("Mencoba memunculkan: " + selectedPrefab.name); // Pesan 3: Nama hewan ada

                        Pose hitPose = hits[0].pose;
                        if (spawnedObject == null)
                        {
                            spawnedObject = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                        }
                        else
                        {
                            spawnedObject.transform.position = hitPose.position;
                        }
                    }
                    else
                    {
                        Debug.Log("Error: Anda belum pilih hewan dari menu!");
                    }
                }
                else
                {
                    Debug.Log("Gagal: Laser tidak mengenai lantai AR.");
                }
            }
        }
    }

    // -------------------------------------------------------------------
    // FUNGSI MEMILIH HEWAN (Dipanggil dari 33 Tombol Menu)
    // -------------------------------------------------------------------
    public void PilihHewan(string namaHewan)
    {
        // Mencari file hewan di dalam folder Resources
        selectedPrefab = Resources.Load<GameObject>(namaHewan);

        // --- TRIK DETEKTIF GETAR ---
        if (selectedPrefab != null)
        {
            // Jika hewannya BERHASIL ditemukan, HP akan bergetar pendek!
            Handheld.Vibrate();
        }
        // ---------------------------

        // Menutup menu panel
        if (panelMenuHewan != null)
        {
            panelMenuHewan.SetActive(false);
        }
    }

    // -------------------------------------------------------------------
    // FUNGSI ANIMASI (Dipakai di Tombol Kanan)
    // -------------------------------------------------------------------
    public void AnimasiJalan()
    {
        if (spawnedObject != null)
        {
            // ✅ PERBAIKAN BUG: Gunakan GetComponentInChildren
            Animator anim = spawnedObject.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("IsWalking", true);
        }
    }

    public void AnimasiDiam()
    {
        if (spawnedObject != null)
        {
            Animator anim = spawnedObject.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("IsWalking", false);
        }
    }

    // -------------------------------------------------------------------
    // FUNGSI TOGGLE MENU (Tetap Sama)
    // -------------------------------------------------------------------
    public void ToggleDaftarHewan()
    {
        if (panelMenuHewan != null)
        {
            bool status = panelMenuHewan.activeSelf;
            panelMenuHewan.SetActive(!status);
            if (!status && panelInfoSatwa != null) panelInfoSatwa.SetActive(false);
        }
    }

    public void ToggleInfoSatwa()
    {
        if (panelInfoSatwa != null)
        {
            bool status = panelInfoSatwa.activeSelf;
            panelInfoSatwa.SetActive(!status);
            if (!status && panelMenuHewan != null) panelMenuHewan.SetActive(false);
        }
    }

    public void TombolBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
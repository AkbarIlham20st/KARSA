using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ARManager : MonoBehaviour
{
    [Header("Pengaturan AR")]
    public ARRaycastManager raycastManager;

    [Header("Pengaturan UI")]
    public GameObject panelMenuHewan;
    public GameObject panelInfo;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public InfoSatwaManager infoSatwaManager;
    private GameObject selectedPrefab;
    private GameObject spawnedObject;

    // Variabel untuk menampung pesan debug di layar HP
    private string teksDebugLayar = "Status: Menunggu pilihan satwa...";

    void Start()
    {
        if (raycastManager == null)
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }
    }

    void Update()
    {
        // Deteksi tap di HP (atau klik di laptop)
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            // Cek apakah jari menyentuh tombol UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                teksDebugLayar = "Tap diblokir: Jari menyentuh Tombol/UI.";
                Debug.Log(teksDebugLayar);
                return;
            }

            Vector2 kordinatSentuhan = Pointer.current.position.ReadValue();

            // Cek apakah Mas sudah memilih hewan dari menu
            if (selectedPrefab == null)
            {
                teksDebugLayar = "Tap masuk! Tapi Mas belum memilih satwa dari menu.";
                Debug.Log(teksDebugLayar);
                return;
            }

            if (raycastManager != null)
            {
                // Cek apakah tap mengenai jaring lantai AR
                if (raycastManager.Raycast(kordinatSentuhan, hits, TrackableType.Planes))
                {
                    teksDebugLayar = "SUKSES: Lantai tertembak! Memunculkan satwa...";
                    Debug.Log(teksDebugLayar);

                    Handheld.Vibrate();
                    Pose hitPose = hits[0].pose;

                    if (spawnedObject != null)
                    {
                        Destroy(spawnedObject);
                    }

                    // 1. MUNCULKAN HEWANNYA
                    spawnedObject = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);

                    // 2. MUNCULKAN KUBUS PENDAMPING (Sebagai pelacak posisi)
                    GameObject kubusPelacak = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    kubusPelacak.transform.position = hitPose.position;
                    kubusPelacak.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    // Kubusnya akan hancur sendiri setelah 5 detik
                    Destroy(kubusPelacak, 5f);
                }
                else
                {
                    teksDebugLayar = "GAGAL MUNCUL: Jaring lantai belum kuat di titik tap tersebut.";
                    Debug.Log(teksDebugLayar);
                }
            }
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(40, 40, 1000, 200), teksDebugLayar, style);
    }

    // =======================================================
    // FUNGSI UNTUK MENU DAN INFO
    // =======================================================

    //public void PilihHewan(string namaHewan)
    //{
    //    selectedPrefab = Resources.Load<GameObject>(namaHewan);
    //    teksDebugLayar = "Satwa dipilih: " + namaHewan + ". Silakan tap lantai AR!";
    //    Debug.Log(teksDebugLayar);

    //    if (panelMenuHewan != null) panelMenuHewan.SetActive(false);
    //}

    public void PilihHewan(string namaHewan)
    {
        selectedPrefab = Resources.Load<GameObject>(namaHewan);

        // Memanggil data dari folder Resources
        DataHewan dataNya = Resources.Load<DataHewan>("Data_" + namaHewan);

        // GANTI BAGIAN INI: Pastikan namanya TampilkanInfo, bukan IsiDataKePanel
        if (dataNya != null && infoSatwaManager != null)
        {
            infoSatwaManager.TampilkanInfo(dataNya);
        }

        teksDebugLayar = "Satwa dipilih: " + namaHewan + ". Silakan tap lantai AR!";
        Debug.Log(teksDebugLayar);

        if (panelMenuHewan != null)
        {
            panelMenuHewan.SetActive(false);
        }
    }

    public void BukaMenuHewan()
    {
        if (panelMenuHewan != null)
        {
            panelMenuHewan.SetActive(!panelMenuHewan.activeSelf);
            if (panelInfo != null && panelMenuHewan.activeSelf) panelInfo.SetActive(false);
        }
    }

    public void BukaInfo()
    {
        if (panelInfo != null)
        {
            panelInfo.SetActive(!panelInfo.activeSelf);
            if (panelMenuHewan != null && panelInfo.activeSelf) panelMenuHewan.SetActive(false);
        }
    }

    public void TutupInfo()
    {
        if (panelInfo != null) panelInfo.SetActive(false);
    }

    public void TombolKembali()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // =======================================================
    // FUNGSI BARU: PENGENDALI ANIMASI (JURUS SAPU JAGAT)
    // =======================================================

    public void AnimasiJalan()
    {
        if (spawnedObject != null)
        {
            Animator anim = spawnedObject.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetBool("IsWalking", true);

                // DETEKTIF: Baca ulang otaknya, apakah beneran berubah jadi True?
                bool statusWalk = anim.GetBool("IsWalking");
                teksDebugLayar = "Status Jalan di Otak Anoa: " + statusWalk.ToString();
                Debug.Log(teksDebugLayar);
            }
        }
    }

    public void AnimasiDiam()
    {
        if (spawnedObject != null)
        {
            Animator[] semuaAnim = spawnedObject.GetComponentsInChildren<Animator>();
            bool adaYangMerespon = false;

            foreach (Animator anim in semuaAnim)
            {
                if (anim.runtimeAnimatorController != null)
                {
                    anim.SetBool("IsWalking", false);
                    adaYangMerespon = true;
                }
            }

            if (adaYangMerespon)
            {
                teksDebugLayar = "SUKSES: Semua otot berhenti (DIAM)!";
                Debug.Log(teksDebugLayar);
            }
        }
    }
}
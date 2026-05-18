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

    [Header("Pengaturan Scene")]
    public string namaSceneKembali;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public InfoSatwaManager infoSatwaManager;
    private GameObject selectedPrefab;
    private GameObject spawnedObject;

    [Header("Pengaturan Suara")]
    public AudioSource audioSource;
    private AudioClip suaraAktif;

    void Start()
    {
        if (raycastManager == null)
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector2 kordinatSentuhan = Pointer.current.position.ReadValue();

            if (selectedPrefab == null)
            {
                return;
            }

            if (raycastManager != null)
            {
                if (raycastManager.Raycast(kordinatSentuhan, hits, TrackableType.Planes))
                {
                    Handheld.Vibrate(); 
                    Pose hitPose = hits[0].pose;

                    if (spawnedObject != null)
                    {
                        Destroy(spawnedObject);
                    }

                    spawnedObject = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }
    }

    public void PilihHewan(string namaHewan)
    {
        selectedPrefab = Resources.Load<GameObject>(namaHewan);

        DataHewan dataNya = Resources.Load<DataHewan>("Data_" + namaHewan);

        if (dataNya != null)
        {
            if (infoSatwaManager != null)
            {
                infoSatwaManager.TampilkanInfo(dataNya);
            }

            suaraAktif = dataNya.suaraHewan;
        }

        if (panelMenuHewan != null) panelMenuHewan.SetActive(false);
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
        SceneManager.LoadScene(namaSceneKembali);
    }

    public void AnimasiJalan()
    {
        if (spawnedObject != null)
        {
            Animator anim = spawnedObject.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetBool("IsWalking", true);
            }
        }
    }

    public void AnimasiDiam()
    {
        if (spawnedObject != null)
        {
            Animator[] semuaAnim = spawnedObject.GetComponentsInChildren<Animator>();

            foreach (Animator anim in semuaAnim)
            {
                if (anim.runtimeAnimatorController != null)
                {
                    anim.SetBool("IsWalking", false);
                }
            }
        }
    }

    public void PutarSuara()
    {
        if (suaraAktif != null && audioSource != null)
        {
            audioSource.clip = suaraAktif;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Suara belum ada atau Audio Source kosong!");
        }
    }
}
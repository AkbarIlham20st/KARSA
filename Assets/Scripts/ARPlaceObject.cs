using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlaceObject : MonoBehaviour
{
    [Header("Aset Satwa")]
    public GameObject objectToSpawn;

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Animator anoaAnimator;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // LOGIKA 1: TAP LAYAR (Hanya untuk memunculkan hewan)
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            // Cegah tap tembus tombol UI (Opsional, tapi bagus agar saat tekan tombol hewan tidak pindah)
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
            {
                return;
            }

            Vector2 touchPosition = touch.position.ReadValue();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);

                    Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
                    lookPos.y = 0;
                    spawnedObject.transform.rotation = Quaternion.LookRotation(-lookPos);

                    anoaAnimator = spawnedObject.GetComponent<Animator>();
                }
                else
                {
                    // Jika ditap lagi, pindahkan posisi hewan (opsional)
                    spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }

    // --- FUNGSI UNTUK TOMBOL UI ---

    // Dipanggil saat tombol "DIAM" ditekan
    public void TombolIdle()
    {
        if (anoaAnimator != null)
        {
            // Matikan mode jalan -> Kembali ke Idle
            anoaAnimator.SetBool("IsWalking", false);
        }
    }

    // Dipanggil saat tombol "JALAN" ditekan
    public void TombolWalk()
    {
        if (anoaAnimator != null)
        {
            // Hidupkan mode jalan
            anoaAnimator.SetBool("IsWalking", true);
        }
    }
}
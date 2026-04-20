using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private float rotationSpeed = 0.2f;

    void Update()
    {
        // Cek jika ada sentuhan di layar
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Jika jari bergerak (Moved) di layar
            if (touch.phase == TouchPhase.Moved)
            {
                // Rotasi objek berdasarkan pergerakan jari horizontal (deltaPosition.x)
                // Menggunakan sumbu Y (atas-bawah) sebagai poros putar
                transform.Rotate(0, -touch.deltaPosition.x * rotationSpeed, 0);
            }
        }
    }
}
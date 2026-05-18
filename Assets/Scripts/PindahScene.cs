using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public void BukaScene(string namaSceneTujuan)
    {
        SceneManager.LoadScene(namaSceneTujuan);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI Quiz")]
    public GameObject panelQuiz;
    public TextMeshProUGUI txtPertanyaan;

    [Header("Data Kuis")]
    public List<DataKuis> daftarKuis = new List<DataKuis>();

    void Start()
    {
        StartCoroutine(TimerKuis());
    }

    IEnumerator TimerKuis()
    {
        while (true) 
        {
            yield return new WaitForSeconds(60f); 

            if (!panelQuiz.activeSelf) 
            {
                MunculkanKuisRandom();
            }
        }
    }

    void MunculkanKuisRandom()
    {
        if (daftarKuis.Count == 0) return;

        int indexRandom = Random.Range(0, daftarKuis.Count);
        DataKuis kuisDipilih = daftarKuis[indexRandom];

        txtPertanyaan.text = kuisDipilih.pertanyaan;

        panelQuiz.SetActive(true);
        Debug.Log("Kuis Muncul secara Otomatis!");
    }

    public void TutupKuis()
    {
        panelQuiz.SetActive(false);
    }
}

[System.Serializable]
public class DataKuis
{
    public string pertanyaan;
    public string[] jawaban;
    public int indexJawabanBenar;
}
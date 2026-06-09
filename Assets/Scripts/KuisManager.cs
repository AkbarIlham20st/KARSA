using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PertanyaanKuis
{
    [TextArea(2, 5)]
    public string soal;
    public string[] pilihanJawaban = new string[4];
    public int indeksJawabanBenar;
}

public class KuisManager : MonoBehaviour
{
    [Header("Bank Soal")]
    public List<PertanyaanKuis> daftarSoal;

    [Header("Referensi UI")]
    public GameObject panelKuis;
    public TextMeshProUGUI teksSoal;
    public TextMeshProUGUI[] teksPilihan;
    public Button[] tombolPilihan;

    [Header("Panel Feedback")]
    public GameObject panelBenar;
    public GameObject panelSalah;

    private int indexSoalSaatIni;
    private bool kuisSedangMuncul = false;

    void Start()
    {
        MulaiTimerKuis();
    }

    public void MulaiTimerKuis()
    {
        StartCoroutine(TimerKuisRoutine());
    }

    IEnumerator TimerKuisRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);

            if (!kuisSedangMuncul && daftarSoal.Count > 0)
            {
                MunculkanKuisRandom();
            }
        }
    }

    void MunculkanKuisRandom()
    {
        kuisSedangMuncul = true;
        panelKuis.SetActive(true);

        indexSoalSaatIni = Random.Range(0, daftarSoal.Count);
        PertanyaanKuis kuis = daftarSoal[indexSoalSaatIni];

        teksSoal.text = kuis.soal;

        for (int i = 0; i < tombolPilihan.Length; i++)
        {
            if (i >= kuis.pilihanJawaban.Length)
            {
                Debug.LogError("🚨 ERROR KETEMU: Pada soal '" + kuis.soal + "', ukuran 'Pilihan Jawaban' di Inspector masih kurang dari " + tombolPilihan.Length + "!");
                continue; 
            }

            teksPilihan[i].text = kuis.pilihanJawaban[i];

            int indexTombol = i;
            tombolPilihan[i].onClick.RemoveAllListeners();
            tombolPilihan[i].onClick.AddListener(() => CekJawaban(indexTombol));
        }
    }

    public void CekJawaban(int indexDipilih)
    {
        panelKuis.SetActive(false);

        if (indexDipilih == daftarSoal[indexSoalSaatIni].indeksJawabanBenar)
            panelBenar.SetActive(true);
        else
            panelSalah.SetActive(true);

        Invoke("TutupFeedback", 2f);
    }

    void TutupFeedback()
    {
        panelBenar.SetActive(false);
        panelSalah.SetActive(false);
        kuisSedangMuncul = false; 
    }

    public void TutupKuisManual()
    {
        if (panelKuis != null)
        {
            panelKuis.SetActive(false);
            kuisSedangMuncul = false; 
        }
    }
}
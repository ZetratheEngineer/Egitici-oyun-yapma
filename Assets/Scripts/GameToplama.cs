using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameToplama : MonoBehaviour
{
    [SerializeField]
    private GameObject KarePrefab;

    [SerializeField]
    private Transform ButonPanel;

    [SerializeField]
    private Transform soruPanel;

    [SerializeField]
    private TextMeshProUGUI soruText;

    [SerializeField]
    private TextMeshProUGUI PuanText;

    [SerializeField]
    private Sprite[] kareSprite;

    [SerializeField]
    private Transform SonucPanel;

    [SerializeField]
    private Transform SonucText;

    [SerializeField]
    private Image winImage;

    [SerializeField]
    private Button[] sonucButtons;
    

    List<int> bolumDegerleriListesi = new List<int>();

    private GameObject[] karelerDizisi = new GameObject[25];

    int bolunenSayi, bolenSayi;
    int soruKacýncý;
    int butonDegeri;
    int dogruSonuc;
    bool butonaBasilsinMi;
    public int KalanHak;
    int puan=0;
    public bool kazandýmMý;
    public int soruPuan = 15;
    KalanHaklar KalanHaklar;

    private void Awake()
    {
        KalanHak = 3;

        KalanHaklar=Object.FindAnyObjectByType<KalanHaklar>();

        KalanHaklar.KalanHaklarýKontrolEt(KalanHak);

        sonucButtons[0].GetComponent<Button>().enabled = false;

        sonucButtons[1].GetComponent<Button>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        butonaBasilsinMi = false;
        soruPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        kareleriOluþtur();
        PuanText.text= puan.ToString();
    }

    GameObject gecerliKare;

    public void kareleriOluþtur()
    {
        for (int i = 0; i < karelerDizisi.Length; i++)
        {
            GameObject kare = Instantiate(KarePrefab, ButonPanel);
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprite[0];
            kare.transform.GetChild(2).GetComponent<Image>().sprite = kareSprite[1];

            karelerDizisi[i] = kare;
            kare.SetActive(false); // Kareleri baþlangýçta gizle
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc", 3f);
    }

    void ButonaBasildi()
    {
        if(butonaBasilsinMi)
        {
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            Debug.Log(butonDegeri);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            SonucuKontrolEt();
        }
        
    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetComponent<Button>().interactable = false;
            Debug.Log("Dogru sonuc");
            PuanKazan();
            bolumDegerleriListesi.RemoveAt(soruKacýncý);
            if (bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliniAc();
            }
            else
            {
                kazandýmMý = true;
                OyunBitti();
            }

        }
        else { 
            Debug.Log("Yanlýþ sonuc");
            KalanHak--;
            KalanHaklar.KalanHaklarýKontrolEt(KalanHak);
            
            StartCoroutine(kaybol(gecerliKare));
        }
        if(KalanHak<=0)
        {
            kazandýmMý = false;
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        soruText.GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < 25; i++)
        {
            karelerDizisi[i].transform.GetComponent<Button>().interactable = false;
        }
        if (kazandýmMý)
        {
            Kazandýnýz();
        }
        else
        {
            Kaybettiniz();
        }
    }

    private void Kaybettiniz()
    {
        SonucPanel.GetComponent<Image>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().text = "Malesef Kaybettiniz";

        sonucButtons[0].transform.GetComponent<Image>().enabled = true;
        sonucButtons[0].transform.GetComponent<Button>().enabled = true;
        sonucButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
        sonucButtons[1].transform.GetComponent<Image>().enabled = true;
        sonucButtons[1].transform.GetComponent<Button>().enabled = true;
        sonucButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;

    }

    private void Kazandýnýz()
    {
        SonucPanel.GetComponent<Image>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().text= "Tebrikler Kazandýnýz";

        sonucButtons[0].transform.GetComponent<Image>().enabled = true;
        sonucButtons[0].transform.GetComponent<Button>().enabled = true;
        sonucButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
        sonucButtons[1].transform.GetComponent<Image>().enabled = true;
        sonucButtons[1].transform.GetComponent<Button>().enabled = true;
        sonucButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;

    }

    IEnumerator kaybol(GameObject galetta)
    {
        
        galetta.transform.GetChild(2).GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(2f);
        galetta.transform.GetChild(2).GetComponent<Image>().enabled = false;
    }
    
    IEnumerator DoFadeRoutine()
    {
        float fadeDuration = 0.2f; // Animasyon süresi (sn)
        float delayBetweenFade = 0.1f; // Her kare arasýndaki gecikme süresi (sn)

        foreach (var kare in karelerDizisi)
        {
            kare.SetActive(true); // Kareyi görünür yap

            CanvasGroup canvasGroup = kare.GetComponent<CanvasGroup>();
            float startAlpha = 0.0f; // Baþlangýç alfa deðeri
            float targetAlpha = 1f; // Hedef alfa deðeri

            float elapsedTime = 0.5f;

            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return elapsedTime;
            }

            // Hedef alfa deðerine ulaþýldýktan sonra belirli bir süre bekleme
            yield return new WaitForSeconds(delayBetweenFade);
        }
    }

    private void BolumDegerleriniTexteYazdir()
    {
        int rastgeleDeger;
        foreach (var kare in karelerDizisi)
        {
            rastgeleDeger = Random.Range(4, 12);
            bolumDegerleriListesi.Add(rastgeleDeger);
            kare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rastgeleDeger.ToString();
        }
        
    }

    public void tekrarButton()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(1);
    }

    public void anaMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void AyrýlButton()
    {
        SceneManager.LoadScene(0);
    }

    private float scaleDuration = 0.5f; // scaleDuration'ý sýnýf düzeyinde tanýmladýk

    void SoruPaneliniAc()
    {
        SoruyuSor();
        StartCoroutine(ScalePanelRoutine());
    }

    IEnumerator ScalePanelRoutine()
    {
        RectTransform panelRectTransform = soruPanel.GetComponent<RectTransform>();

        Vector3 initialScale = Vector3.zero; // Baþlangýç boyutu
        Vector3 targetScale = Vector3.one; // Hedef boyutu

        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            panelRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hedef boyuta ulaþýldýktan sonra gerekirse baþka iþlemler yapýlabilir
    }

    public void PuanKazan()
    {
        puan += soruPuan;
        PuanText.text = puan.ToString();
    }


    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);
     
        soruKacýncý = Random.Range(0, bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[soruKacýncý];

        bolunenSayi = bolenSayi * dogruSonuc;
        
        bolunenSayi = bolenSayi * bolumDegerleriListesi[soruKacýncý];
        
        butonaBasilsinMi = true;

        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString() + " = ?";
    }
}

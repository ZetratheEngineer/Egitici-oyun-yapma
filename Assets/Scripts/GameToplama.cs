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
    int soruKac�nc�;
    int butonDegeri;
    int dogruSonuc;
    bool butonaBasilsinMi;
    public int KalanHak;
    int puan=0;
    public bool kazand�mM�;
    public int soruPuan = 15;
    KalanHaklar KalanHaklar;

    private void Awake()
    {
        KalanHak = 3;

        KalanHaklar=Object.FindAnyObjectByType<KalanHaklar>();

        KalanHaklar.KalanHaklar�KontrolEt(KalanHak);

        sonucButtons[0].GetComponent<Button>().enabled = false;

        sonucButtons[1].GetComponent<Button>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        butonaBasilsinMi = false;
        soruPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        kareleriOlu�tur();
        PuanText.text= puan.ToString();
    }

    GameObject gecerliKare;

    public void kareleriOlu�tur()
    {
        for (int i = 0; i < karelerDizisi.Length; i++)
        {
            GameObject kare = Instantiate(KarePrefab, ButonPanel);
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprite[0];
            kare.transform.GetChild(2).GetComponent<Image>().sprite = kareSprite[1];

            karelerDizisi[i] = kare;
            kare.SetActive(false); // Kareleri ba�lang��ta gizle
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
            bolumDegerleriListesi.RemoveAt(soruKac�nc�);
            if (bolumDegerleriListesi.Count > 0)
            {
                SoruPaneliniAc();
            }
            else
            {
                kazand�mM� = true;
                OyunBitti();
            }

        }
        else { 
            Debug.Log("Yanl�� sonuc");
            KalanHak--;
            KalanHaklar.KalanHaklar�KontrolEt(KalanHak);
            
            StartCoroutine(kaybol(gecerliKare));
        }
        if(KalanHak<=0)
        {
            kazand�mM� = false;
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
        if (kazand�mM�)
        {
            Kazand�n�z();
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

    private void Kazand�n�z()
    {
        SonucPanel.GetComponent<Image>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().enabled = true;
        SonucText.GetComponent<TextMeshProUGUI>().text= "Tebrikler Kazand�n�z";

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
        float fadeDuration = 0.2f; // Animasyon s�resi (sn)
        float delayBetweenFade = 0.1f; // Her kare aras�ndaki gecikme s�resi (sn)

        foreach (var kare in karelerDizisi)
        {
            kare.SetActive(true); // Kareyi g�r�n�r yap

            CanvasGroup canvasGroup = kare.GetComponent<CanvasGroup>();
            float startAlpha = 0.0f; // Ba�lang�� alfa de�eri
            float targetAlpha = 1f; // Hedef alfa de�eri

            float elapsedTime = 0.5f;

            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return elapsedTime;
            }

            // Hedef alfa de�erine ula��ld�ktan sonra belirli bir s�re bekleme
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

    public void Ayr�lButton()
    {
        SceneManager.LoadScene(0);
    }

    private float scaleDuration = 0.5f; // scaleDuration'� s�n�f d�zeyinde tan�mlad�k

    void SoruPaneliniAc()
    {
        SoruyuSor();
        StartCoroutine(ScalePanelRoutine());
    }

    IEnumerator ScalePanelRoutine()
    {
        RectTransform panelRectTransform = soruPanel.GetComponent<RectTransform>();

        Vector3 initialScale = Vector3.zero; // Ba�lang�� boyutu
        Vector3 targetScale = Vector3.one; // Hedef boyutu

        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            panelRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hedef boyuta ula��ld�ktan sonra gerekirse ba�ka i�lemler yap�labilir
    }

    public void PuanKazan()
    {
        puan += soruPuan;
        PuanText.text = puan.ToString();
    }


    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);
     
        soruKac�nc� = Random.Range(0, bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[soruKac�nc�];

        bolunenSayi = bolenSayi * dogruSonuc;
        
        bolunenSayi = bolenSayi * bolumDegerleriListesi[soruKac�nc�];
        
        butonaBasilsinMi = true;

        soruText.text = bolunenSayi.ToString() + " : " + bolenSayi.ToString() + " = ?";
    }
}

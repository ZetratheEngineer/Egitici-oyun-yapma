using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class menuFade : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI HavaDurumuText; 
    
    [SerializeField]
    private TextMeshProUGUI SaatText;

    //public Dropdown menuDropdown ; // Unity arayüzünden sürüklenecek dropdown nesnesi
    public GameObject menuButton; // Unity arayüzünden sürüklenecek buton nesnesi
    public float fadeDuration = 2f; // Geçiþ süresi (saniye cinsinden)
    
    
    private CanvasGroup canvasGroup;
    private float currentAlpha = 0f;
    private float targetAlpha = 1f;
    private float alphaSpeed;
    public float delayTime = 0; 

    void Start()
    {
        
        canvasGroup = menuButton.GetComponent<CanvasGroup>();
        alphaSpeed = (targetAlpha - currentAlpha) / fadeDuration;
        //canvasGroup = menuDropdown.GetComponent<CanvasGroup>();
        alphaSpeed = (targetAlpha - currentAlpha) / fadeDuration;

        StartCoroutine(FadeEffectswithDelay());

        HavaDurumuYazma();
        SaatBilgi();
    }


    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void HavaDurumuYazma()
    {
        string Konum = "antalya";
        string api = "0eca13c407689a9d9d97fa7f19cf3078";
        string connection = "https://api.openweathermap.org/data/2.5/weather?q=" + Konum + "&mode=xml&lang=tr&units=metric&appid=" + api;
        XDocument weather = XDocument.Load(connection);
        var temp = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
        string inf = temp + "°C";
        HavaDurumuText.text = inf;
    }

    public void SaatBilgi()
    {
        SaatText.text = DateTime.Now.ToShortTimeString();
    }

    public void ExitButton() 
    { 
        Application.Quit();
    }

    IEnumerator FadeEffectswithDelay()
    {
        yield return new WaitForSeconds(delayTime);

            while (currentAlpha < targetAlpha)
            {
                currentAlpha += alphaSpeed * Time.deltaTime;
                canvasGroup.alpha = currentAlpha;
                yield return new WaitForSeconds(0.05f);
            }
        
  
    }
}

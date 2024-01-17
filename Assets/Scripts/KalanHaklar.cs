using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalanHaklar : MonoBehaviour
{
    [SerializeField]
    private GameObject KalanHak1, KalanHak2, KalanHak3;


    public void KalanHaklarýKontrolEt(int KalanHak)
    {
        switch (KalanHak)
        {
            case 3:
                KalanHak1.SetActive(true);
                KalanHak2.SetActive(true);
                KalanHak3.SetActive(true); 
                break;
            case 2:
                KalanHak1.SetActive(true);
                KalanHak2.SetActive(true);
                KalanHak3.SetActive(false);
                break;
            case 1:
                KalanHak1.SetActive(true);
                KalanHak2.SetActive(false);
                KalanHak3.SetActive(false);
                break;
                case 0:
                KalanHak1.SetActive(false); 
                KalanHak2.SetActive(false);
                KalanHak3.SetActive(false);
                break;
        }
    }
    
}

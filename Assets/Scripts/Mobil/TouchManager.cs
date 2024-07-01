using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchManager : MonoBehaviour
{
    public delegate void DokunmaEventDelegate(Vector2 swipePos);

    public static event DokunmaEventDelegate DragEvent;
    
    public static event DokunmaEventDelegate SwipeEvent;

    public static DokunmaEventDelegate TapEvent;

    private Vector2 dokunmaHareketi;

    [Range(50, 250)] public int minDragUzaklık = 100;   
    
    
    [Range(50,250)] public int minSuruklemeUzaklik = 200;

    [SerializeField] private TextMeshProUGUI taniTxt_1, taniTxt_2;

    public bool taniKullanilsinmi = false;

    private float tiklamaMaxSure = 0;
    public float ekranaTiklamaSuresi = 20;

    private void Start()
    {
        TaniYazdirFNC("","");
    }

    void TiklandiFNC()
    {
        if (TapEvent != null)
        {
            TapEvent(dokunmaHareketi);
        }
    }

    void SurukleFNC()
    {
        if (DragEvent != null)
        {
            DragEvent(dokunmaHareketi);
        }
    }

    void SurukleBittiFNC()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(dokunmaHareketi);
        }
    }

    void TaniYazdirFNC(string txt_1, string txt_2)
    {
        taniTxt_1.gameObject.SetActive(taniKullanilsinmi);
        taniTxt_2.gameObject.SetActive(taniKullanilsinmi);

        if (taniTxt_1 && taniTxt_2)
        {
            taniTxt_1.text = txt_1;
            taniTxt_2.text = txt_2;
        }
    }


    string SuruklemeTaniFNC(Vector2 surukelemeHareket)
    {
        string direction = "";

        if (Mathf.Abs(surukelemeHareket.x) > Math.Abs(surukelemeHareket.y))
        {
            direction = (surukelemeHareket.x >= 0) ? "sag" : "sol";
        }
        else
        {
            direction = (surukelemeHareket.y >= 0) ? "yukarı" : "aşağı";
        }

        return direction;
    }

private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                dokunmaHareketi = Vector2.zero;
                tiklamaMaxSure = Time.time + ekranaTiklamaSuresi;
                TaniYazdirFNC("","");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                dokunmaHareketi += touch.deltaPosition;

                if (dokunmaHareketi.magnitude > minDragUzaklık)
                {
                    SurukleFNC();
                    TaniYazdirFNC("surukleme kontrolu", dokunmaHareketi.ToString()+ "" + SuruklemeTaniFNC(dokunmaHareketi));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {

                if (dokunmaHareketi.magnitude > minSuruklemeUzaklik)
                {
                    SurukleBittiFNC();
                }
               else if (Time.time < tiklamaMaxSure)
                {
                    TiklandiFNC();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSettingsManager : MonoBehaviour
{
    private MyPlayerInput myPlayerInput;
    private TouchManager touchManager;

    public Slider dokunmaSlider;
    public Slider suruklemeSlider;
    public Slider dokunmaHizSlider;
    
    

    private void Start()
    {
        myPlayerInput = GameObject.FindObjectOfType<MyPlayerInput>();
        touchManager = GameObject.FindObjectOfType<TouchManager>();

        if (dokunmaSlider != null)
        {
            dokunmaSlider.value = 100;
            dokunmaSlider.minValue = 50;
            dokunmaSlider.maxValue = 150;
        }

        if (suruklemeSlider != null)
        {
            suruklemeSlider.value = 50;
            suruklemeSlider.minValue = 20;
            suruklemeSlider.maxValue = 250;
        }

        if (dokunmaHizSlider != null)
        {
            dokunmaHizSlider.value = 0.015f;
            dokunmaHizSlider.minValue = 0.05f;
            dokunmaHizSlider.maxValue = 0.5f;
        }
    }

    public void UpdateSettingsPanel()
    {
        if (dokunmaSlider != null && touchManager != null)
        {
            touchManager.minDragUzaklık = (int) dokunmaSlider.value;
        }

        if (suruklemeSlider != null && touchManager != null)
        {
            touchManager.minSuruklemeUzaklik = (int)suruklemeSlider.value;
        }

        if (dokunmaHizSlider != null && myPlayerInput != null)
        {
            myPlayerInput.minDokunmaZamani = dokunmaHizSlider.value;
        }
    }
}

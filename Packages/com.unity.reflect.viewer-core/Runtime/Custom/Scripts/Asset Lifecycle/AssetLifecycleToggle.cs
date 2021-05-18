using System.Collections;
using System.Collections.Generic;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class AssetLifecycleToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject slider;
    private bool toolActive;

    public AssetLifeCycleSliderControl MinMaxPropertyControl;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!toolActive)
        {
            slider.SetActive(true);
            toolActive = true;
        }
        else
        {
            slider.SetActive(false);
            toolActive = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }


}

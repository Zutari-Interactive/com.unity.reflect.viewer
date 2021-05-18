using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.Reflect;

public class LightInteractor : ClickInteractor
{
    private Manager manager;

    private string id;

    private bool lightsOn = true;

    void OnEnable()
    {
        manager = FindObjectOfType<LightManager>();
        if(manager == null)
        {
            Debug.LogWarning("no light manager present in scene");
        }
        ChangeColor();
        if (GetComponent<Renderer>())
        {
            gameObject.AddComponent<MeshCollider>();
        }
    }

    public override void Click()
    {
        base.Click();
        id = mData.GetParameter("Switch ID");
        if (!lightsOn)
        {
            lightsOn = true;
            ChangeColor();
            StopCoroutine(TurnLightsOff());
            StartCoroutine(TurnLightsOn());

        }
        else
        {
            lightsOn = false;
            ChangeColor();
            StopCoroutine(TurnLightsOn());
            StartCoroutine(TurnLightsOff());
        }

    }

    IEnumerator TurnLightsOff()
    {
        Debug.Log("clicked on " + id);
        LightGroup lightsGroup = manager.dict[id] as LightGroup;
        foreach( Light light in lightsGroup.group)
        {
            light.enabled = false;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        yield return null;
    }

    IEnumerator TurnLightsOn()
    {
        Debug.Log("clicked on " + id);
        LightGroup lightsGroup = manager.dict[id] as LightGroup;
        foreach (Light light in lightsGroup.group)
        {
            light.enabled = true;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        yield return null;
    }

    private void ChangeColor()
    {
        if (lightsOn)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.red;
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
        }
    }
}

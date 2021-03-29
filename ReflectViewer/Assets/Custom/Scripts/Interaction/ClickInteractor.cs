using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;

public class ClickInteractor : Interactor
{
    public delegate void OnClick(Metadata data, bool state);
    public event OnClick onClick;

    private bool lightsOn = true;

    public Metadata mData;

    private void OnMouseUp()
    {
        if (lightsOn)
        {
            Click();
            ChangeState();
        }
        else
        {
            Click();
            ChangeState();
        }
    }

    public virtual void Click()
    {
        mData = GetComponent<Metadata>();

        //if(mData != null)
        //{
        //    onClick(mData, lightsOn);
        //}
    }

    private void ChangeState()
    {
        lightsOn = !lightsOn;
    }

    //VR hand
    //mouse & touch
}

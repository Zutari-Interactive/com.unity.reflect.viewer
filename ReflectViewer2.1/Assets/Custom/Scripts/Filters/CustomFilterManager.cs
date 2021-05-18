using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFilterManager : MonoBehaviour
{
    public CustomFilterLibrary filterLibrary;

    public Transform root;

    public void StartManager()
    {
        filterLibrary.Setup(root);
    }
}

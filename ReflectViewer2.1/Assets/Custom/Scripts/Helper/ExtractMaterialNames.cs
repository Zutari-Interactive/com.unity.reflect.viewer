using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExtractMaterialNames : MonoBehaviour
{
    private List<string> matNames = new List<string>();

    public bool fetchNames;

    void OnValidate()
    {

    }
}

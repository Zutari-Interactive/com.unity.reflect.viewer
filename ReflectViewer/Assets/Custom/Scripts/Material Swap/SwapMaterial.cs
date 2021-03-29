using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using Zutari.Database;

public class SwapMaterial : CustomNode
{
    public ReplaceMaterialLibrary library;
    private List<string> matKeyWords = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        matKeyWords = library.FetchKeyWords();
    }

    public void SortObject(Metadata data)
    {
        Renderer rend = data.gameObject.GetComponent<Renderer>();

        if (rend == null)
            return;

        var mats = rend.sharedMaterials;
        for (int i = 0; i < mats.Length; i++)
        {
            for (int x = 0; x < matKeyWords.Count; x++)
            {
                if (mats[i].name.Contains(matKeyWords[x]))
                {
                    rend.material = library.FetchMaterial(matKeyWords[x]);
                    Debug.Log($"{matKeyWords[x]} material changed");
                }
            }
        }
    }

}

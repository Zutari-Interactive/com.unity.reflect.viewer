using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Elements.Database;

[ExecuteInEditMode]
public class EditorMaterialFinder : MonoBehaviour
{
    public ReplaceMaterialLibrary matLibrary;

    private List<Renderer> rends = new List<Renderer>();
    private List<string> keys = new List<string>();

    private string keyword;

    public bool swap;

    void OnValidate()
    {
        if (swap)
        {
            FindExistingRenderers();
            swap = false;
        }
    }

    public void FindExistingRenderers()
    {
        rends.Clear();
        keys.Clear();
        Scene s = SceneManager.GetActiveScene();
        GameObject[] roots = s.GetRootGameObjects();

        keys = matLibrary.FetchKeyWords();

        foreach (var item in roots)
        {
            Transform[] children = item.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                Renderer[] kids = children[i].GetComponentsInChildren<Renderer>();
                foreach (var rend in kids)
                {
                    rends.Add(rend);
                }
                    
            }
            
        }

        Debug.Log($"Renderer count = {rends.Count}");

        if (rends.Count > 0)
            AssessMaterials();
        else
            Debug.Log("No renderers found");
        
    }

    private void AssessMaterials()
    {
        for (int i = 0; i < rends.Count; i++)
        {
            Material[] eM = rends[i].sharedMaterials;

            for (int x = 0; x < eM.Length; x++)
            {
                if (KeywordMatch(eM[x]))
                {
                    eM[x] = ReplaceMaterial();
                }
                    
            }
            rends[i].sharedMaterials = eM;
        }
        Debug.Log("material assessment complete");
    }

    private Material ReplaceMaterial()
    {
       return matLibrary.FetchMaterial(keyword);
    }

    private bool KeywordMatch(Material mat)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (mat.name.Contains(keys[i]))
            {
                Debug.Log($"{keys[i]} found");
                keyword = keys[i];
                return true;
            }
        }

        return false;

    }
}

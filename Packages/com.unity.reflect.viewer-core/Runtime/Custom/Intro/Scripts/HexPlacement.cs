using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;

public class HexPlacement : MonoBehaviour
{
    public GameObject diamondPrefab;
    public Transform parent;

    [Header("Logos")]
    public Image[] logos;

    [Header("Background")]
    public Image background;
    public Color defaultBGColor;

    [Header("Animation Effect settings")]
    public float xOffset;
    public float yOffset;

    public float fallOff;
    public float scaleTime;
    public List<Color> colorSwatch = new List<Color>();
    public bool bounce;

    [Header("Animation Direction")]
    public bool horizontal;

    private Dictionary<int, List<GameObject>> hexRows = new Dictionary<int, List<GameObject>>();
    private bool open = false;
    private bool logoCheck = false;
    private RectTransform rt;
    private int xRowCount;
    private int yRowCount;
    



    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        CalculateRowsAndColumns();
        SpawnDiamonds();
    }

    private void CalculateRowsAndColumns()
    {
        xRowCount = (Screen.width / 100) + 2;
        Debug.Log($"Screen width = {Screen.width}");
        yRowCount = (Screen.height / 25) + 2;
        Debug.Log($"Screen height = {Screen.height}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartSequence();
        }

       if (bounce)
       {
           bounce = false;
           StartSequence();
       }
    }

    private void StartSequence()
    {
        StartCoroutine(SetDiamondScale());
    }

    IEnumerator SetDiamondScale()
    {
        Debug.Log("resize");
        if (!open)
        {
            Debug.Log("scale down");
            open = true;
            MatchColor();
            SelectLogo();
            LogoSequence();
            for (int i = 0; i < hexRows.Values.Count; i++)
            {
                ScaleDown(hexRows[i], i);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(scaleTime + 2f);
            bounce = true;
        }
        else
        {
            Debug.Log("scale up");
            open = false;
            LogoSequence();
            for (int i = 0; i < hexRows.Values.Count; i++)
            {
                ScaleUp(hexRows[i], i);
                yield return new WaitForEndOfFrame();
            }
            
            yield return new WaitForSeconds(scaleTime + 2f);
            bounce = true;

        }
        
    }

    private void SelectLogo()
    {
        if (!logoCheck)
        {
            logos[0].enabled = true;
            logos[1].enabled = false;
            logoCheck = true;
        }
        else
        {
            logos[1].enabled = true;
            logos[0].enabled = false;
            logoCheck = false;
        }
    }

    private void LogoSequence()
    {
        foreach (var item in logos)
        {
            if(item.isActiveAndEnabled)
            {
                if (open)
                    item.CrossFadeAlpha(1f, 1f, false);
                else
                    item.CrossFadeAlpha(0f, 0.5f, false);
            }
        }
        
    }

    private void ColorSwap(Color c, List<GameObject> tris)
    {
        Debug.Log("swap color");
        background.color = c;
        foreach (var item in tris)
        {
            var imgs = item.GetComponentsInChildren<Image>();
            foreach (var sp in imgs)
            {
                sp.color = c;
                
            }
        }
    }

    private void ScaleDown(List<GameObject> hex, int index)
    {
        float newScale = 0f;

        foreach (var item in hex)
        {
            var tris = item.GetComponentsInChildren<Image>();
            foreach (var x in tris)
            {
                //tween this scale rather
                Vector3 ns = new Vector3(newScale, newScale, 1f);
                Tween.LocalScale(x.transform, ns, scaleTime, 0f);
            }
        }
    }

    private void ScaleUp(List<GameObject> hex, int index)
    {
        float newScale = 0.98f;

        if (index > 0)
        {
            newScale = newScale - (fallOff * index);
        }

        SetBackgroundColor();

        foreach (var item in hex)
        {
            var tris = item.GetComponentsInChildren<Image>();
            foreach (var x in tris)
            {
                Vector3 ns = new Vector3(newScale, newScale, 1f);
                Tween.LocalScale(x.transform, ns, scaleTime, 0f);
            }
        }
    }

    private void SpawnDiamonds()
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 nextPos = corners[0];
        Debug.Log("origin pos = " + nextPos);
        
        if (horizontal)
            PlaceHorizontal(nextPos);
        else
            PlaceVertical(nextPos);

    }

    private void PlaceVertical(Vector3 np)
    {
        int index = 0;

        for (int i = 0; i < yRowCount; i++)
        {
            List<GameObject> row = new List<GameObject>();

            for (int x = 0; x < xRowCount + 1; x++)
            {
                GameObject newDiamond = Instantiate(diamondPrefab);
                newDiamond.transform.SetParent(parent);
                newDiamond.transform.position = np;
                np = new Vector3(np.x, np.y + 50f, np.z);
                row.Add(newDiamond);
            }

            hexRows.Add(index, row);
            index++;
            if (index % 2 == 0)
            {
                np = new Vector3(np.x + 50f, 0f, np.z);
            }
            else
            {
                np = new Vector3(np.x + 50f, 0f + yOffset, np.z);
            }


        }
    }

    private void PlaceHorizontal(Vector3 np)
    {
        int index = 0;

        for (int i = 0; i < yRowCount; i++)
        {
            List<GameObject> row = new List<GameObject>();

            for (int x = 0; x < xRowCount; x++)
            {
                GameObject newDiamond = Instantiate(diamondPrefab);
                newDiamond.transform.SetParent(parent);
                newDiamond.transform.position = np;
                np = new Vector3(np.x + xOffset, np.y, np.z);
                row.Add(newDiamond);
            }

            hexRows.Add(index, row);
            index++;
            if (index % 2 == 0)
            {
                np = new Vector3(0f, np.y + yOffset, np.z);
            }
            else
            {
                np = new Vector3(0f + 50f, np.y + yOffset, np.z);
            }

        }
    }

    

    private void MatchColor()
    {
        Debug.Log("match colors");
        Color nextCol = colorSwatch[UnityEngine.Random.Range(0, colorSwatch.Count - 1)];
        for (int i = 0; i < hexRows.Keys.Count; i++)
        {
            ColorSwap(nextCol,hexRows[i]);
        }
    }


    private void SetBackgroundColor()
    {
        background.color = defaultBGColor;
    }

}

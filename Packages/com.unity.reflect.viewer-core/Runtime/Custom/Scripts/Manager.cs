using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Dictionary<string, CategoryGroup> dict = new Dictionary<string, CategoryGroup>();
    public List<string> ids = new List<string>();
    public Interactor interactor;
}

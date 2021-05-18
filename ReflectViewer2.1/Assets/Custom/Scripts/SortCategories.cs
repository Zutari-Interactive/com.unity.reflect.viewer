using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Reflect.Model;
using UnityEditor;
using System;

/// <summary>
/// script must be placed on SyncPrefab, can filter objects by any parameter in the the MetaData and group according to common parameters (like by Category)
/// </summary>
namespace UnityEngine.Reflect
{
    [ExecuteInEditMode]
    public class SortCategories : CustomFilter
    {
        private Metadata[] metadata;

        private Dictionary<string, Transform> categories = new Dictionary<string, Transform>();

        private bool alreadyRun = false;

        //public variables
        [Tooltip("string of which parameter (in metadata) you would like to sort your objects by")]
        public string SearchParameter;
        public Transform root;
        public bool run = false;
        
        public void Sort()
        {
            CollectMetadata();
            alreadyRun = true;
        }
       
       private void Update()
       {
            //for edit mode primarily
           if (run)
           {
               CollectMetadata();
               alreadyRun = true;
               run = false;
           }
       }

        private void CollectMetadata()
        {
            if (alreadyRun)
            {
                Debug.LogError("You have already run this operation, to run again please delete and reimport this object");
                return;
            }
                
            metadata = GetComponentsInChildren<Metadata>();
            Debug.Log("metadata objects found: " + metadata.Length);
            SetupCategories();
        }

        private void SetupCategories()
        {
            for (int i = 0; i < metadata.Length; i++)
            {

                string findCategory = metadata[i].GetParameter(SearchParameter);
                Debug.Log(findCategory);
                if (findCategory != string.Empty)
                {
                    Comparison(findCategory, i);
                }

            }

            Debug.Log("categories " + categories.Count);
        }

        public void SortObject(Metadata data)
        {
            string findCategory = data.GetParameter(SearchParameter);
            if (findCategory != string.Empty)
            {
                Comparison(findCategory, data);
            }
        }

        private void Comparison(string category, Metadata metadata)
        {
            if (!categories.ContainsKey(category))
            {
                GameObject newParent = new GameObject();
                newParent.name = category;
                categories.Add(category, newParent.transform);
                metadata.gameObject.transform.SetParent(newParent.transform);
                newParent.transform.SetParent(root);
                
            }
            else
            {
                Transform temp = categories[category];
                metadata.gameObject.transform.SetParent(temp);
            }

            if (category.Equals("Lighting Devices"))
            {
                Debug.Log("Lighting devices parent found");
            }
        }

        private void SecondaryComparison(string category, GameObject go)
        {
            if (category.Equals("Lighting Devices"))
            {
                Debug.Log("Lighting devices parent found");
                go.AddComponent<LightGrouper>();
            }
            else if (category.Equals("Sprinklers"))
            {
                go.AddComponent<SprinklerGrouper>();
            }
            else if (category.Equals("Air Terminals"))
            {
                go.AddComponent<AirTerminalGrouper>();
            }
        }

        private void Comparison(string category, int i)
        {
            Debug.Log("comparison");

            if (!categories.ContainsKey(category))
            {
                GameObject newParent = new GameObject();
                newParent.name = category;
                categories.Add(category, newParent.transform);
                metadata[i].gameObject.transform.SetParent(newParent.transform);
                newParent.transform.SetParent(transform);


            }
            else
            {
                Transform temp = categories[category];
                metadata[i].gameObject.transform.SetParent(temp);
            }
        }
    }
}

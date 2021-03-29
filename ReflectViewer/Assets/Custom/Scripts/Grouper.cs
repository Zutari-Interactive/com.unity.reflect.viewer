using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.Reflect
{
    //script searches through all objects groups all matching IDs together
    public class Grouper : CustomNode
    {
        [Tooltip("string of which parameter (in metadata) you would like to sort your objects by")]
        public string SearchParameter;

        public bool group = false;

        private GameObject _metaDataObject;

        // [HideInInspector]
        public List<CategoryGroup> groups = new List<CategoryGroup>();

        [HideInInspector]
        public Manager manager;

        public GameObject MetaDataObject
        {
            get => _metaDataObject;
            private set => _metaDataObject = value;
        }

        public virtual void FindDeviceIDs(Metadata data)
        {
            Debug.Log("find ID");
            var id = data.GetParameter(SearchParameter);
            if (id != "")
            {
                if (!manager.dict.ContainsKey(id))
                {
                    _metaDataObject = data.gameObject;
                    CreateGroup(id);
                }
            }
            group = false;
        }

        public virtual void AddInteractor(Metadata data)
        {

        }

        public virtual void CreateGroup(string id)
        {
            Debug.Log("Creating group with ID = " + id);
            CategoryGroup group = new CategoryGroup();
            group.groupID = id;
            groups.Add(group);
            group.AddObjectToList(_metaDataObject);
            AddToManager(group);
        }

        public virtual void AddToManager(CategoryGroup group)
        {
            if(manager == null)
            {
                Debug.Log("manager null, finding manager");
                manager = GetComponent<Manager>();
                manager.dict.Clear();
                manager.ids.Clear();
            }
            Debug.Log($"adding {group.groupID} to group");
            manager.dict.Add(group.groupID, group);
            manager.ids.Add(group.groupID);
        }
    }
}



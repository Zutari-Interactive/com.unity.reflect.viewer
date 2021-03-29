using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.Reflect
{
    //script searches through all objects with Parameter - Switch ID, groups all matching IDs together
    [ExecuteInEditMode]
    [RequireComponent(typeof(LightManager))]
    public class LightGrouper : Grouper
    {
        //[Tooltip("Parent gameobject for the light fixtures - should be created if you've run Sort Category script on project parent - " +
        //        "from this parent all fixture switch IDs will be found and assigned to the matching device in a LightGroup")]
        //public GameObject lightFixturesParent;

        private void OnEnable()
        {
            manager = GetComponent<LightManager>();
            groups.Clear();
            manager.dict.Clear();
            manager.ids.Clear();
        }

        //private void OnValidate()
        //{
        //    if (group)
        //    {
        //        groups.Clear();
        //        manager.dict.Clear();
        //        //FindDeviceIDs();
        //    }
        //}

        public override void FindDeviceIDs(Metadata data)
        {
            base.FindDeviceIDs(data);
            FindFixtureIDs(data);
            group = false;
        }

        public override void AddInteractor(Metadata data)
        {
            var id = data.GetParameter("Category");
            if(id.Equals("Lighting Devices"))
            {
                if (!data.gameObject.GetComponent<LightInteractor>())
                    data.gameObject.AddComponent<LightInteractor>();
            }
        }

        private void FindFixtureIDs(Metadata data)
        {
            foreach (var item in groups)
            {
                var id = data.GetParameter(SearchParameter);
                if (item.groupID == id)
                {
                    Light attachedLight = data.transform.GetComponentInChildren<Light>();

                    if (attachedLight != null)
                    {
                        Debug.Log("light added to group " + item.groupID);
                        item.group.Add(attachedLight);
                    }

                    AddInteractor(data);
                }
                    
            }
        }

        public override void CreateGroup(string id)
        {
            Debug.Log("Creating light group with ID = " + id);
            
            LightGroup lightGroup = new LightGroup
            {
                groupID = id
            };
            groups.Add(lightGroup);
            AddToManager(lightGroup);
            
        }

        //private void AddToManager(LightGroup lightGroup)
        //{
        //    manager.dict.Add(lightGroup.groupID, lightGroup);
        //}
    }
}

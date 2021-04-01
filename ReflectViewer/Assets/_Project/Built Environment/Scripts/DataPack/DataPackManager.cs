using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;

public class DataPackManager : CustomNode
{
    public string[] Ids;
 

    public void FindID(Metadata data)
    {
        foreach (var item in Ids)
        {
            string id = data.GetParameter("Id");
            if (item.Equals(id))
            {
                CreateDataPack(data.gameObject, id);
            }
        }
    }

    private void CreateDataPack(GameObject g, string id)
    {
        Debug.Log("data object found");
        DataPackInteractor dp = g.AddComponent(typeof(DataPackInteractor)) as DataPackInteractor;
        dp.SetAddress(id);
    }
}

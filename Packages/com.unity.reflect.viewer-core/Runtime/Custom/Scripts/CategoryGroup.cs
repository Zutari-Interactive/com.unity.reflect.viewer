using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

[Serializable]
public class CategoryGroup
{
    public string groupID;
    public List<Object> group = new List<Object>();

    public virtual void AddObjectToList(Object newObject)
    {
        group.Add(newObject);
    }

    public virtual void SaveGroupID(string id)
    {
        groupID = id;
    }
}

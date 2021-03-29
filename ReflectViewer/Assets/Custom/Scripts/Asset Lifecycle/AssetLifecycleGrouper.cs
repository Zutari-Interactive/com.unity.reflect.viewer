using UnityEngine;
using UnityEngine.Reflect;


[ExecuteInEditMode]
[RequireComponent(typeof(AssetLifecycleManager))]
public class AssetLifecycleGrouper : Grouper
{
    #region VARIABLES

    public static AssetLifecycleGrouper Instance;

    #endregion
    #region UNITY METHODS

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        manager = GetComponent<AssetLifecycleManager>();
        groups.Clear();
        manager.dict.Clear();
        manager.ids.Clear();
    }

    #endregion

    #region METHODS

    public override void FindDeviceIDs(Metadata data)
    {
        base.FindDeviceIDs(data);
    }

    public override void CreateGroup(string id)
    {
        AssetLifeCycleGroup assetLifeCycleGroup = new AssetLifeCycleGroup
        {
            groupID = id
        };
        groups.Add(assetLifeCycleGroup);
        assetLifeCycleGroup.AddObjectToList(MetaDataObject);
        AddToManager(assetLifeCycleGroup);
    }

    public override void AddToManager(CategoryGroup @group)
    {
        base.AddToManager(@group);
    }

    #endregion
}

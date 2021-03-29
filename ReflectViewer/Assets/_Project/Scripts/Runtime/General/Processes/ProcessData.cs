using System;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using Zutari.Database;
using Zutari.LifeCycle;

namespace Zutari.General
{
    public class ProcessData : MonoBehaviour
    {
        #region VARIABLES

        [Header("Asset State Materials")]
        public AssetStateMaterialDatabase AssetStateMaterialDatabase;

        public static Action<string> OnProcessData;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            OnProcessData += ProcessJsonData;
        }

        #endregion

        #region METHODS

        public void ProcessJsonData(string data)
        {
            JSONArray jsonArray = (JSONArray)JSON.Parse(data);

            string nameKey = "Name";
            string initialYearKey = "Initial";
            string afterYearKey = "After_";

            int initialYear = 2020;
            int timeLapse = 15;

            string instanceName = "";
            double degredationValue = 0;
            AssetState assetState = AssetState.New;

            int length = jsonArray.Count;
            for (int i = 0; i < length; i++)
            {
                instanceName = jsonArray[i][nameKey];
                degredationValue = jsonArray[i][initialYearKey];
                assetState = DetermineAssetState(degredationValue);

                // Find the Asset with the Required Instance Name/ID
                GameObject asset = AssetLifecycleGrouper.Instance.groups.Find(group =>
                {
                    return @group.groupID == instanceName
                        ? @group.@group[0]
                        : null;
                }).@group[0] as GameObject;

                // If no Asset is Found Continue searching
                if (!asset) continue;

                AssetLifeCycle assetLifeCycle = asset.AddComponent<AssetLifeCycle>();
                assetLifeCycle.CurrentLifeCycle =
                    new LifeCycleState(assetState, (float) degredationValue, DetermineAssetMaterial(assetState));

                // Add the Initial Year to the Dictionary
                assetLifeCycle.LifeCycleDictionary.Add(initialYear, assetLifeCycle.CurrentLifeCycle);
                // Apply the Changes to the Asset for the Initial Year
                assetLifeCycle.OnYearChanged(initialYear);

                // Determine Each years Asset Life Cycle
                for (int j = initialYear + 1; j <= initialYear + timeLapse; j++)
                {
                    string newAfterYearKey = $"{afterYearKey}{j}";
                    degredationValue = jsonArray[i][newAfterYearKey];
                    assetState = DetermineAssetState(degredationValue);
                    assetLifeCycle.LifeCycleDictionary
                                  .Add(j,
                                       new LifeCycleState(assetState, (float) degredationValue,
                                                          DetermineAssetMaterial(assetState)));
                }
            }
        }

        private AssetState DetermineAssetState(double value)
        {
            if (value >= 0   && value < 2.5) return AssetState.Replace;
            if (value >= 2.5 && value < 3.5) return AssetState.SignificantOrMajorRepair;
            if (value >= 3.5 && value < 4) return AssetState.SpecificOrMinorMaintenance;
            if (value >= 4   && value < 4.5) return AssetState.RoutineMaintenance;
            return AssetState.New;
        }

        private Material DetermineAssetMaterial(AssetState state)
        {
            return AssetStateMaterialDatabase.Get(state);
        }

        public static void CallProcessData(string data)
        {
            OnProcessData?.Invoke(data);
        }

        #endregion
    }
}

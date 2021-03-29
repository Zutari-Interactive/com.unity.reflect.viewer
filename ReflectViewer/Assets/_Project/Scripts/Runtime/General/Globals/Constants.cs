using System.IO;
using Unity.TouchFramework;
using UnityEngine;

namespace Zutari.General
{
    public class Constants : MonoBehaviour
    {
        #region VARIABLES

        public static string JsonPath = string.Empty;
        public static string Croissant => Path.Combine(Application.streamingAssetsPath, "croissant.exe");

        public static MinMaxPropertyControl MinMaxPropertyControl;

        #endregion

        #region UNITY METHODS

        #endregion

        #region METHODS

        #endregion
    }
}

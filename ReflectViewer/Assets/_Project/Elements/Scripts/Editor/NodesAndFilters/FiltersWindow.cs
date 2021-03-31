#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Zutari.Elements.Editors
{
    public class FiltersWindow : EditorWindow
    {
        #region WINDOW

        public static FiltersWindow Window;

        public static void OpenWindow()
        {
            Window = GetWindow<FiltersWindow>("Filter Window");
            Window.Show();
        }

        #endregion

        #region VARIABLES

        #endregion

        #region UNITY METHODS

        #endregion

        #region METHODS

        #endregion
    }
}
#endif

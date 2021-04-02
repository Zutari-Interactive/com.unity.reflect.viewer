#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Databases.ConnectionStrings
{
    [CustomEditor(typeof(MySQLString))]
    public class MySQLStringEditor : Editor
    {
    }
}
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.Reflect.MeasureTool;

namespace Elements.UI
{
    [Serializable]
    public struct ElementsExternalStateData : IEquatable<ElementsExternalStateData>
    {
        public MeasureToolStateData measureToolStateData;

        public override string ToString()
        {
            return ToString("MeasureToolStateData {0}");
        }

        public string ToString(string format)
        {
            return string.Format(format, measureToolStateData);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = measureToolStateData.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is UIStateData other && Equals(other);
        }

        public bool Equals(ElementsExternalStateData other)
        {
            return measureToolStateData == other.measureToolStateData;
        }

        public static bool operator ==(ElementsExternalStateData a, ElementsExternalStateData b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ElementsExternalStateData a, ElementsExternalStateData b)
        {
            return !(a == b);
        }
    }
}
